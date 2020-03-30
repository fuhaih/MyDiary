## 安装

[官方教程](https://www.postgresql.org/download/linux/redhat/)

>添加yum源

```sh
yum install https://download.postgresql.org/pub/repos/yum/reporpms/EL-7-x86_64/pgdg-redhat-repo-latest.noarch.rpm
```

>yum安装client

```sh
yum install postgresql12
```

>yum安装server
```sh
yum install postgresql12-server
```

>初始化并启动数据库

```sh
# 初始化数据库
/usr/pgsql-12/bin/postgresql-12-setup initdb
# 设置开机启动
systemctl enable postgresql-12
# 启动服务
systemctl start postgresql-12
```

>进入数据库

```sh
# 切换到postgres用户
[root@iZwz9bgky7baa1u8pb4fniZ fuhai]# su - postgres
# psql命令进入postgresql数据库
-bash-4.2$ psql
# 列出表格
postgres=# \l
# 退出psql
postgres=# \q;
# 退出 postgres
-bash-4.2$ exit
```

>创建用户

```sh
postgres=# create user test with password '123456'
```

>开启远程访问

```sh
vim /var/lib/pgsql/12/data/postgresql.conf
```

修改
```vim
listen_addresses = "*"
```

重启服务

```sh
systemctl restart postgresql-12
```
