## 查看是否已安装JDK
```s
[root@izm5e944c3bh8eikqxjle5z ~]# yum list installed |grep java
 
abrt-java-connector.x86_64             1.0.6-12.el7                    @base    
java-1.7.0-openjdk.x86_64              1:1.7.0.171-2.6.13.0.el7_4      @updates 
java-1.7.0-openjdk-headless.x86_64     1:1.7.0.171-2.6.13.0.el7_4      @updates 
java-1.8.0-openjdk.x86_64              1:1.8.0.161-0.b14.el7_4         @updates 
java-1.8.0-openjdk-devel.x86_64        1:1.8.0.161-0.b14.el7_4         @updates 
java-1.8.0-openjdk-headless.x86_64     1:1.8.0.161-0.b14.el7_4         @updates 
javapackages-tools.noarch              3.4.1-11.el7                    @base    
python-javapackages.noarch             3.4.1-11.el7                    @base    
tzdata-java.noarch                     2018c-1.el7                     @updates 
```
在输出列表中有两条
```s
java-1.8.0-openjdk.x86_64              1:1.8.0.161-0.b14.el7_4         @updates 
java-1.8.0-openjdk-devel.x86_64        1:1.8.0.161-0.b14.el7_4         @updates 
```
这两条分别是java的运行环境和开发环境，安装开发环境会带有运行环境

## 卸载CentOS系统Java环境
```s
[root@izm5e944c3bh8eikqxjle5z ~]# yum -y remove java-1.8.0-openjdk*        *表时卸载所有openjdk相关文件输入  
[root@izm5e944c3bh8eikqxjle5z ~]# yum -y remove tzdata-java.noarch         卸载tzdata-java  
```

## 安装java运行换jre
```s
yum install java-1.8.0-openjdk
```
## 安装java开发环境
```s
yum install java-1.8.0-openjdk-devel
# 或者是
yum install java-1.8.0-openjdk*
```

## 配置环境变量
通过yum默认安装的路径为 /usr/lib/jvm 
查找该路径下的文件或者文件夹就能看到安装的jre
```s
[root@izm5e944c3bh8eikqxjle5z ~]# ls -l /usr/lib/jvm
总用量 8
lrwxrwxrwx 1 root root   26 4月  19 16:37 java -> /etc/alternatives/java_sdk
drwxr-xr-x 4 root root 4096 3月  23 14:34 java-1.7.0-openjdk-1.7.0.171-2.6.13.0.el7_4.x86_64
lrwxrwxrwx 1 root root   32 4月  19 16:37 java-1.8.0 -> /etc/alternatives/java_sdk_1.8.0
lrwxrwxrwx 1 root root   40 4月  19 16:37 java-1.8.0-openjdk -> /etc/alternatives/java_sdk_1.8.0_openjdk
drwxr-xr-x 7 root root 4096 4月  19 16:37 java-1.8.0-openjdk-1.8.0.161-0.b14.el7_4.x86_64
lrwxrwxrwx 1 root root   34 4月  19 16:37 java-openjdk -> /etc/alternatives/java_sdk_openjdk
lrwxrwxrwx 1 root root   21 3月  23 14:34 jre -> /etc/alternatives/jre
lrwxrwxrwx 1 root root   27 3月  23 14:34 jre-1.7.0 -> /etc/alternatives/jre_1.7.0
lrwxrwxrwx 1 root root   35 3月  23 14:34 jre-1.7.0-openjdk -> /etc/alternatives/jre_1.7.0_openjdk
lrwxrwxrwx 1 root root   54 3月  23 14:34 jre-1.7.0-openjdk-1.7.0.171-2.6.13.0.el7_4.x86_64 -> java-1.7.0-openjdk-1.7.0.171-2.6.13.0.el7_4.x86_64/jre
lrwxrwxrwx 1 root root   27 3月  23 14:34 jre-1.8.0 -> /etc/alternatives/jre_1.8.0
lrwxrwxrwx 1 root root   35 3月  23 14:34 jre-1.8.0-openjdk -> /etc/alternatives/jre_1.8.0_openjdk
lrwxrwxrwx 1 root root   51 3月  23 14:34 jre-1.8.0-openjdk-1.8.0.161-0.b14.el7_4.x86_64 -> java-1.8.0-openjdk-1.8.0.161-0.b14.el7_4.x86_64/jre
lrwxrwxrwx 1 root root   29 3月  23 14:34 jre-openjdk -> /etc/alternatives/jre_openjdk
```

前面安装的是1.8.0版本，所以找到jre-1.8.0-openjdk-1.8.0.161-0.b14.el7_4.x86_64

然后打开linux全局环境变量配置文件
```s
[root@izm5e944c3bh8eikqxjle5z ~]# vi /etc/profile
``` 
在文件末尾加上一下内容
```s
#set java environment  
JAVA_HOME=/usr/lib/jvm/jre-1.8.0-openjdk-1.8.0.161-0.b14.el7_4.x86_64
PATH=$PATH:$JAVA_HOME/bin  
CLASSPATH=.:$JAVA_HOME/lib/dt.jar:$JAVA_HOME/lib/tools.jar  
export JAVA_HOME  CLASSPATH  PATH 
```
保存关闭，执行下面命令使配置生效
```s
[root@izm5e944c3bh8eikqxjle5z ~]#  source  /etc/profile  
```
## 验证：
```s
[root@izm5e944c3bh8eikqxjle5z ~]# java -version
openjdk version "1.8.0_161"
OpenJDK Runtime Environment (build 1.8.0_161-b14)
OpenJDK 64-Bit Server VM (build 25.161-b14, mixed mode)
[root@izm5e944c3bh8eikqxjle5z ~]# javac -version
javac 1.8.0_161
```
