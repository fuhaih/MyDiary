# 安装erlang
```vim shell
[root@izm5e944c3bh8eikqxjle5z ~]# yum install erlang
```

# 安装rabbitmq-server
## 安装
```vim shell
[root@izm5e944c3bh8eikqxjle5z ~]# curl -s https://packagecloud.io/install/repositories/rabbitmq/rabbitmq-server/script.deb.sh | sudo bash
[root@izm5e944c3bh8eikqxjle5z ~]# yum install rabbitmq-server
```
## 启用插件
```vim shell
[root@izm5e944c3bh8eikqxjle5z ~]# rabbitmq-plugins enable rabbitmq_management 启动
[root@izm5e944c3bh8eikqxjle5z ~]# rabbitmq-plugins disable rabbitmq_management 关闭
```
rabbitmq-server 安装后是默认启动了插件的，如果插件关闭，是无法开启rabbitmq-server的控制台服务的。
## 开启服务
rabbitmq安装的时候默认不是以守护进程的方式启动服务的，可以执行以下命令让mq开机启动
```vim shell
[root@izm5e944c3bh8eikqxjle5z ~]# chkconfig rabbitmq-server on
```
以管理员身份启动服务
```vim shell
# 启动服务
[root@izm5e944c3bh8eikqxjle5z ~]# /sbin/service rabbitmq-server start
# 关闭服务
[root@izm5e944c3bh8eikqxjle5z ~]# /sbin/service rabbitmq-server stop
```
## rabbitmq控制台
### 本地访问
    本地可以直接通过http://localhost:15672来访问rabbitmq控制台，用户名和密码都是guest
### 远程ip访问
**rabbitmq配置**
编辑/etc/rabbitmq/rabbitmq.config 文件，添加如下配置
```vim shell
{rabbit, [{tcp_listeners, [5672]}, {loopback_users, ["admin"]}]}
```
指定用户为admin，目前还没有创建admin用户
**添加用户**
```vim shell
# 创建管理员用户，负责整个MQ的运维
[root@izm5e944c3bh8eikqxjle5z ~]# sudo rabbitmqctl add_user admin passwd_admin 
# 赋予其administrator角色
[root@izm5e944c3bh8eikqxjle5z ~]# sudo rabbitmqctl set_user_tags admin administrator 
# 可以创建RabbitMQ监控用户，负责整个MQ的监控
[root@izm5e944c3bh8eikqxjle5z ~]# sudo rabbitmqctl add_user  user_monitoring  passwd_monitor  
# 赋予其monitoring角色
[root@izm5e944c3bh8eikqxjle5z ~]# sudo rabbitmqctl set_user_tags user_monitoring monitoring  
# 可以创建某个项目的专用用户，只能访问项目自己的virtual hosts
[root@izm5e944c3bh8eikqxjle5z ~]# sudo rabbitmqctl  add_user  user_proj  passwd_proj  
# 赋予其management角色
[root@izm5e944c3bh8eikqxjle5z ~]# sudo rabbitmqctl set_user_tags user_proj management 
```
**设置权限**    

    这个权限主要是对于virtualhost 的访问权限    
    权限设置：rabbitmqctl set_permissions [-p vhostpath] {user} {conf} {write} {read} 
    查看（指定vhost）所有用户的权限信息：rabbitmqctl list_permissions [-p vhostPath]
    查看指定用户的权限信息：rabbitmqctl list_user_permissions {username}    
    清除用户的权限信息： rabbitmqctl clear_permissions [-p vhostPath] {username}    
**访问**    

    http://[rabbitmq所在服务器ip]:15672     
    用户名是刚刚创建的admin

