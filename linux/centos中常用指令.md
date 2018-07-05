# 磁盘
```vim shell
# 查看磁盘大小
[root@izm5e944c3bh8eikqxjle5z ~]# df -h
# 查看内存大小
[root@izm5e944c3bh8eikqxjle5z ~]# free
```
# 权限问题
## 开机自启动
```vim shell
# 设置/取消开机自启
[root@izm5e944c3bh8eikqxjle5z ~]# systemctl enable httpd.service
[root@izm5e944c3bh8eikqxjle5z ~]# systemctl disable httpd.service
[root@izm5e944c3bh8eikqxjle5z ~]# chkconfig httpd.service on
[root@izm5e944c3bh8eikqxjle5z ~]# chkconfig httpd.service off
```
