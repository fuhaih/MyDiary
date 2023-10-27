>Authorization

用来验证，一般是存放token

>Content-Type、Content-Disposition

请求都会携带有Content-Type

如果是文件下载，Content-Type="application/octet-stream",并且会再携带上`Content-Disposition`头来返回文件名 
`Content-Type: application/octet-stream
Content-Disposition: attachment;filename=test.xlsx`

返回数据的类型 [对照表](http://tool.oschina.net/commons/) 

>Accept-Encoding和Content-Encoding

`Accept-Encoding：gzip, deflate, br`表示浏览器支持的压缩格式，发送请求的时候会随请求发送到服务器，服务器如果支持某种压缩格式，就可以用该格式进行数据压缩，比如说支持gzip，就会用gzip格式压缩数据，然后在Response Headers中加上 `Content-Encoding：gzip`

>access-control-allow-origin、access-control-allow-methods、access-control-max-age

这几个请求头是用来处理跨域问题的：

跨域产生的原因是因为浏览器的同源策略，带有src的标签不受同源策略约束，其他资源访问受到同源策略的约束。

请求资源->服务器返回资源->浏览器接收到资源，发现资源不同源，拒绝使用;

要解决跨域问题，可以在response header中添加下列头;
```
Access-Control-Allow-Origin: *
Access-Control-Allow-Methods: POST,OPTIONS,GET
Access-Control-Max-Age: 3600
Access-Control-Allow-Headers: accept,x-requested-with,Content-Type
Access-Control-Allow-Credentials: true
```
请求资源->服务器返回资源->浏览器接收到资源，发现资源不同源，但是response header中包含了Access-Control-Allow-Origin，说明服务器允许该资源跨域访问，所以浏览器也就正常接收资源。

>Accept-Ranges:bytes

该请求头是在response中使用，表示客户端可以进行范围查询，该请求头一般用在文件下载中，进行范围查询可以支持断点下载。

服务端加上`Accept-Ranges`后，客户端请求时可以带上`Range`头，请求通过`Range`头来访问时，服务器的头文件会包含`Content-Range`。

请求流程：

客户端请求api -> 服务端头 `Accept-Ranges: bytes` -> 客户端请求头`Range: 0-499` ->服务端返回206状态码，服务端头 `Content-Range: 0-499/1024`;`Content-Range`后面的`/`的数值为文件的总长度。

>Cookie 、Set-Cookie

这两个是cookie操作相关的headers     
Cookie是Request Headers   
Set-Cookie是Response Headers    

服务器端的HttpContext是可以进行cookie操作的，但是不会直接写到cookie里，cookie是保存在客户端的。
当服务的进行cookie操作时，会在Set-Cookie中返回相应的信息，客户端接收到后，发现有`Set-Cookie` Response Headers,就会把里面的内容写入到cookie中。

客户端每次访问时，都会根据domain等信息来查找可以发送过去的cookie，然后写入到`Cookie` Request Headers中。

>Etag

Etag、Last-Modified文件缓存相关的Response Headers     
If-Non-Match、If-Modified-Since文件缓存相关的Request Headers 


