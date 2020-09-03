
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

## 创建一个新的容器并运行
**语法:** docker run [OPTIONS] IMAGE [COMMAND] [ARG...]   
**常用OPTIONS:**    
> -d 后台运行

> --name [name] 容器名称

> --restart [value] 开机启动 不自动重启容器. (默认value)

  >> on-failure 	容器发生error而退出(容器退出状态不为0)重启容器

  >> unless-stopped 	在容器已经stop掉或Docker stoped/restarted的时候才重启容器

  >> always 	在容器已经stop掉或Docker stoped/restarted的时候才重启容器

> -p [port:port] 端口映射

> -it 交互
```shell
[root@izm5e944c3bh8eikqxjle5z ~]# docker run -d --restart always -p 6606:6606 --name firstweb fuhai/firstweb:1.0
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
```sh
# 列出镜像
[root@izm5e944c3bh8eikqxjle5z ~]# docker image list
[root@izm5e944c3bh8eikqxjle5z ~]# docker images
# 列出镜像ID
[root@izm5e944c3bh8eikqxjle5z ~]# docker images -q
```
## 删除镜像
```sh
[root@izm5e944c3bh8eikqxjle5z ~]# docker rmi <image id/name>
# 删除<none>镜像
[root@izm5e944c3bh8eikqxjle5z ~]# docker rmi $(docker images -f "dangling=true" -q)

# 删除所有镜像
[root@izm5e944c3bh8eikqxjle5z ~]# docker rmi $(docker images -q)
```
同一个镜像可以有多个tag，所以有可能看到有两个镜像拥有相同的镜像id，所以docker rmi `<name>`有可能只是删除一个tag而已，镜像不会真正删除镜像。

# Docker一些目录
> /var/run/docker.sock

docker套接字，docker的镜像和容器操作是通过与这个套接字通讯来实现的，很重要的文件

> /var/lib/docker

docker文件目录，包括镜像、容器、数据卷等都在这里，在删除旧版本的docker的时候，如果旧版docker里的镜像和容器等都不需要，可以删除这个文件夹

# Docker 离线安装

>安装

下载离线安装包

```sh
docker-ce-19.03.9-3.el7.x86_64.rpm
docker-ce-cli-19.03.9-3.el7.x86_64.rpm
container-selinux-2.119.1-1.c57a6f9.el7.noarch.rpm
containerd.io-1.2.6-3.3.el7.x86_64.rpm
```

使用yum进行安装

```sh
yum install *.rpm
```
>导出镜像

```sh

docker save name:tag > image.tar

```

>导入镜像

```sh
docker load < image.tar
```