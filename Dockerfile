FROM mono:onbuild
MAINTAINER venantvr

RUN export executable=$(ls --format single-column /usr/src/app/build/*.exe) && \
	echo mono ${executable} > /usr/src/app/build/launcher.sh && \
        chmod +x /usr/src/app/build/launcher.sh

RUN echo Hello, $(ls --format single-column /usr/src/app/build/*.exe) will start on boot ! 

CMD [ "/bin/sh", "/usr/src/app/build/launcher.sh" ]

EXPOSE 1234

