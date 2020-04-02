## 配置maven

这个可以看idea.md

## 使用maven创建一个项目

>创建webapp项目

File->New->Project->Maven

选中Create from archetype

选择org.apache.maven.archetypes:maven-archetype-webapp

Name : `webapp` (项目名)
 
Location :`D:\fuhai\workspace\webapp`

GroupId :`org.example`(cn.fuhai)

ArtifactId :`webapp` (项目名)

下一步的maven配置选择自己配置好的maven和setting文件。

项目创建好之后，会下载包到仓库中，idea右下角会有`Maven projects need to be imported`,点击`Enable Auto-Import`，这样在修改pom.xml文件时会自动下载依赖。

>修改目录结构

新生成的webapp项目的目录结构是这样的

```
webapp
  ----src
      ----main
          ----webapp
              ----WEB-INF
                  ----web.xml
              ----index.jsp
  ----target
  ----pom.xml
  ----webapp.iml
```

target文件夹是在项目编译后生成的，源文件等信息都在src目录下，接下来需要完善一下项目的目录结构，添加`java`、`resources`、`test`这三个文件夹
```
webapp
  ----src
      ----main
          ----java
          ----resources
          ----webapp
              ----WEB-INF
                  ----web.xml
              ----index.jsp
      ----test
  ----target
  ----pom.xml
  ----webapp.iml
```
`java` ： 源代码目录

`resources` ： 资源目录，一般用来放置配置文件

`test` : 测试目录，用来写单元测试

idea可以配置文件夹属性，通过右键文件夹->Make Directory as可以设置。

* Sources Root  (源文件目录)
* Resouces Root   (资源目录)
* Test Resources Root   (测试目录)
* Excluded    (执行目录，target)
* Unmark as   (unmark不标记)
* Generated Sources Root

>添加spring依赖

[maven仓库](https://mvnrepository.com/)

创建的项目下默认会有junit依赖
```xml
<dependency>
  <groupId>junit</groupId>
  <artifactId>junit</artifactId>
  <version>4.11</version>
  <scope>test</scope>
</dependency>
```

现在要添加常用的几个spring依赖,需要什么版本可以在仓库里找。一般spring的组件版本都是一致的。
```xml
<properties>
  <project.build.sourceEncoding>UTF-8</project.build.sourceEncoding>
  <maven.compiler.source>1.7</maven.compiler.source>
  <maven.compiler.target>1.7</maven.compiler.target>
  <springframework.version>5.1.5.RELEASE</springframework.version>
</properties>

<dependencies>
  <!--spring框架的三个基础包-->
  <dependency>
    <groupId>org.springframework</groupId>
    <artifactId>spring-core</artifactId>
    <version>${springframework.version}</version>
  </dependency>

  <dependency>
    <groupId>org.springframework</groupId>
    <artifactId>spring-beans</artifactId>
    <version>${springframework.version}</version>
  </dependency>

  <dependency>
    <groupId>org.springframework</groupId>
    <artifactId>spring-context</artifactId>
    <version>${springframework.version}</version>
  </dependency>

  <!--springMVC的两个包-->
  <dependency>
    <groupId>org.springframework</groupId>
    <artifactId>spring-web</artifactId>
    <version>${springframework.version}</version>
  </dependency>

  <dependency>
    <groupId>org.springframework</groupId>
    <artifactId>spring-webmvc</artifactId>
    <version>${springframework.version}</version>
  </dependency>

  <dependency>
    <groupId>junit</groupId>
    <artifactId>junit</artifactId>
    <version>4.11</version>
    <scope>test</scope>
  </dependency>
</dependencies>
```

如上，由于各个spring组件的版本都是一致的，要升级的时候也是要同时修改，所以可以直接在`properties`标签中定义spring版本`springframework.version`,然后通过`${springframework.version}`方式来使用。

在添加完依赖后，idea会自动下载依赖到本地仓库，当配置文件没有红色警告时说明下载完成了。

>添加框架支持

右键项目目录->Add Frameworks Support

选择Spring -> Spring MVC

Use libraty会自动选中刚刚配置的包
`org.springframework:spring-webmvc:5.1.5RELEASE`

点击OK，`WEB-INF`目录下会多出两个配置文件 `applicationContext.xml`和`dispatcher-servlet.xml`

**注意**

如果添加框架支持时没有Spring选项，可能是项目已经有Spring支持，但是不完整，所以需要移除

File->Project Structure->Moudules

找到Srping然后移除。

>配置mvc


web.xml
```xml
<!DOCTYPE web-app PUBLIC
 "-//Sun Microsystems, Inc.//DTD Web Application 2.3//EN"
 "http://java.sun.com/dtd/web-app_2_3.dtd" >

<web-app>
  <display-name>Archetype Created Web Application</display-name>
    <!--spring beans 配置文件所在的目录-->
    <context-param>
        <param-name>contextConfigLocation</param-name>
        <param-value>/WEB-INF/applicationContext.xml</param-value>
    </context-param>
    <!--为spring配置一个监听器，用于监听spring-bean上下文加载-->
    <listener>
        <listener-class>org.springframework.web.context.ContextLoaderListener</listener-class>
    </listener>
    <!--springMVC 配置-->
    <servlet>
        <servlet-name>dispatcher</servlet-name>
        <servlet-class>org.springframework.web.servlet.DispatcherServlet</servlet-class>
        <init-param>
            <param-name>contextConfigLocation</param-name>
            <param-value>/WEB-INF/dispatcher-servlet.xml</param-value>
        </init-param>
        <load-on-startup>1</load-on-startup>
    </servlet>
    <servlet-mapping>
        <servlet-name>dispatcher</servlet-name>
        <url-pattern>/</url-pattern>
    </servlet-mapping>
</web-app>
```

dispatcher-servlet.xml

```xml
<?xml version="1.0" encoding="UTF-8"?>
<beans xmlns="http://www.springframework.org/schema/beans"
       xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
       xmlns:context="http://www.springframework.org/schema/context"
       xmlns:mvc="http://www.springframework.org/schema/mvc"
       xsi:schemaLocation="http://www.springframework.org/schema/beans http://www.springframework.org/schema/beans/spring-beans.xsd http://www.springframework.org/schema/context http://www.springframework.org/schema/context/spring-context.xsd http://www.springframework.org/schema/mvc http://www.springframework.org/schema/mvc/spring-mvc.xsd">
    <!--扫描包-->
    <context:component-scan base-package="controller"/>
    <!--使用action-->
    <mvc:annotation-driven/>
    <!--使用静态资源-->
    <mvc:default-servlet-handler/>
    <!--视图解析器-->
    <bean id="viewResolver" class="org.springframework.web.servlet.view.InternalResourceViewResolver">
        <property name="viewClass" value="org.springframework.web.servlet.view.JstlView"/>
        <property name="prefix" value="/WEB-INF/views/"/>
        <property name="suffix" value=".jsp"/>
    </bean>
</beans>
```
>启动调试

在java文件夹下创建一个`controller` package，在上面配置中有配置到

编写一个简单的控制器

```java
package controller;

import  org.springframework.stereotype.Controller;
import  org.springframework.web.bind.annotation.RequestMapping;
import  org.springframework.web.bind.annotation.ResponseBody;

@Controller
@RequestMapping(value = "/" ,produces = "application/json;charset=utf-8")
public class IndexController {
    @RequestMapping(value = "test")
    @ResponseBody
    public String test(){
        return "test";
    }
}
```

Run->Edit Configurations->Templates中选择Tomcat local模板，Create configuration,会添加一个Tomcat配置。选中该配置

`Server`中配置端口号等信息

`Deployment`中添加Artifact

然后就可以run调试，选中配置的Tomcat。