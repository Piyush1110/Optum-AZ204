FROM nginx:latest

LABEL Auhor="Sonu Sathyadas"
LABEL Location="Mumbai"

WORKDIR /usr/share/nginx/html

COPY ./ ./

#ENV ConnectionString="http://localhost:2334"

RUN echo "Hello"

EXPOSE 80
EXPOSE 443

ENTRYPOINT [ "nginx", "-g", "daemon off;" ]