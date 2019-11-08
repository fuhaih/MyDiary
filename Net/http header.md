>Authorization

用来验证，一般是存放token

>Content-Type

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