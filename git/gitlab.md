# docker 安装

>拉取镜像

```
docker pull gitlab/gitlab-ce
```


>docker运行

```sh
# 创建gitlab数据存储路径
mkdir /usr/local/gitlab
docker run -d -p 443:443 -p 80:80 -p 222:22 --hostname 0.0.0.0 -v /usr/local/gitlab/config:/etc/gitlab -v /usr/local/gitlab/logs:/var/log/gitlab -v /usr/local/gitlab/data:/var/opt/gitlab gitlab/gitlab-ce
```
端口映射时候，由于22端口是宿主机远程ssh访问端口，所以改成222.

>修改hostname

使用docker来运行gitlab的时候，gitlab的host会是docker的id，这里需要手动修改为ip或者是0.0.0.0

cd到路径`/usr/local/gitlab/data/gitlab-rails/etc`,找到`gitlab.yml`配置文件，修改host

```yml
  gitlab:
    ## Web server settings (note: host is the FQDN, do not include http://)
    host: 0.0.0.0
    port: 80
    https: false

```
`docker restart gitlab`重启gitlab服务（其实上面配置好像也没必要）

`docker logs -f gitlab`查看gitlab启动进度，稍微会有点慢。

然后浏览器输入地址 http://host_ip:port

>gitlab汉化