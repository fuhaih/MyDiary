## 构建springmvc-spring-hibernate

vscode 左边面板右键，点击`Create Maven Project`，选择`more`，查找`springmvc-spring-hibernate`

groupid和artifactId被统称为“坐标”是为了保证项目唯一性而提出的，如果你要把你项目弄到maven本地仓库去，你想要找到你的项目就必须根据这两个id去查找。

**groupId** 一般分为多个段，这里我只说两段，第一段为域，第二段为公司名称。域又分为org、com、cn等等许多，其中org为非营利组织，com为商业组织。举个apache公司的tomcat项目例子：这个项目的groupId是org.apache，它的域是org（因为tomcat是非营利项目），公司名称是apache，artigactId是tomcat。

个人项目可以为cn.<name>，cn表示域为中国，<name>是个人姓名缩写

**artifactId** 一般指项目名称

项目构建完成后，一般vscode右下角会有loading图标，从仓库中加载maven项目所需的包

## 遇到的坑

>不能识别get set方法

hibernate使用`Lombok data`注解，所以需要给vscode安装 `Lombok Annotations Support for VS Code`插件

>调试时找不到main函数

spring mvc项目是没有main函数的，需要使用tomcat容器来启动调试。