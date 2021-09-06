
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
## docker file

```
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY ./published ./
ENV ASPNETCORE_ENVIRONMENT=Production ASPNETCORE_PATHBASE=/hk
RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai /etc/localtime
RUN echo 'Asia/Shanghai' >/etc/timezone
ENTRYPOINT ["dotnet", "HongKouEnergyPlatform.dll"]
```

docker是分层的，dockerfile中每个命令都是一层，这里是直接发布编译好的dll的，也可以直接拷贝源代码，使用dockerfile编译再发布

```
FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 6606/tcp

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY Host/FHCore.MVC/FHCore.MVC.csproj Host/FHCore.MVC/
RUN dotnet restore Host/FHCore.MVC/FHCore.MVC.csproj
COPY . .
RUN dotnet build Host/FHCore.MVC/FHCore.MVC.csproj -c Release -o /app

FROM build AS publish
COPY Host/FHCore.MVC/layui /app/layui
RUN dotnet publish Host/FHCore.MVC/FHCore.MVC.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT dotnet FHCore.MVC.dll
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

> -v 卷映射


容器相当于一个运行着的镜像，当重新docker run之后，就相当于新创建了一个容器，这时候之前容器的数据将会消失，所以一些需要存储的数据应该不要放在容器里。

比如说一些日志文件和一些其他文件等，不能放在容器里，应该通过-v映射到实体机上。

> --network 网络模式 

  >> none
  >> host 宿主机的网络，这个一般要用到宿主机资源时候才这样做，但是安全性没有桥接网络好，因为网络没有隔离。
  >> Bridge 桥接模式，默认的模式，相当于宿主机创建了一个子网络，每个容器一个子网ip。


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
[root@izm5e944c3bh8eikqxjle5z ~]# docker logs -f --tail 500 <CONTAINER ID>
```
-f : 跟踪日志输出

--since :显示某个开始时间的所有日志

-t : 显示时间戳

--tail :仅列出最新N条容器日志

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