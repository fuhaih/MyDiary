# [在centeos中安装mongodb数据库](https://docs.mongodb.com/master/tutorial/install-mongodb-on-red-hat/)
## 1、配置包管理工具（yum）
创建仓库文件，以便可以直接用yum工具来安装MongoDB数据库
```vim shell
vim  /etc/yum.repos.d/mongodb-org-3.6.repo
```
编辑以下内容到仓库文件
```vim shell
[mongodb-org-3.6]
name=MongoDB Repository
baseurl=https://repo.mongodb.org/yum/redhat/$releasever/mongodb-org/3.6/x86_64/
gpgcheck=1
enabled=1
gpgkey=https://www.mongodb.org/static/pgp/server-3.6.asc
```

## 2、安装MongoDB
```linux command
sudo yum install -y mongodb-org
```

## 3、开启/禁用SELinux 
/etc/selinux/config
```vim shell
# 禁用
SELINUX=disabled
# 启用
SELINUX=permissive
```

## 4、更改ip
 vim 打开/etc/mongod.conf
 把127.0.0.1更改为0.0.0.0，否则mongodb只监听本地的端口，mongodb将无法远程连接

 ## 5、开启mongodb服务
 ```vim shell
 # 开启服务
 sudo service mongod start
 # 查看服务是否开启
 sudo chkconfig mongod on
 # 关闭服务
 sudo service mongod stop 
 # 重启服务
 sudo service mongod restart 
 ```
 ## 6、进入mongodb
 ```vim shell
 # 进入mongodb
 mongo
 # 退出mongodb
 exit
 ```

 ## 7、卸载mongodb
 ```vim shell
 # 卸载服务
 sudo yum erase $(rpm -qa | grep mongodb-org)
 # 删除数据
 sudo rm -r /var/log/mongodb
sudo rm -r /var/lib/mongo
 ```


