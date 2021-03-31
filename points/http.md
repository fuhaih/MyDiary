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

-->延伸问题 请求体格式/http参数传递集中格式

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






