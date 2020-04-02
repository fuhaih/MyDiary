## java安装

>安装

下载安装包进行安装

>配置环境变量

JAVA_HOME : `C:\Program Files\Java\jdk-13.0.2`     
Path : `%JAVA_HOME%\bin`

>测试

```
>javac
>java -version
```

## maven 安装

>安装

下载压缩包解压到`D:\apache\apache-maven-3.6.3`目录下

>配置环境变量

M2_HOME : `D:\apache\apache-maven-3.6.3`     
Path : `%M2_HOME%\bin`

>测试

```
>mvn -version
```

>配置镜像

修改`D:\apache\apache-maven-3.6.3\conf\setting.xml`,在mirrors标签下添加下列镜像配置
```xml
<!-- 阿里云仓库 -->
<mirror>
  <id>alimaven</id>
  <mirrorOf>central</mirrorOf>
  <name>aliyun maven</name>
  <url>http://maven.aliyun.com/nexus/content/repositories/central/</url>
</mirror>
<!-- 中央仓库1 -->
<mirror>
  <id>repo1</id>
  <mirrorOf>central</mirrorOf>
  <name>Human Readable Name for this Mirror.</name>
  <url>http://repo1.maven.org/maven2/</url>
</mirror>
<!-- 中央仓库2 -->
<mirror>
  <id>repo2</id>
  <mirrorOf>central</mirrorOf>
  <name>Human Readable Name for this Mirror.</name>
  <url>http://repo2.maven.org/maven2/</url>
</mirror>
```
>配置仓库

```xml
<localRepository>D:\apache\maven-repository</localRepository>
```

## tomcat 安装

>下载tomcat

这里下载的是`apache-tomcat-9.0.33-windows-x64.zip`

>解压

解压到`D:\apache\apache-tomcat-9.0.33`目录下

>配置

配置文件是在conf/server.xml

>环境变量配置

个人倾向于不配置环境变量，方便多个版本使用，类似mongodb一样

>测试

bin目录下点击`startup.bat`文件，就能运行tomcat

浏览器输入`localhost:8080`

>安装成服务

bin目录下使用管理员身份运行命令

```sh
# 安装服务
service.bat install
# 启动服务
net start Tomcat9
```

>tomcat控制台输出乱码问题

修改`\conf\logging.properties`文件，注释掉`java.util.logging.ConsoleHandler.encoding = UTF-8`

该问题目前出现在Tomcat9中

## vs code

[Java in Visual Studio Code](https://code.visualstudio.com/docs/languages/java)

>安装插件

* Debugger for Java
* Java Dependency Viewer
* Java Extension Pack
* Java Test Runner
* Language Support for Java(TM) by Red Hat

>配置java和maven

```json
{
  "java.home": "C:\\Program Files\\Java\\jdk-13.0.2",
  "java.configuration.maven.userSettings": "D:\\apache\\apache-maven-3.6.3\\conf\\settings.xml",
  "maven.executable.path": "D:\\apache\\apache-maven-3.6.3\\bin\\mvn.cmd",
  "maven.terminal.useJavaHome": true,
  "maven.terminal.customEnv": [
    {
      "environmentVariable": "JAVA_HOME",
      "value": "C:\\Program Files\\Java\\jdk-13.0.2"
    }
  ]
}
```
