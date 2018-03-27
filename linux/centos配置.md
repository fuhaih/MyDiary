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
        ctrl+alt+f2进行图形化界面和命令行界面切换

# vim 使用

        i 进入编辑状态

        esc 推出编辑

        :q 退出不保存

        :wq 保存退出

        :q! 强制退出
