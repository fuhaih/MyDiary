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
添加完成后，右下角弹框点击`import changes`,就会到本地仓库导入包，如果没有就会远程下载到本地仓库再导入

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



## 视图

> 使用视图
添加依赖感

```xml
<!--视图包-->
<dependency>
  <groupId>javax.servlet</groupId>
  <artifactId>jstl</artifactId>
  <version>1.2</version>
</dependency>
```
dispatcher-servlet.xml中配置viewResolver


```xml
<beans>
    <!--视图解析器-->
  <bean id="viewResolver" class="org.springframework.web.servlet.view.InternalResourceViewResolver">
      <property name="viewClass" value="org.springframework.web.servlet.view.JstlView"/>
      <!--视图前缀，也就是要报视图放在/WEB-INF/views/路径下-->
      <property name="prefix" value="/WEB-INF/views/"/>
      <property name="suffix" value=".jsp"/>
  </bean>
</beans>
```

在配置的路径`/WEB-INF/views/`下添加一个jsp文件`home.jsp`,在控制器中编写home方法

```java
@Controller
@RequestMapping(value = "/" ,produces = "application/json;charset=utf-8")
public class HomeController {
    @RequestMapping(value = "home")
    public String Home(){
        return "home";
    }
}
```

在默认情况下，控制器方法返回的值会交给视图解析器处理，这里返回的字符串就是视图名称。当加上`@ResponseBody`注解时是数据会经过转换器转换输入到输入流。


>视图传输模型

* ModelAndView

* Model／Map／ModelMap

* @SessionAttributes

* @ModelAttribute
>el表达式不支持

查看`web.xml`文件，Servlet2.3/jsp1.2默认是不支持el表达式的

**解决方案1:**
jsp文件头中添加`isELIgnored="false"`
```jsp
<%@ page contentType="text/html;charset=UTF-8" language="java" isELIgnored="false" %>
```

**解决方案2**

`web.xml` 配置文件中配置Servlet为2.5版本
```xml
<?xml version="1.0" encoding="UTF-8"?>
<web-app xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
         xmlns="http://java.sun.com/xml/ns/javaee"
         xsi:schemaLocation="http://java.sun.com/xml/ns/javaee http://java.sun.com/xml/ns/javaee/web-app_2_5.xsd"
         id="WebApp_ID" version="2.5">

</web-app>
```


## Spring和SpringMVC

> mvc:annotation-driven

```xml
<mvc:annotation-driven ignoreDefaultModelOnRedirect="true" conversion-service="" validator="" message-codes-resolver="">  
        <mvc:argument-resolvers>  
            <bean class="com.lay.user.util.CustomerArgumentResolver"/>  
        </mvc:argument-resolvers>  
        <mvc:message-converters>  
            <bean class=""/>  
        </mvc:message-converters>  
        <mvc:return-value-handlers>  
            <bean class=""/>  
        </mvc:return-value-handlers>  
</mvc:annotation-driven>  
```

* <mvc:argument-resolvers>

参数解析器，可通过实现HandlerMethodArgumentResolver接口实现，该实现不会覆盖原有spring mvc内置解析对参数的解析，要自定义的内置支持参数解析可以考虑注册RequestMappingHandlerAdapter

* <mvc:return-value-handlers>
 
对返回值的处理。自定义实现类需要实现HandlerMethodReturnValueHandler，这个和上面提到的mvc:argument-resolvers自定义实现类的使用上几乎没差别。同样的，如果想改变内置返回值处理的话请直接注入RequestMappingHandlerAdapter

* <mvc:message-converters>  

主要是对 @RequestBody 参数和 @ResponseBody返回值的处理，可选的，在这里注册的HttpMessageConverter默认情况下优先级是高于内置的转换器的，那么怎么自定义转换器呢？通过实现HttpMessageConverter<T>接口便可以了，当然，你也可以继承AbstractHttpMessageConverter<T>，这样做会更轻松，具体做法参考源码


**mvc:annotation-driven默认情况下注册了一下的转换器**

```
StringHttpMessageConverter
FormHttpMessageConverter
ByteArrayHttpMessageConverter
MarshallingHttpMessageConverter
MappingJacksonHttpMessageConverter
SourceHttpMessageConverter
BufferedImageHttpMessageConverter
```

所以很多时候要使用转换器，只要安装包就行了，比如说要使用json转换器，只需要安装相应的json包就行了。

>ResponBody

上面说了mvc:annotation-driven的作用，下面就实际使用一下，通过`ResponBody`注解来返回json格式数据

添加响应的json包
```xml
<!-- https://mvnrepository.com/artifact/com.fasterxml.jackson.core/jackson-databind -->
<dependency>
  <groupId>com.fasterxml.jackson.core</groupId>
  <artifactId>jackson-databind</artifactId>
  <version>${jackson.version}</version>
</dependency>
<!-- https://mvnrepository.com/artifact/com.fasterxml.jackson.core/jackson-core -->
<dependency>
  <groupId>com.fasterxml.jackson.core</groupId>
  <artifactId>jackson-core</artifactId>
  <version>${jackson.version}</version>
</dependency>
<!-- https://mvnrepository.com/artifact/com.fasterxml.jackson.core/jackson-annotations -->
<dependency>
  <groupId>com.fasterxml.jackson.core</groupId>
  <artifactId>jackson-annotations</artifactId>
  <version>${jackson.version}</version>
</dependency>
```
由于`mvc:annotation-driven`已经默认注册了`MappingJacksonHttpMessageConverter`,所以不用多做配置，直接使用就行了

```java
@Controller
@RequestMapping(value = "/",produces = "application/json;charset=utf-8")
public class HomeController {
    @RequestMapping(value = "user")
    @ResponseBody
    public User getUser(User user)
    {
        User user = new User("test","testpwd");
        return  user;
    }
}
```

**注意：**

要在`produces`中定义数据类型，这里定义为`application/json;charset=utf-8`


## idea问题

> 调试

点击`Run->Debug 'Tomcat'` 或者按 `F9`快捷键

`Run->Run 'Tomcat'` 是直接运行项目，不会进入断点调试

>断点只能进入一次

这个是因为点击`Run To Cursor`来跳到下一个断点导致的，跳断点应该使用快捷键`F9`


>绿色波浪线警告

有时候有些个人项目的包名不是正常单词，IDE会检查出拼写问题，在引用包时也不会智能提示，这时候可以`save to project-level dictionary`,把该单词添加到字典中

## rest web api

@RestController

## idea 使用 git

>JetBrains.gitignore

[JetBrains.gitignore](https://github.com/github/gitignore/blob/master/Global/JetBrains.gitignore)

## 配置

>properties配置文件

在`resource`文件夹中创建`database.properties`配置文件

```properties
driver =com.mysql.cj.jdbc.Driver
url = mysql:jdbc://localhost:3306//test?userUnicode=true&characterEncoding=utf-8&serverTimezone=UFC;
user =root
password = root 
```

然后在`dispatcher-servlet.xml`中进行如下配置
```xml
<context:property-placeholder  location="classpath:database.properties"/>
```

最后通过注解方式获取配置信息

## 拦截器

## beans配置

## Hibernate

## 单元测试

## 打包部署

## docker部署

## yml使用

## spring cloud

## swagger

## 权限控制问题

## 多项目管理

pom.xml `<modules></modules>`