# 安装logstash6
>添加repo
```command
$ vi /etc/yum.repos.d/logstash.repo
```
添加如下内容

```
[logstash-6.x]
name=Elastic repository for 6.x packages
baseurl=https://artifacts.elastic.co/packages/6.x/yum
gpgcheck=1
gpgkey=https://artifacts.elastic.co/GPG-KEY-elasticsearch
enabled=1
autorefresh=1
type=rpm-md
```
>安装
```command
$ sudo yum install logstash
```
