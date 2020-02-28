## 程序

名称：SHEnergyServer

发布位置：/usr/local/console/SHEnergyServer/publish/

启动程序命令：dotnet SHEnergyServer.dll

## 编写服务文件

```
cd /usr/lib/systemd/system
vim SHEnergyServer.service
```

```
[Unit]
Description=SHEnergyServer2
[Service]
Type=simple
WorkingDirectory=/usr/local/console/SHEnergyServer/publish/
ExecStart=/usr/local/bin/dotnet SHEnergyServer.dll
Restart=always
[Install]
WantedBy=multi-user.target
```

`WorkingDirectory`为程序的发布路径

`ExecStart`的dotnet命令需要写完整路径。

## 启动程序

```sh
# 重载系统服务
systemctl daemon-reload
# 启动程序
systemctl start SHEnergyServer.service
# 查看状态
systemctl status SHEnergyServer.service
# 开机启动
systemctl enable SHEnergyServer.service
```

## 关于程序配置文件路径问题

## 开放端口号

```sh
# 添加端口
firewall-cmd --permanent --add-port=8081/tcp
# 查看开放端口
firewall-cmd --list-all
# 重新加载配置
firewall-cml --reload 
```

