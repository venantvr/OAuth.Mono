﻿var loginData = "grant_type=password&username=test.test@mail.com&password=test123";

var xmlhttp = new XMLHttpRequest();
xmlhttp.onreadystatechange = function() {
    if (xmlhttp.readyState === 4 && xmlhttp.status === 200) {
        alert(xmlhttp.responseText);
    }
};
xmlhttp.open("POST", "http://127.0.0.1:1234/token", true);
xmlhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
xmlhttp.send(loginData);