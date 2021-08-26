# 1、http组成

## 1.1 组成

## 1.2 传参

### 1.2.1 url传参

通过url的QueryString传参
```h
POST /test.html?userid=1&name=test HTTP/1.1
```

在url中通过`&`符号来连接参数，传输到后端。

### 1.2.2 body传参

[body传参](#21-ContentType)

# 2、 header

## 2.1 ContentType

常用的几个ContentType类型

### 2.1.1 json

`application/json`
```json
POST /test.html HTTP/1.1
Host: example.org
Content-Type: application/json

{"userid":1,"usename":"root"}
```

### 2.1.2 urlencoded

`application/x-www-form-urlencoded`
```
POST /test.html HTTP/1.1
Host: example.org
Content-Type: application/x-www-form-urlencoded

user=root&pwd=root
```

form表单默认的Content-Type就是`application/x-www-form-urlencoded`,该类型的数据格式和url传参相似，都是通过`&`符号来连接参数，`application/x-www-form-urlencoded`则是把参数放在body中，url就是直接放在url末尾，一些比较重要的数据推荐还是放在body中，在https访问时能够被加密，而放在url中会直接暴露在外，不安全。


### 2.1.3 formdata

`multipart/form-data`

格式：

```h
POST /test.html HTTP/1.1
Host: example.org
Content-Type: multipart/form-data;boundary="boundary"

--boundary
Content-Disposition: form-data; name="field1"

value1
--boundary
Content-Disposition: form-data; name="field2"; filename="image.png"
Content-Type: image/png  

...contents of image.png...  
--boundary--
```
`multipart/form-data`格式会以`boundary`来分隔每个参数，http请求格式中，header和body会有以后间隔，所以这里是从--boundary开始是body内容，每个参数的格式就是
```c
//--boundary
//参数描述Content-Disposition、Content-Type
//空格
//参数数据
```
body最后还以`--boundary--`结束，这里注意，`--boundary--`后面需要换行，避免影响到下一个http请求。




### 2.1.4 其他格式

其他格式也是可以，但是需要告诉浏览器或者ajax如何序列化数据

## 2.2 Accept

这个是数据格式协商，和服务器协商能接受什么样的数据格式。

# 扩展问题

1、在浏览器地址栏输入一个 URL 后回车，背后发生了什么

* dns域名服务器解析域名，获取ip，根据ip找到服务器，建立tcp连接

-->延伸问题 tcp连接三次握手和四次挥手

-->延伸问题 ip协议五元组

-->延伸问题 tcp滑动窗口

-->延伸问题 tcp协议和udp协议区别

* 封装请求报文

请求行：get /dir/html HTTP/1.1

请求头：Host:www.test.com

请求体: name=test&age=12

请求行中包含了请求方法、uri、协议版本 

-->延伸问题 协议1.1和1.2的区别

-->延伸问题 请求体格式/http参数传递几种格式

-->延伸问题 各种请求方法的含义，特殊请求方法OPTIONS、PATCH

-->延伸问题 rest标准

-->延伸问题 http常用状态码及含义

-->延伸问题 http常用请求头（以及项目中使用到的请求头）

汉语程序中使用到的与断点下载有关的请求头

身份认证请求头

-->延伸问题 nginx七层转发和lvs五层转发

-->延伸问题 跨域问题、原理（几种 使用js 代理 vue可以使用代理进行跨域访问 发布时候可以使用nginx代理来处理跨域访问）

-->延伸问题 multipart/form-data、x-www-form-urlencode、json、querystring几种传递参数方式 

* multipart/form-data 方便 XSS 过滤，可以用来上传二进制数据，但是传复杂对象 (多级属性) 麻烦
* x-www-form-urlencode form表单的默认的上传格式
* JSON 传复杂对象方便，但是 XSS 过滤麻烦
* querystring 前面几个都是放在body里的，querystring是在url中的，比较容易被截获，不能存放重要的参数和数据






