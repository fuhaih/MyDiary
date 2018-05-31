## 下载mysql的repo源的rpm软件包

    $ wget http://repo.mysql.com/mysql-community-release-el7-5.noarch.rpm

## 安装mysql-community-release-el7-5.noarch.rpm包

    sudo rpm -ivh mysql-community-release-el7-5.noarch.rpm

安装这个包后，会获得两个mysql的yum repo源：/etc/yum.repos.d/mysql-community.repo，/etc/yum.repos.d/mysql-community-source.repo。

## 安装mysql

    $ sudo yum install mysql-server
## 启动mysql服务
    $ systemctl start mysqld
    #设置服务开机启动
    $ systemctl enable mysqld.service
## 重置root密码
在安装了mysql后，默认有个root用户,密码为空

    $ mysql -u root
    # 切换数据库
    mysql >use mysql;
    # 密码设置为123456
    mysql > update user set password=password('123456') where user='root';
    # 刷新权限
    mysql > flush privileges;

在mysql命令行输入执行语句时，需要以“;”结尾，否则命令行不认为命令结束，可以一直打命令。
## 远程登录授权
    #允许root用户在任何地方进行远程登录，并具有所有库任何操作权限
    mysql>GRANT ALL PRIVILEGES ON *.* TO 'root'@'%' IDENTIFIED BY 'youpassword' WITH GRANT OPTION;

    #允许root用户在一个特定的IP进行远程登录，并具有所有库任何操作权限
    mysql>GRANT ALL PRIVILEGES ON *.* TO root@"172.16.16.152" IDENTIFIED BY "youpassword" WITH GRANT OPTION;

    #允许root用户在一个特定的IP进行远程登录，并具有所有库特定操作权限
    mysql>GRANT select，insert，update，delete ON *.* TO root@"172.16.16.152" IDENTIFIED BY "youpassword";

    #允许root用户在一个特定的IP进行远程登录，并具有某个库特定操作权限
    mysql>GRANT select，insert，update，delete ON TEST-DB TO root@"172.16.16.152" IDENTIFIED BY "youpassword";

**注意：**
在完成这些操作后需要执行FLUSH PRIVILEGES命令才能生效    
    
    mysql>FLUSH PRIVILEGES

## 删除授权
    #清除了用户test-user对于TEST-DB的相关授权权限
    REVOKE all on TEST-DB from test-user;
    #清除了用户test-user对于所有数据库相关授权权限
    REVOKE all on *.* from test-user;