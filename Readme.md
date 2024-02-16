# SignalRChat

This is a sample application that demonstrates how to use SignalR to build a real-time chat application.

## Post Message to User Group

To post a message to a user group, you can use the following HTTP request:

````
POST /message/postMessage?groupName=SGroup&userName=PostMan&Message=Hello%20from%20Curl HTTP/1.1
User-Agent: PostmanRuntime/7.36.0
Accept: */*
Postman-Token: 17f8c76e-fda2-400c-9a9f-abb90128b431
Host: localhost:8091
Accept-Encoding: gzip, deflate, br
Connection: keep-alive
Content-Length: 0
 
HTTP/1.1 200 OK
Content-Length: 0
Date: Thu, 15 Feb 2024 11:53:17 GMT
Server: Kestrel
````