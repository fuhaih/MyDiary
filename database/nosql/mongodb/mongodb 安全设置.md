# 版本号4.2

## auth配置

默认情况下mongo不用用户名密码都能都登录并操作数据，这样会有很大的风险。首先在权限没有开启的时候先创建一个管理员账户

>创建用户

mongo命令进入命令行
```sh
>use admin
>db.createUser({user:"root",pwd:"root",roles:["root"]})
```

>配置auth

打开配置文件 /etc/mongod.conf进入如下配置
```conf
security:
  authorization: enabled
```
>重启mongodb

```sh
# systemctl restart mongod
```
