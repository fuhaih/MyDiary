# 常用命令

>yum install 

>查看是否按照了某个软件

```sh
[root~]# yum list installed | grep filter
```

# 异常
## Error Downloading Packages
    出现这个异常可能是yum的缓存已经满了，清空一下缓存再进行安装就行了

```vim shell
[root@izm5e944c3bh8eikqxjle5z ~]# yum clean all
```