# 配置静态地址
> 查找network配置文件

配置文件是在/etc/sysconfig/network-scripts目录下

    $ ls /etc/sysconfig/network-scripts

找到ifcfg前缀的文件，本机上是ifcfg-enp0s3，打开配置文件

    $ ls /etc/sysconfig/network-scripts/ifcfg-enp0s3
> 设置IP、子网掩码和网关

    IPADDR=192.168.0.17
    NETMASK=255.255.252.0
    GATEWAY=192.168.0.1

网关要看路由器，一般路由器是192.168.1.1，我的是192.168.0.1

> 设置开机启动网络   

    ONBOOT=yes
> 设置dns

    $ vi /etc/resolv.conf

添加配置

    nameserver 192.168.1.1
    nameserver 8.8.8.8

> 重启网络

    $ systemctl restart network.service
> 测试网络
    
    $ ping www.baidu.com
> VirtualBox额外配置

把网络连接模式更改为桥接模式