# 安装 
## tar安装

> 安装依赖

```sh
sudo yum -y install gcc gcc-c++ # nginx 编译时依赖 gcc 环境
sudo yum -y install pcre pcre-devel # 让 nginx 支持重写功能
# zlib 库提供了很多压缩和解压缩的方式，nginx 使用 zlib 对 http 包内容进行 gzip 压缩
sudo yum -y install zlib zlib-devel
# 安全套接字层密码库，用于通信加密
$ sudo yum -y install openssl openssl-devel
```
>解压包

下载并解压tar包

```sh
# 进入到常用的存放安装包路径下
cd /usr/local/package 
# 获取安装包
wget -c https://nginx.org/download/nginx-1.18.0.tar.gz
# 解压安装包
tar -zxvf nginx-1.18.0.tar.gz
# 进入解压目录
cd nginx-1.18.0
```


>查看模块 并安装
```sh
# 通过configure命令查看关于stream模块的状态 disable表示已经安装，目前不可使用
./configure --help | grep stream
```
要使用到nginx的stream模块，所以查看一下steam模块是否已经安装，发现状态是enable，也就是可安装的，那就配置一下

```sh
# 配置
./configure --with-stream
# 安装
make & make install
```
安装好之后配置信息是在`/usr/local/nginx`下

>nginx启动

在`/usr/lib/systemd/system`下创建一个nginx.service文件

```service
[Unit]
Description=nginx - high performance web server
Documentation=http://nginx.org/en/docs/
After=network-online.target remote-fs.target nss-lookup.target
Wants=network-online.target

[Service]
Type=forking
PIDFile=/var/run/nginx.pid
ExecStart=/usr/sbin/nginx -c /etc/nginx/nginx.conf
ExecReload=/bin/kill -s HUP $MAINPID
ExecStop=/bin/kill -s TERM $MAINPID

[Install]
WantedBy=multi-user.target

```
这里的PIDFile路径要和nginx配置文件中的pid路径一致,
nginx配置文件在`/usr/local/nginx/conf/nginx.conf`,修改其pid路径为`/var/run/nginx.pid`

```conf

#user  nobody;
worker_processes  1;

#error_log  logs/error.log;
#error_log  logs/error.log  notice;
#error_log  logs/error.log  info;

pid        /var/run/nginx.pid;


events {
    worker_connections  1024;
}


http {
    include       mime.types;

    default_type  application/octet-stream;

    #log_format  main  '$remote_addr - $remote_user [$time_local] "$request" '
    #                  '$status $body_bytes_sent "$http_referer" '
    #                  '"$http_user_agent" "$http_x_forwarded_for"';

    #access_log  logs/access.log  main;

    sendfile        on;
    #tcp_nopush     on;

    #keepalive_timeout  0;
    keepalive_timeout  65;

    #gzip  on;

    server {
        listen       80;
        server_name  localhost;

        location / {
            root   html;
            index  index.html index.htm;
        }

        error_page   500 502 503 504  /50x.html;
        location = /50x.html {
            root   html;
        }

    }

}

```

启动nginx

```sh
systemctl start nginx
```

>nginx 配置

创建conf.d文件夹

```
mkdir conf.d
```

把http 模块单独放在conf.d文件夹中

`include /usr/local/nginx/conf/conf.d/*.conf;`

```conf

#user  nobody;
worker_processes  1;

#error_log  logs/error.log;
#error_log  logs/error.log  notice;
#error_log  logs/error.log  info;

pid        /var/run/nginx.pid;


events {
    worker_connections  1024;
}


http {
    include       mime.types;
    include /usr/local/nginx/conf/conf.d/*.conf;

    default_type  application/octet-stream;

    #log_format  main  '$remote_addr - $remote_user [$time_local] "$request" '
    #                  '$status $body_bytes_sent "$http_referer" '
    #                  '"$http_user_agent" "$http_x_forwarded_for"';

    #access_log  logs/access.log  main;
    sendfile        on;
    #tcp_nopush     on;
    #keepalive_timeout  0;
    keepalive_timeout  65;
    #gzip  on;
}
```

在conf.d下创建一个default.conf的配置文件

```conf
server {
    listen       80;
    server_name  0.0.0.0;

    #charset koi8-r;
    #access_log  /var/log/nginx/host.access.log  main;

    location / {
        root   /usr/local/nginx/html;
        index  index.html index.htm;
    }
    location ^~ /hk/ {
        proxy_pass         http://0.0.0.0:5000/hk/;
        proxy_http_version 1.1;
        proxy_set_header   Upgrade $http_upgrade;
        proxy_set_header   Connection keep-alive;
        proxy_set_header   Host $host;
        proxy_cache_bypass $http_upgrade;
        proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header   X-Forwarded-Proto $scheme;
    }
    #error_page  404              /404.html;

    # redirect server error pages to the static page /50x.html
    #
    error_page   500 502 503 504  /50x.html;
    location = /50x.html {
        root   /usr/local/nginx/html;
    }

    # proxy the PHP scripts to Apache listening on 127.0.0.1:80
    #
    #location ~ \.php$ {
    #    proxy_pass   http://127.0.0.1;
    #}

    # pass the PHP scripts to FastCGI server listening on 127.0.0.1:9000
    #
    #location ~ \.php$ {
    #    root           html;
    #    fastcgi_pass   127.0.0.1:9000;
    #    fastcgi_index  index.php;
    #    fastcgi_param  SCRIPT_FILENAME  /scripts$fastcgi_script_name;
    #    include        fastcgi_params;
    #}

    # deny access to .htaccess files, if Apache's document root
    # concurs with nginx's one
    #
    #location ~ /\.ht {
    #    deny  all;
    #}
}

```

这里配置的动态资源，静态资源在`/usr/local/nginx/html`下，动态资源`http://www.xxx.com/hk`跳转到动态资源地址。

在配置location时，要注意location的优先级，否则容易出错。

如果要配置其他端口或者使用域名(server_name)的时候,可以再另外写一个conf文件，nginx会两个都配置进去。

最后重启服务

```
systemctl restart nginx
```

>使用stream模块

```conf

user  root;
worker_processes  1;

#error_log  logs/error.log;
#error_log  logs/error.log  notice;
#error_log  logs/error.log  info;

pid        /var/run/nginx.pid;


events {
    worker_connections  1024;
}


http {
    include       mime.types;
    include /usr/local/nginx/conf/conf.d/*.conf;
    default_type  application/octet-stream;
    sendfile        on;
    keepalive_timeout  65;
}
stream {
    upstream ssh {
    	server 0.0.0.0:22;
    }
    server {
      listen 8500;
      proxy_pass ssh;
      proxy_connect_timeout 1h;
      proxy_timeout 1h;  
    }
}

```
这里监听8500端口，然后代理到22端口，也就是可以通过8500端口ssh连接。

stream配置也可以单独用一个文件夹来存放，类似上面http配置一样。通过include来导入。

>allow白名单设置

```conf
server {
  listen 80;
  # 允许单个ip
  allow 192.168.68.210;
  # 192.168.68.160/28 代表网段192.168.68.160-192.168.68.175
  allow 192.168.68.160/28;
  # 阻止单个ip，多个也可以如上使用网段。
  deny 192.168.68.121;  
}
```

>nginx 新增https代理模块

nginx代理https需要用到` --with-http_ssl_module`模块，这个并不是nginx的基本模块，所以需要先查看一下是否有配置该模块。

```sh
[root@jnjt-mul002 ~]# /usr/local/nginx/sbin/nginx -V
nginx version: nginx/1.18.0
built by gcc 4.8.5 20150623 (Red Hat 4.8.5-39) (GCC) 
built with OpenSSL 1.0.2k-fips  26 Jan 2017
TLS SNI support enabled
configure arguments: --with-stream
```

看`configure arguments`里并没有该` --with-http_ssl_module`模块，需要重新编译配置

```sh
# 查看模块
./configure --help | grep ssl
```

到源码路径中
```sh
cd /usr/local/package/nginx
./configure --with-stream --with-http_ssl_module
make
```
不用make install

备份原来的nginx,然后用/objs下的nginx替换原来的nginx
```sh
mv /usr/local/nginx/sbin/nginx /usr/local/nginx/sbin/nginx.bak
cp /usr/local/package/nginx/objs/nginx /usr/local/nginx/sbin/nginx
# 重启nginx服务
systemctl restart nginx
```

>nginx 配置https

下载解压证书

配置nginx，conf.d下创建一个新配置文件https.conf

```conf
server {
    listen       443 ssl;
    server_name  hp.ttbems.com;
    # server_name localhost;
    ssl_certificate           /etc/ssl/hk.ttbems.com/Nginx/1_hk.ttbems.com_bundle.crt;
    # ssl_certificate	      /etc/ssl/hk.ttbems.com/Nginx/full_chain_rsa.crt;
    ssl_certificate_key       /etc/ssl/hk.ttbems.com/Nginx/2_hk.ttbems.com.key;
    ssl_prefer_server_ciphers on;
    ssl_protocols             TLSv1 TLSv1.1 TLSv1.2;
    ssl_ciphers               ECDHE-RSA-AES128-GCM-SHA256:HIGH:!aNULL:!MD5:!RC4:!DHE; 
    ssl_dhparam               /usr/local/nginx/ssl/dhparam.pem;
    ssl_ecdh_curve            secp384r1;
    ssl_session_cache         shared:SSL:10m;
    ssl_session_tickets       off;
    ssl_stapling              on; #ensure your cert is capable
    ssl_stapling_verify       on; #ensure your cert is capable
    
    add_header Strict-Transport-Security "max-age=63072000; includeSubdomains; preload";
    add_header X-Frame-Options DENY;
    add_header X-Content-Type-Options nosniff;
    
    #charset koi8-r;
    #access_log  /var/log/nginx/host.access.log  main;
    location =/ {
    	return 301 https://$host/hkview;
    }
    location / {
        root   /usr/local/nginx/html;
        index  index.html index.htm;
    }
    location ^~ /hk/ {
        proxy_pass         http://0.0.0.0:5000/hk/;
        proxy_http_version 1.1;
        proxy_set_header   Upgrade $http_upgrade;
        proxy_set_header   Connection keep-alive;
        proxy_set_header   Host $host;
        proxy_cache_bypass $http_upgrade;
        proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header   X-Forwarded-Proto $scheme;
    }
    # 代理到rabbitmq
    location ^~ /rabbitmq/ {
        proxy_pass         http://0.0.0.0:15672/;
        proxy_http_version 1.1;
        proxy_set_header   Upgrade $http_upgrade;
        proxy_set_header   Connection keep-alive;
        proxy_set_header   Host $host;
        proxy_cache_bypass $http_upgrade;
        proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header   X-Forwarded-Proto $scheme;
    }
    
    #error_page  404              /404.html;

    # redirect server error pages to the static page /50x.html
    #
    error_page   500 502 503 504  /50x.html;
    location = /50x.html {
        root   /usr/local/nginx/html;
    }

    # proxy the PHP scripts to Apache listening on 127.0.0.1:80
    #
    #location ~ \.php$ {
    #    proxy_pass   http://127.0.0.1;
    #}

    # pass the PHP scripts to FastCGI server listening on 127.0.0.1:9000
    #
    #location ~ \.php$ {
    #    root           html;
    #    fastcgi_pass   127.0.0.1:9000;
    #    fastcgi_index  index.php;
    #    fastcgi_param  SCRIPT_FILENAME  /scripts$fastcgi_script_name;
    #    include        fastcgi_params;
    #}

    # deny access to .htaccess files, if Apache's document root
    # concurs with nginx's one
    #
    #location ~ /\.ht {
    #    deny  all;
    #}
}

```

>http跳转到https

修改default.conf配置文件
```conf
server {
    listen 80;
    # 填写绑定证书的域名
    server_name hk.ttbems.com; 
    # 把http的域名请求转成https
    return 301 https://$host$request_uri; 
}

```

>配置http2 

需要安装`ngx_http_v2_module`

配置

```conf
listen 443 ssl http2;
```

>开启gzip压缩

修改nginx.conf配置
```conf
http {
    include       mime.types;
    include /usr/local/nginx/conf/conf.d/*.conf;
    default_type  application/octet-stream;

    sendfile        on;
    #tcp_nopush     on;

    #keepalive_timeout  0;
    keepalive_timeout  65;

    # gzip  on;
    
    
    gzip  on;
    gzip_min_length 1k;
    gzip_buffers 4 16k;
    gzip_http_version 1.0;
    gzip_comp_level 2;
    gzip_types text/plain application/javascript application/css  text/css application/xml text/javascript application/x-httpd-php image/jpeg image/gif image/png image/svg+xml;
    gzip_vary off;
    gzip_disable "MSIE [1-6]\.";


}


```

>配置allow

```conf
server {
    listen 8522;
    allow 180.158.86.158;
    # 电信网段是211.95.166.160-211.95.166.169
    # 211.95.166.160/28表示的网段是211.95.166.160-211.95.166.175
    # 28表示前面28位为子网掩码，后面四位为子网
    # 需要添加deny all；否则还是所有的ip都能进行访问
    # 添加deny all相当于紧张所有ip访问，除了allow以外;
    allow 211.95.116.161;
    deny all;
    proxy_pass ssh;
    proxy_connect_timeout 1h;
    proxy_timeout 1h;
}

```

配置allow时需要添加`deny all;`，否则allow起作用，但是也并不会禁止其他ip访问。

>关于文件上传

使用nginx做代理的时候，可能会出现文件上传大小限制，可以做如下配置来处理

```nginx
client_max_body_size 500m;
client_body_buffer_size 256k;
client_body_temp_path /etc/nginx/proxy_temp;
```

可以配置在`http`、`server`、`location`这几个节点，区别就是作用的范围。

> 关于module

在configure配置的时候可以配置为dynamic

./configure --with-http_image_filter_module=dynamic

这时候进行make install编译安装，会把模块单独生成为ngx_http_image_filter_module.so文件，放在modules文件夹下

然后在nginx.conf 中可以通过load_module "xxx/ngx_http_image_filter_module.so";
进行模块的加载

> 路径配置

```s
location /swagger/ {
    proxy_pass http://47.104.238.228:8443;
}
```
跳转到`http://47.104.238.228:8443/swagger`

```s
location /swagger {
    proxy_pass http://47.104.238.228:8443/swagger;
}
```
跳转到`http://47.104.238.228:8443/swagger`

```s
location /swagger {
    proxy_pass http://47.104.238.228:8443/api;
}
```
跳转到`http://47.104.238.228:8443/api`

