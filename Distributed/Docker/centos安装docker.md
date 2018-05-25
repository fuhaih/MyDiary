## 安装docker
```vim shell
[root@izm5e944c3bh8eikqxjle5z ~]# yum -y install docker-io
```

## 启动docker
```vim shell
[root@izm5e944c3bh8eikqxjle5z ~]# service docker start
```

## 关闭docker
```vim shell
[root@izm5e944c3bh8eikqxjle5z ~]# service docker stop
```

## docker开机启动
```vim shell
[root@izm5e944c3bh8eikqxjle5z ~]# systemctl enable docker.service
```

## 运行测试用例
```vim shell
[root@izm5e944c3bh8eikqxjle5z ~]# docker run hello-world
```
如果测试用例镜像不存在，docker会从远程仓库中下载镜像来运行

## 镜像加速
打开 /etc/docker/daemon.json，添加配置
```vim shell
{
  "registry-mirrors": ["http://hub-mirror.c.163.com"]
}
```

## 查看镜像运行情况
```vim shell
[root@izm5e944c3bh8eikqxjle5z ~]# docker images hello-world
```

## 查看容器
```vim shell
[root@izm5e944c3bh8eikqxjle5z ~]# docker ps -a
```

## 停止容器
```vim shell
[root@izm5e944c3bh8eikqxjle5z ~]# docker stop <CONTAINER ID>
```
停止容器并不是说容器已经删除，也不需要重新执行run来创建容器，可以通过start启动已经停止的容器
## 启动容器
```vim shell
[root@izm5e944c3bh8eikqxjle5z ~]# docker start <CONTAINER ID>
```
## 查看容器日志
```vim shell
[root@izm5e944c3bh8eikqxjle5z ~]# docker logs -f <CONTAINER ID>
```

## 删除所有容器
```vim shell
[root@izm5e944c3bh8eikqxjle5z ~]# docker rm $(docker ps -a -q)
```
## 列出所有镜像
```vim shell
[root@izm5e944c3bh8eikqxjle5z ~]# docker image list
```
## 删除镜像
```vim shell
[root@izm5e944c3bh8eikqxjle5z ~]# docker rmi <image id/name>
```