# 用vnc连接linux图形界面
## windows安装vnc

        在window中安装vnc view就行了
## linux安装vnc server
模糊查询vnc的包

        yum search vnc 

安装tigervnc-server

        yum install tigervnc-server 

开启vnc服务，初次开启需要输入两次密码

        vncserver

修改桌面类型和分辨率的地方

        vi /root/.vnc/xstartup 
vnc配置

        vi /etc/sysconfig/vncservers
        增加内容
        VNCSERVERS="1:root"
        VNCSERVERARGS[1]="-geometry 800x600 -alwaysshared -depth 24"
        保存退出
vncserver 重启

        service vncserver restart 重启vncserver服务，目前失败
        /etc/init.d/vncserver restart 重启vncserver 服务 ，目前也失败

        vncserver -list 列出vncserver
        vncserver -kill :1 删除display为1的vncserver

        替换重启的方案：
        vncserver 
        vncserver -list 发现有两个vncserver，然后删除较早生成的那个
        vncserver -kill :1
        （不一定是1，是删除之前创建的vncserver）

修改分辨率和颜色

        vncserver -geometry 800x600
        这个命令实际中很有用，例如本地分辨率为1024x768 如果不设定远程VNC服务分辨率就会造成桌面显示不全的问题。
        vncserver -depth 指定显示颜色,设定范围8～32
        vncserver -depth 16
        用16bits颜色显示
        vncserver -pixelformat 指定色素格式与-depth大致相同，只是表示方法不一样
        vncserver -pixelformat RGB888
        vncserver -geometry 1152x864 -depth 24 即可以1152x8




# linux使用技巧
        gnome desktop:
        使用startx指令可以进入桌面
# 防火墙
centeos中有两个工具可以设置防火墙
## firewalld 
firewalld是centos 7 中默认使用的
## iptables


# vim 使用

        i 进入编辑状态

        esc 推出编辑

        :q 退出不保存

        :wq 保存退出

        :q! 强制退出
## vim非正常退出
vim在编辑的时候会生成一个.filename.swp的文件，如在编辑/etc/mongod.conf文件时，如果异常退出，会生产一个/etc/.mongod.conf.swp的文件，如果时正常退出，会自动删除该问题。
异常退出恢复就是靠该文件：
```vim shell
# 异常退出恢复
vim -r /etc/mongod.conf

# 恢复完成后可以删除swp文件
rm /etc/.mongod.conf.swp
```

## vim停止输出
Ctrl + s快捷键会让vim停止向终端输出，这时候vim就像是卡死一样，只需按Ctrl + q 即可恢复正常。


# linux 配置静态ip
## 方法1
查找/etc/sysconfig/network-scripts目录下的ifcfg-enp0s3文件
````vim shell
ls /etc/sysconfig/network-scripts
````
如果有该文件，就打开进行编辑
```vim shell
vi /etc/sysconfig/network-scripts/ifcfg-enp0s3
```
编辑内容如下
```vim shell
TYPE="Ethernet"
PROXY_METHOD="none"
BROWSER_ONLY="no"
BOOTPROTO="static"
IPADDR=192.168.0.17
NETMASK=255.255.255.0
NM_CONTROLLED="no"
DEFROUTE="yes"
IPV4_FAILURE_FATAL="no"
IPV6INIT="yes"
IPV6_AUTOCONF="yes"
IPV6_DEFROUTE="yes"
IPV6_FAILURE_FATAL="no"
IPV6_ADDR_GEN_MODE="stable-privacy"
NAME="ENP0S3"
UUID="XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX"
DEVICE="enp0s3"
ONBOOT="yes"
```

重启网络
```vim shell
systemctl restart network.service
```


