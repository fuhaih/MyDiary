# nginx离线安装

## 安装编译工具和依赖

```cmd
yum install gcc-c++
yum install -y pcre pcre-devel
yum install -y zlib zlib-devel
yum install -y openssl openssl-devel
```

## 到官网下载nginx包

`nginx-1.24.0.tar.gz` 

## 编译配置

把包放在指定目录下，这里放在了/home/package/目录下，然后解压

```
tar -zxvf nginx-1.24.0.tar.gz
```

进入解压目录

```
cd nginx-1.24.0
```

配置安装路径、模块等信息

```
./configure --prefix=/usr/share/nginx --sbin-path=/usr/sbin/nginx --modules-path=/usr/lib64/nginx/modules --conf-path=/etc/nginx/nginx.conf --error-log-path=/var/log/nginx/error.log --http-log-path=/var/log/nginx/access.log --http-client-body-temp-path=/var/lib/nginx/tmp/client_body --http-proxy-temp-path=/var/lib/nginx/tmp/proxy --http-fastcgi-temp-path=/var/lib/nginx/tmp/fastcgi --http-uwsgi-temp-path=/var/lib/nginx/tmp/uwsgi --http-scgi-temp-path=/var/lib/nginx/tmp/scgi --pid-path=/run/nginx.pid --lock-path=/run/lock/subsys/nginx --user=nginx --group=nginx  --with-stream
```

如果`./configure`不配置，安装路径和执行文件等会在`/usr/local/nginx`目录下，和常用的不太一致，相当于是安装第三方包的路径


