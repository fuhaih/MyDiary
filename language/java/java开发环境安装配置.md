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
