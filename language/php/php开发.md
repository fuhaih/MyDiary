# edusoho
语言: php
框架：Symfony 

# php 安装包

## thread safe 和 non thread safe

需要先了解PHP的两种执行方式：ISAPI和FastCGI。   
线程安全模式是为ISAPI执行方式准备的，因为PHP模块都不是线程安全的。  
而FastCGI执行方式是以单一线程来执行操作，所以不需要安全模式

组合：
线程安全模式 + ISAPI + iis
非线程安全模式 +  FastCGI + （iis/nginx/apache）

# window 安装php

**non thread safe + FastCGI + iis**

1、首先下载non thread safe模式的安装包，解压到D:\php目录下

2、复制一份php.ini-development ，更改名称为php.ini

3、编辑php.ini

```ini
extension_dir = "ext" 
doc_root = "D:\wwwroot"
```
4、配置环境变量

```
D:\php
```
cmd命令测试

```
hph --help
```


5、iis配置

IIS管理器主页->IIS->处理程序映射->添加模块映射

填写内容
```
请求路径：*.php
模块：FastCgiModule
可执行文件：D:\php\php-cgi.exe
名称：FastCGI
```

如果没有FastCgiModule模块可以选择，说明还没有安装，在`启动或关闭Windows功能`中选择`CGI`进行安装就行了   
Internet Information Services->万维网服务->应用程序开发功能->CGI

6、验证

在D:\wwwroot下创建一个hello.php文件

```php
<html> 
  <head> 
    <title>World</title> 
  </head>

  <body> 
    <?php echo "Hello world" ?> 
  </body> 
</html> 
```

iis中新建一个网站，监听8088，目录指向D:\wwwroot
浏览器打开localhost:8088\hello.php

