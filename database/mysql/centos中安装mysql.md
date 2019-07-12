## 下载mysql的repo源的rpm软件包

具体版本连接看[官网](https://dev.mysql.com/downloads/repo/yum/)

    $ wget http://repo.mysql.com/mysql80-community-release-el7-3.noarch.rpm

## 安装mysql80-community-release-el7-3.noarch.rpm包

    sudo rpm -ivh mysql80-community-release-el7-3.noarch.rpm

安装这个包后，会获得两个mysql的yum repo源：/etc/yum.repos.d/mysql-community.repo，/etc/yum.repos.d/mysql-community-source.repo。

## 安装mysql

    $ sudo yum install mysql-community-server
## 启动mysql服务
    $ systemctl start mysqld
    #设置服务开机启动
    $ systemctl enable mysqld.service
## 重置root密码
在安装了mysql后，默认有个root用户,密码为空
```bash
# 无密码
$ mysql -uroot
# 密码为123456
$ mysql -uroot -p123456
# 切换数据库
mysql >use mysql;
# 密码设置为123456
mysql > update user set password=password('123456') where user='root';
# 刷新权限
mysql > flush privileges;
```

## 8.0及之后版本

8.0版本首次进入数据库时可能会有下面异常

    ERROR 1045 (28000): Access denied for user 'root'@'localhost' (using password: YES)

修改配置文件 /etc/my.cnf;

    skip-grant-tables

在8.0以后不能通过update语句来更新密码了
```bash
# 无密码
$ mysql -uroot
# 刷新权限 很重要
mysql > flush privileges;
# 修改root密码，密码必须包含大小写字母和特殊字符
mysql > alter user 'root'@'%' IDENTIFIED BY 'MyNewPass@123';

# root对应的host可能不是%，这个需要查看

# 进入mysql库
mysql > use mysql

# 查看host
mysql > select user,host from user;
+------------------+-----------+
| user             | host      |
+------------------+-----------+
| mysql.infoschema | localhost |
| mysql.session    | localhost |
| mysql.sys        | localhost |
| root             | localhost |
+------------------+-----------+

# 如果root的host是localhost，需要把上面语句的%修改为localhost
mysql > alter user 'root'@'localhost' IDENTIFIED BY 'MyNewPass@123';

```

**修改完成之后，就可以把配置中skip-grant-tables去掉，重启mysql服务，这时候就能正常使用mysql命令来进入mysql命令行模式了**


在mysql命令行输入执行语句时，需要以“;”结尾，否则命令行不认为命令结束，可以一直打命令。


## 远程登录授权(以下在8.0版本会有所差异)
```bash
    #允许root用户在任何地方进行远程登录，并具有所有库任何操作权限
    mysql>GRANT ALL PRIVILEGES ON *.* TO 'root'@'%' IDENTIFIED BY 'youpassword' WITH GRANT OPTION;

    #允许root用户在一个特定的IP进行远程登录，并具有所有库任何操作权限
    mysql>GRANT ALL PRIVILEGES ON *.* TO root@"172.16.16.152" IDENTIFIED BY "youpassword" WITH GRANT OPTION;

    #允许root用户在一个特定的IP进行远程登录，并具有所有库特定操作权限
    mysql>GRANT select，insert，update，delete ON *.* TO root@"172.16.16.152" IDENTIFIED BY "youpassword";

    #允许root用户在一个特定的IP进行远程登录，并具有某个库特定操作权限
    mysql>GRANT select，insert，update，delete ON TEST-DB TO root@"172.16.16.152" IDENTIFIED BY "youpassword";
```
## 远程登录授权mysql8.0版本

```bash
# 注意root账户的host必须为%；localhost是无法远程访问的
mysql>GRANT ALL ON *.* TO 'root'@'%' WITH GRANT OPTION;
# 如果host是localhost，需要更新修改一下
mysql > use mysql
mysql > update user set host = '%' where user = 'root';
```

**注意：**
在完成这些操作后需要执行FLUSH PRIVILEGES命令才能生效    
    
    mysql>FLUSH PRIVILEGES

## 删除授权
    #清除了用户test-user对于TEST-DB的相关授权权限
    REVOKE all on TEST-DB from test-user;
    #清除了用户test-user对于所有数据库相关授权权限
    REVOKE all on *.* from test-user;
