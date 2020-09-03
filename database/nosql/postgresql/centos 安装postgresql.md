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
# 切换到postgres用户 这个要在root下切换
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
# 新建用户并给予登录权限
postgres=# create role test password '123456' login;
# 新建超级用户并给予登录权限
postgres=# create role test superuser password '123456' login;

```

>开启远程访问

```sh
vim /var/lib/pgsql/12/data/postgresql.conf
```

修改
```vim
listen_addresses = "*"
```

```
vim /var/lib/pgsql/12/data/pg_hba.conf
```

修改
```vim
host    all     all     0.0.0.0/0    md5
```

ident:本地登录

md5:用户名密码登录

trust:仅验证用户

refuse:

重启服务

```sh
systemctl restart postgresql-12
```
