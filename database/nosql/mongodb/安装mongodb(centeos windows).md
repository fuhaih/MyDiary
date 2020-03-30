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


 # windows安装mongodb

## 1、下载对应版本安装包进行安装

选择全部安装，默认安装路径会在`C:\Program Files\MongoDB`下


## 2、在其他盘添加一个配置文件mongod.cfg
如`D:\MongoDB\mongod.cfg`
```
systemLog:
    destination: file
    path: D:\MongoDB\logs\mongoLog.log
storage:
    dbPath: D:\MongoDB\Data
net:
    port: 27017
    bindIp: 0.0.0.0
```
配置项中  
`D:\MongoDB\logs\mongoLog.log`是日志路径  
`D:\MongoDB\Data`是数据路径

## 3、根据配置文件来启动mongodb
需要使用`mongod`命令，该命令是在安装目录下，为了方便使用可以把命令所在地址添加到环境变量中。  
`C:\Program Files\MongoDB\Server\3.6\bin`


```
mongod -f D:\MongoDB\mongod.cfg
```

查看D:\MongoDB\logs\mongoLog.log会有日志记录mongodb的启动信息   
通过`http://localhost:27017/`查看mongodb是否启动成功    

## 4、把mongodb以windows服务的形式来启动
这个需要使用管理员权限，使用管理员权限来打开命令行工具    
```
mongod -f D:\MongoDB\mongod.cfg --install --serviceName "MongoDB" --serviceDisplayName "MongoDB"
```

然后通过`net start MongoDB`命令来启动服务，也可以直接到服务管理面板来手动启动服务

卸载服务和重装服务
```sh
# mongod命令
mongod -f D:\MongoDB\mongod.cfg --remove --serviceName "MongoDB"
mongod -f D:\MongoDB\mongod.cfg --reinstall --serviceName "MongoDB"

# windows sc 命令
# sc delete <serviceDisplayName>
sc delete MongoDB
```
## 进入控制台

```sh
mongo 127.0.0.1:27017

```

默认端口号可以直接`mongo`命令，当修改过端口号时需要手写ip和端口号。
