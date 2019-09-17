# 基本概念

## 模块modules

Apache本身不会处理很复杂的需求，是通过不同的模块，用不同的协议来处理不同的需求和文件。

**php例子**
```Apache
### fastcgi 

## 加载模块
LoadModule fcgid_module modules/mod_fcgid.so

## 模块配置
<IfModule fcgid_module>
## php路径
FcgidInitialEnv PHPRC "D:/php"
FcgidInitialEnv PHP_FCGI_MAX_REQUESTS      1000
FcgidMaxRequestsPerProcess       1000
FcgidMaxProcesses             15
FcgidIOTimeout             120
FcgidIdleTimeout                120
AddType application/x-httpd-php .php
<Files ~ "\.php$>"
  AddHandler fcgid-script .php
  ## 用来处理php文件的fastcgi路径   
  FcgidWrapper "D:/php/php-cgi.exe" .php
</Files>
</IfModule>
```
`mod_fcgid.so`需要[下载](https://www.apachelounge.com/download/)，一些比较常用的模块会在Apache的`modules`目录下。

## 虚拟主机配置

使用虚拟主机可以进行多个站点的配置。  
>首先需要开启虚拟主机配置,并添加监听端口

```sh
Include conf/extra/httpd-vhosts.conf
#有几个虚拟主机就要配置几个Listen，还有一个Listen是Apache默认目录的Listen
Listen 8082
```

>在`conf/extra/httpd-vhosts.conf`文件中进行虚拟主机的配置

```apache
<VirtualHost *:8082>
  # 站点目录，在edusoho项目的web目录下
  DocumentRoot D:/wwwroot/web
  DirectoryIndex app.php
  ServerAdmin localhost:8082
  ServerName localhost:8082
  ErrorLog "D:/wwwroot/web/dummy-host2.localhost-error.log"
  CustomLog "D:/wwwroot/web/dummy-host2.localhost-access.log" combined
  <Directory "D:/wwwroot/web">
    # enable the .htaccess rewrites
    Options Indexes FollowSymLinks 
    Options +ExecCGI
    # URL rewrite 配置，All允许项目web目录下的.htaccess来配置URL rewrite
    # 在 AllowOverride 设置为 None 时， .htaccess 文件将被完全忽略
    AllowOverride All
    Order Allow,Deny
    #权限配置
    Allow from all
    Require all granted
  </Directory>
</VirtualHost>
```
