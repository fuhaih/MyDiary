# vscode 远程开发

有时候开发环境在linux上，又想要windows的舒适可视化，那就可以使用vscode进行远程开发，直接在windows中编辑调试编译linux机器上的代码

## 配置ssh

>生成ssh秘钥

首先要配置好ssh连接，可以在远程调试时不必每次都输入用户密码来登录linux

```s
# 生成秘钥 -b 是秘钥长度，-f是秘钥文件，由于有其他的ssh连接的秘钥（gitlab、github），所以直接指定秘钥文件的名称，避免覆盖了其他的ssh秘钥
# 会生成私钥公钥两个文件id_rsa.ub、id_rsa.ub.pub
ssh-keygen -t rsa -b 4096 -f ~/.ssh/id_rsa.ub
# 把秘钥配置到linux机器上，使用ssh-copy-id能直接把秘钥拷贝过去
ssh-copy-id -i ~/.ssh/id_rsa.ub root@192.168.68.xxx
```

>配置conf文件

配置`~/.ssh/conf`文件，没有的话手动创建

添加如下配置
```s
#Add ubuntu
Host ub_server
  Port 22
  HostName 192.168.68.xxx
  User root
  IdentityFile ~/.ssh/id_rsa.ub
```

>测试

```s
ssh ub_server
```

直接使用别名登录，会从conf文件中查找ub_server别名的配置，然后根据配置的`IdentityFile`找到秘钥文件，HostName为登录ip，Port为登录端口

## 配置vscode

>安装插件

插件中搜索Remote Development，安装

>使用

使用cmd+shift+p快捷键打开命令输入窗口，输入`>Remote-ssh` 选择`Remote-SSH:Connect to Host...`，选择刚刚配置的ub_server

或者点击左下角的Remote Development插件图标，选择`Remote-SSH:Connect to Host...`，选择刚刚配置的ub_server