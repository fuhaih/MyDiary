# docker 安装

>拉取镜像

```
docker pull gitlab/gitlab-ce
```


>docker运行

```sh
# 创建gitlab数据存储路径
mkdir /usr/local/gitlab
docker run -d -p 443:443 -p 80:80 -p 222:22 --restart=always --hostname 0.0.0.0 -v /usr/local/gitlab/config:/etc/gitlab -v /usr/local/gitlab/logs:/var/log/gitlab -v /usr/local/gitlab/data:/var/opt/gitlab gitlab/gitlab-ce
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

>

配置

```
gitlab_rails['gitlab_ssh_host'] = '192.168.199.231'
gitlab_rails['gitlab_shell_ssh_port'] = 222 # 此端口是run时22端口映射的222端口
```

# git 多账户使用

>清除全局配置

```
git config --global --unset user.name "你的名字"
git config --global --unset user.email "你的邮箱"
```

使用本地配置来进行配置

```
git config --local user.name "你的名字"
git config --local user.email "你的邮箱"
```


>生成秘钥

```
ssh-keygen -t rsa -f ~/.ssh/id_rsa.github -C "lx@qq.com"
ssh-keygen -t rsa -f ~/.ssh/id_rsa.gitlab -C "lx@qq.com"
```

把秘钥添加到ssh agent中,这个是必须的，否则可能出现异常

```
$ ssh-agent bash
$ ssh-add ~/.ssh/id_rsa.github
$ ssh-add ~/.ssh/id_rsa.gitlab
```


拷贝秘钥
```
clip < ~/.ssh/id_rsa.github.pub
```
然后登陆账户添加到github上，gitlab同理

>创建config文件（重点）

```
touch ~/.ssh/config    
```

写入如下配置

```yml
#Default gitHub user Self
Host github.com
    HostName github.com
    PreferredAuthentications publickey
    User fuhaih
    IdentityFile ~/.ssh/id_rsa.github

#Add gitLab user 
Host 192.168.68.100
    Port 222
    HostName 192.168.68.100
    User root
    PreferredAuthentications publickey
    IdentityFile ~/.ssh/id_rsa.gitlab
```

>测试秘钥

```sh
ssh -T git@github.com
ssh -T git@192.168.68.100
```

