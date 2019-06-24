# 常用命令

>yum install 

>查看是否按照了某个软件

```sh
[root~]# yum list installed | grep filter
```

>yum update

`Repodata is over 2 weeks old. Install yum-cron? Or run: yum makecache fast`
需要升级软件包和系统内核

```bash
[root~]# yum clean up
[root~]# yum update
```
# 异常
## Error Downloading Packages
    出现这个异常可能是yum的缓存已经满了，清空一下缓存再进行安装就行了

```vim shell
[root@izm5e944c3bh8eikqxjle5z ~]# yum clean all
```