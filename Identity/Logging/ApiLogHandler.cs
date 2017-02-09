﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Routing;
using Newtonsoft.Json;

namespace Identity.Logging
{
    public class ApiLogHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var apiLogEntry = CreateApiLogEntryWithRequestData(request);
            if (request.Content != null)
            {
                await request.Content.ReadAsStringAsync()
                    .ContinueWith(task => { apiLogEntry.RequestContentBody = task.Result; }, cancellationToken);
            }

            return await base.SendAsync(request, cancellationToken)
                .ContinueWith(task =>
                              {
                                  var response = task.Result;

                                  // Update the API log entry with response info
                                  apiLogEntry.ResponseStatusCode = (int) response.StatusCode;
                                  apiLogEntry.ResponseTimestamp = DateTime.Now;

                                  if (response.Content != null)
                                  {
                                      apiLogEntry.ResponseContentBody = response.Content.ReadAsStringAsync().Result;
                                      apiLogEntry.ResponseContentType = response.Content.Headers.ContentType.MediaType;
                                      apiLogEntry.ResponseHeaders = SerializeHeaders(response.Content.Headers);
                                  }

                                  // TODO: Save the API log entry to the database

                                  return response;
                              }, cancellationToken);
        }

        private ApiLogEntry CreateApiLogEntryWithRequestData(HttpRequestMessage request)
        {
            var context = request.GetRequestContext();
            //var routeData = request.GetRouteData();

            return new ApiLogEntry
                   {
                       Application = ConfigurationManager.AppSettings["ApplicationName"],
                       User = context.Principal.Identity.Name,
                       Machine = Environment.MachineName,
                       RequestMethod = request.Method.Method,
                       RequestHeaders = SerializeHeaders(request.Headers),
                       RequestTimestamp = DateTime.Now,
                       RequestUri = request.RequestUri.ToString()
                   };
        }

        private string SerializeRouteData(IHttpRouteData routeData)
        {
            return JsonConvert.SerializeObject(routeData, Formatting.Indented);
        }

        private string SerializeHeaders(HttpHeaders headers)
        {
            var dict = new Dictionary<string, string>();

            foreach (var item in headers.ToList())
            {
                if (item.Value != null)
                {
                    var header = item.Value.Aggregate(string.Empty, (current, value) => current + value + " ");

                    // Trim the trailing space and add item to the dictionary
                    header = header.TrimEnd(" ".ToCharArray());
                    dict.Add(item.Key, header);
                }
            }

            return JsonConvert.SerializeObject(dict, Formatting.Indented);
        }
    }
}