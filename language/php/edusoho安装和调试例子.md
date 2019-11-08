# 版本问题
使用wamp(window 、Apache、mysql、php)安装，存在很多版本问题  
1、php版本  
php7.2之后的版本优化问题，对参数进行比较严格的限制，而edusoho中的参数传递不是很严谨，所以在7.2及以上的版本中运行会报错  
2、mysql版本    
mysql有groups表，在mysql8.0版本中，关于groups表的查询会报错，这个可能是表名和关键字冲突了。所以要使用5.7版本。    
3、Apache版本   
Apache有两种类型的版本，Apache Lounge和Apache Haus，在windows上推荐用Apache Lounge    
版本号上并没发现大问题。    
4、mod_fcgid版本      
mod_fcgid版本最好和Apache相差不大。

目前测试可用版本：

* `php-7.1.32-nts-Win32-VC14-x64` 
* `mysql5.7`
* `httpd-2.4.41-win64-VS16-Apache Lounge`
* `mod_fcgid-2.3.10-win64-VS16`


# php
>下载解压

解压到随便一个路径

>环境变量

php的解压路径，如`D:\php`，添加到path中。


> 配置

php.ini-development拷贝一份，更改名称为`php.ini`, php启动将默认使用该配置文件，添加如下配置(把ini文件的`;`号去掉就行)

```ini
; On windows:
extension_dir = "ext"
;extension=php_bz2.dll
extension=php_curl.dll
;extension=php_fileinfo.dll
;extension=php_ftp.dll
extension=php_gd2.dll
;extension=php_gettext.dll
;extension=php_gmp.dll
extension=php_intl.dll
;extension=php_imap.dll
;extension=php_interbase.dll
;extension=php_ldap.dll
extension=php_mbstring.dll
;extension=php_exif.dll      ; Must be after mbstring as it depends on it
extension=php_mysqli.dll
;extension=php_oci8_12c.dll  ; Use with Oracle Database 12c Instant Client
;extension=php_odbc.dll
;extension=php_openssl.dll
;extension=php_pdo_firebird.dll
extension=php_pdo_mysql.dll
;extension=php_pdo_oci.dll
extension=php_pdo_odbc.dll
;extension=php_pdo_pgsql.dll
;extension=php_pdo_sqlite.dll
;extension=php_pgsql.dll
;extension=php_shmop.dll

; The MIBS data available in the PHP distribution must be installed.
; See http://www.php.net/manual/en/snmp.installation.php
;extension=php_snmp.dll

;extension=php_soap.dll
extension=php_sockets.dll
;extension=php_sqlite3.dll
;extension=php_tidy.dll
extension=php_xmlrpc.dll
extension=php_xsl.dll

upload_max_filesize = 1024M
post_max_size = 1024M
log_errors_max_len = 1024
memory_limit = 1024M

```
`extension`是要引用到的包，`extension_dir = "ext"`是包路径，默认是在当前路径的`ext`文件夹下，所以在windows系统中直接配置`"ext"`就行了，linux下不能这么配置。

# mysql

查看[centos中安装mysql](../../database/mysql/centos中安装mysql.md)

# Apache
>[下载解压](https://www.apachelounge.com/download/)

下载解压，放在C盘目录下(默认配置是在c盘目录下，不在c盘目录下的话需要修改配置)
如`C:\Apache24`

目录可以查看httpd.conf文件

```sh
Define SRVROOT "c:/Apache24"
```

>mod_fcgid

下载对应版本的`mod_fcgid`后，把`mod_fcgid.so`文件放在Apache目录下的`modules`文件夹下。

[下载地址](https://www.apachelounge.com/download/)

>模块配置

```sh
#
# Dynamic Shared Object (DSO) Support
#
# To be able to use the functionality of a module which was built as a DSO you
# have to place corresponding `LoadModule' lines at this location so the
# directives contained in it are actually available _before_ they are used.
# Statically compiled modules (those listed by `httpd -l') do not need
# to be loaded here.
#
# Example:
# LoadModule foo_module modules/mod_foo.so
#
LoadModule access_compat_module modules/mod_access_compat.so
LoadModule actions_module modules/mod_actions.so
LoadModule alias_module modules/mod_alias.so
LoadModule allowmethods_module modules/mod_allowmethods.so
LoadModule asis_module modules/mod_asis.so
LoadModule auth_basic_module modules/mod_auth_basic.so
#LoadModule auth_digest_module modules/mod_auth_digest.so
#LoadModule auth_form_module modules/mod_auth_form.so
#LoadModule authn_anon_module modules/mod_authn_anon.so
LoadModule authn_core_module modules/mod_authn_core.so
#LoadModule authn_dbd_module modules/mod_authn_dbd.so
#LoadModule authn_dbm_module modules/mod_authn_dbm.so
LoadModule authn_file_module modules/mod_authn_file.so
#LoadModule authn_socache_module modules/mod_authn_socache.so
#LoadModule authnz_fcgi_module modules/mod_authnz_fcgi.so
#LoadModule authnz_ldap_module modules/mod_authnz_ldap.so
LoadModule authz_core_module modules/mod_authz_core.so
#LoadModule authz_dbd_module modules/mod_authz_dbd.so
#LoadModule authz_dbm_module modules/mod_authz_dbm.so
LoadModule authz_groupfile_module modules/mod_authz_groupfile.so
LoadModule authz_host_module modules/mod_authz_host.so
#LoadModule authz_owner_module modules/mod_authz_owner.so
LoadModule authz_user_module modules/mod_authz_user.so
LoadModule autoindex_module modules/mod_autoindex.so
#LoadModule brotli_module modules/mod_brotli.so
#LoadModule buffer_module modules/mod_buffer.so
#LoadModule cache_module modules/mod_cache.so
#LoadModule cache_disk_module modules/mod_cache_disk.so
#LoadModule cache_socache_module modules/mod_cache_socache.so
#LoadModule cern_meta_module modules/mod_cern_meta.so
LoadModule cgi_module modules/mod_cgi.so
#LoadModule charset_lite_module modules/mod_charset_lite.so
#LoadModule data_module modules/mod_data.so
#LoadModule dav_module modules/mod_dav.so
#LoadModule dav_fs_module modules/mod_dav_fs.so
#LoadModule dav_lock_module modules/mod_dav_lock.so
#LoadModule dbd_module modules/mod_dbd.so
#LoadModule deflate_module modules/mod_deflate.so
LoadModule dir_module modules/mod_dir.so
#LoadModule dumpio_module modules/mod_dumpio.so
LoadModule env_module modules/mod_env.so
#LoadModule expires_module modules/mod_expires.so
#LoadModule ext_filter_module modules/mod_ext_filter.so
#LoadModule file_cache_module modules/mod_file_cache.so
#LoadModule filter_module modules/mod_filter.so
#LoadModule http2_module modules/mod_http2.so
#LoadModule headers_module modules/mod_headers.so
#LoadModule heartbeat_module modules/mod_heartbeat.so
#LoadModule heartmonitor_module modules/mod_heartmonitor.so
#LoadModule ident_module modules/mod_ident.so
#LoadModule imagemap_module modules/mod_imagemap.so
LoadModule include_module modules/mod_include.so
#LoadModule info_module modules/mod_info.so
LoadModule isapi_module modules/mod_isapi.so
#LoadModule lbmethod_bybusyness_module modules/mod_lbmethod_bybusyness.so
#LoadModule lbmethod_byrequests_module modules/mod_lbmethod_byrequests.so
#LoadModule lbmethod_bytraffic_module modules/mod_lbmethod_bytraffic.so
#LoadModule lbmethod_heartbeat_module modules/mod_lbmethod_heartbeat.so
#LoadModule ldap_module modules/mod_ldap.so
#LoadModule logio_module modules/mod_logio.so
LoadModule log_config_module modules/mod_log_config.so
#LoadModule log_debug_module modules/mod_log_debug.so
#LoadModule log_forensic_module modules/mod_log_forensic.so
#LoadModule lua_module modules/mod_lua.so
#LoadModule macro_module modules/mod_macro.so
#LoadModule md_module modules/mod_md.so
LoadModule mime_module modules/mod_mime.so
#LoadModule mime_magic_module modules/mod_mime_magic.so
LoadModule negotiation_module modules/mod_negotiation.so
LoadModule proxy_module modules/mod_proxy.so
#LoadModule proxy_ajp_module modules/mod_proxy_ajp.so
#LoadModule proxy_balancer_module modules/mod_proxy_balancer.so
#LoadModule proxy_connect_module modules/mod_proxy_connect.so
#LoadModule proxy_express_module modules/mod_proxy_express.so
LoadModule proxy_fcgi_module modules/mod_proxy_fcgi.so
#LoadModule proxy_ftp_module modules/mod_proxy_ftp.so
#LoadModule proxy_hcheck_module modules/mod_proxy_hcheck.so
#LoadModule proxy_html_module modules/mod_proxy_html.so
LoadModule proxy_http_module modules/mod_proxy_http.so
#LoadModule proxy_http2_module modules/mod_proxy_http2.so
#LoadModule proxy_scgi_module modules/mod_proxy_scgi.so
#LoadModule proxy_uwsgi_module modules/mod_proxy_uwsgi.so
#LoadModule proxy_wstunnel_module modules/mod_proxy_wstunnel.so
#LoadModule ratelimit_module modules/mod_ratelimit.so
#LoadModule reflector_module modules/mod_reflector.so
#LoadModule remoteip_module modules/mod_remoteip.so
#LoadModule request_module modules/mod_request.so
#LoadModule reqtimeout_module modules/mod_reqtimeout.so
LoadModule rewrite_module modules/mod_rewrite.so
#LoadModule sed_module modules/mod_sed.so
#LoadModule session_module modules/mod_session.so
#LoadModule session_cookie_module modules/mod_session_cookie.so
#LoadModule session_crypto_module modules/mod_session_crypto.so
#LoadModule session_dbd_module modules/mod_session_dbd.so
LoadModule setenvif_module modules/mod_setenvif.so
#LoadModule slotmem_plain_module modules/mod_slotmem_plain.so
#LoadModule slotmem_shm_module modules/mod_slotmem_shm.so
#LoadModule socache_dbm_module modules/mod_socache_dbm.so
#LoadModule socache_memcache_module modules/mod_socache_memcache.so
#LoadModule socache_redis_module modules/mod_socache_redis.so
#LoadModule socache_shmcb_module modules/mod_socache_shmcb.so
#LoadModule speling_module modules/mod_speling.so
#LoadModule ssl_module modules/mod_ssl.so
#LoadModule status_module modules/mod_status.so
#LoadModule substitute_module modules/mod_substitute.so
#LoadModule unique_id_module modules/mod_unique_id.so
#LoadModule userdir_module modules/mod_userdir.so
#LoadModule usertrack_module modules/mod_usertrack.so
#LoadModule version_module modules/mod_version.so
LoadModule vhost_alias_module modules/mod_vhost_alias.so
#LoadModule watchdog_module modules/mod_watchdog.so
#LoadModule xml2enc_module modules/mod_xml2enc.so
```

这些模块具体功能尚未知道，目前只是记录能用的配置，其中`LoadModule rewrite_module modules/mod_rewrite.so`比较重要，这个是处理url重写的，在edusoho中有很多重写规则，所以需要用到这个模块。

>配置fastcgi

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

>虚拟主机配置

httpd.conf中添加如下配置
```sh
Include conf/extra/httpd-vhosts.conf
#有几个虚拟主机就要配置几个Listen，还有一个Listen是Apache默认目录的Listen
Listen 8082
```
在`conf/extra/httpd-vhosts.conf`文件中进行虚拟主机的配置

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
2.4版本必须加上`Require all granted`,否则会出现权限异常`Forbidden You don't have permission to access this resource.`


# 调试

调试使用`xdebug`来进行调试。xdebug调试原理：

Apache ——(fastcgi)——> php ——(xdebug)——> vscode

php开发的流程大概就是：

* http request 到达Apache

* Apache通过fastcgi和php通讯，解析php

* php通过xdebug来访问vscode进行调试。

vscode中使用php-debug插件来调试php，该插件会监听9000端口，然后php在运行时会通过xdebug向vscode监听的9000端口发送消息来进行通讯调试。所以Apache、php和vscode都是独立的模块，通过不同协议进行互相通讯、调试。

## xdebug配置
1、这个需要先下载对应的php版本的xdebug

[下载链接](https://xdebug.org/download.php)
下载后发在php的`ext`文件夹目录下。

版本的话需要对照下面两个参数。
* Architecture：x64
* Zend Extension Build: API320160303,NTS,VC14

所以下载了`PHP 7.1 VC14 (64 bit)`版本，线程安全版是`PHP 7.1 VC14 TS (64 bit)`;

这些信息可以通过`phpinfo()`获取到。


2、在php中配置xdebug

在php.ini末尾添加下列配置信息。
```ini
[XDebug]
; XDEBUG Extension
zend_extension=php_xdebug-2.7.2-7.1-vc14-nts-x86_64.dll
xdebug.remote_enable = on
xdebug.profiler_enable = on
xdebug.profiler_enable_trigger = off
xdebug.profiler_output_name = cachegrind.out.%t.%p
xdebug.profiler_output_dir = "D:/WWW/tmp"
xdebug.show_local_vars=0
;启用远程调试
xdebug.remote_autostart= 1
```


# 异常

>500 

edusoho 使用的是symfony框架，出现500异常的时候，可以查看项目目录的`app\logs`文件夹下查找异常信息。

这里出现的500异常是因为数据库版本问题，导致sql执行错误。

>跳转异常

URL rewrite 问题，配置URL rewrite 。

>