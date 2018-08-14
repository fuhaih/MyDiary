# 安装
>安装redis
```
#如果没有redis源，先安装fedora的epel仓库
$ yum install epel-release
$ yum install redis
```
>修改配置
```vim
$ vim /etc/redis.conf
```

配置如下信息
```config
port 6379
requirepass 111
# 127.0.0.1会使得redis只能本机访问
bind 0.0.0.0
protected-mode no
```

>启动服务
```vim
$ systemctl start redis.service
$ systemctl enable redis.service
```
