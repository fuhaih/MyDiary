# 配置maven

这个可以看idea.md

# 使用maven创建一个项目

## 创建webapp项目

File->New->Project->Maven

选中Create from archetype

选择org.apache.maven.archetypes:maven-archetype-webapp

Name : `webapp` (项目名)
 
Location :`D:\fuhai\workspace\webapp`

GroupId :`org.example`(cn.fuhai)

ArtifactId :`webapp` (项目名)

下一步的maven配置选择自己配置好的maven和setting文件。

项目创建好之后，会下载包到仓库中，idea右下角会有`Maven projects need to be imported`,点击`Enable Auto-Import`，这样在修改pom.xml文件时会自动下载依赖。

## 修改目录结构

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

## 添加spring依赖

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

## 添加框架支持

右键项目目录->Add Frameworks Support

选择Spring -> Spring MVC

Use libraty会自动选中刚刚配置的包
`org.springframework:spring-webmvc:5.1.5RELEASE`

点击OK，`WEB-INF`目录下会多出两个配置文件 `applicationContext.xml`和`dispatcher-servlet.xml`

**注意**

如果添加框架支持时没有Spring选项，可能是项目已经有Spring支持，但是不完整，所以需要移除

File->Project Structure->Moudules

找到Srping然后移除。

## 配置mvc


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
## 启动调试

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



# 视图

## 使用视图
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


## 视图传输模型

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


# Spring和SpringMVC

## mvc:annotation-driven

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

## ResponBody

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

## bean注入方式


> xml配置

可以通过xml配置来注入bean

视图处理器就是通过xml来配置的

```xml
<bean id="viewResolver" class="org.springframework.web.servlet.view.InternalResourceViewResolver">
    <property name="viewClass" value="org.springframework.web.servlet.view.JstlView"/>
    <property name="prefix" value="/WEB-INF/views/"/>
    <property name="suffix" value=".jsp"/>
</bean>
```


> 注解

通过`@Controller`,`@Component`,`@Service`,`@Repository`这几个注解都能注入bean，不过需要配置`context:component-scan`,容器就会从配置路径中查找这几个注解，把bean注入到容器中。

```xml
<context:component-scan base-package="cn.fuhai.controller"/>
```
spring mvc的控制器就是以这种方式注入的。

## spring容器和spring mvc容器

一个应用程序中可以有多个ICO容器，默认情况下会有spring和spring mvc这两个容器，spring 容器称为根容器，spring mvc容器是spring容器的子容器，负责mvc相关的bean的管理。

`applicationContext.xml`为spring容器的配置文件，`dispatcher-servlet.xml`为spring mvc的配置文件，这两个都要在`web.xml`中进行配置。

这两个容器都可以通过xml配置和注解方式来进行bean的注册。
在使用注解进行注册时，如果两个配置同时扫描的一个包，那么那个包下的bean会注册到spring容器中，所以不应该把控制器所在的包配置到 `applicationContext.xml` 的`context:component-scan`中，这样会把控制器注册到spring 容器中，导致spring mvc获取不到路由（具体原因尚未清楚）。


父子容器的几个特点

* 子容器可以访问父容器的bean

* 父容器不能访问子容器的bean

* 子容器不能通过@Value("${}")来获取父容器的properties配置

* 

## properties配置例子
创建配置文件`jdbc.properties`放在资源包下

```
jdbc.driver =com.mysql.cj.jdbc.Driver
jdbc.url = mysql:jdbc://localhost:3306//test?userUnicode=true&characterEncoding=utf-8&serverTimezone=UFC;
jdbc.user =root
jdbc.password = root
```


首先要先知道要把配置注册到哪个容器中，这样方便使用`${}`来获取配置

这里只是把`viewResolver`和`Controller`配置在spring mvc容器中，其他的bean包括properties配置都放在spring容器中。

spring mvc 容器配置文件`dispatcher-servlet.xml`
```xml
<!--这里只扫描controller包，其他包交个spring扫描-->
<context:component-scan base-package="cn.fuhai.controller"/>
```

spring容器配置文件`applicationContext.xml`
```xml
<context:component-scan base-package="cn.fuhai.*">
    <context:exclude-filter type="annotation" expression="org.springframework.stereotype.Controller"/>
</context:component-scan>
```
spring中扫描`cn.fuhai`包下所有的注解，但是过滤掉`@Controller`注解。

然后在`applicationContext.xml`中配置properties文件
```xml
<bean id="propertyConfigurer"
      class="org.springframework.beans.factory.config.PropertyPlaceholderConfigurer">
    <property name="locations">
        <array>
            <value>classpath:jdbc.properties</value>
        </array>
    </property>
</bean>
```
**classpath：** (这个暂时不知道)

然后配置相应的模型类

```java
package cn.fuhai.beans.config;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Component;

public class DatabaseConfig {
    private String Driver;
    private String Url;
    private String User;
    private String Password;

    public String getDriver() {
        return Driver;
    }

    public String getUrl() {
        return Url;
    }

    public String getUser() {
        return User;
    }

    public String getPassword() {
        return Password;
    }

    public void setDriver(String driver) {
        Driver = driver;
    }

    public void setUrl(String url) {
        Url = url;
    }

    public void setUser(String user) {
        User = user;
    }

    public void setPassword(String password) {
        Password = password;
    }
}
```

最后在`applicationContext.xml`配置文件中通过xml方式注册bean

```xml
<bean id="dataSource"
      class="cn.fuhai.beans.config.DatabaseConfig">
    <property name="Driver">
        <value>${jdbc.driver}</value>
    </property>
    <property name="Url" >
        <value>${jdbc.url}</value>
    </property>
    <property name="User" >
        <value>${jdbc.user}</value>
    </property>
    <property name="Password">
        <value>${jdbc.password}</value>
    </property>
</bean>
```

也可以通过注解方式来注册再通过@Value注解获取配置。

```java
package cn.fuhai.beans.config;

import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.ComponentScan;
import org.springframework.stereotype.Component;
import org.springframework.stereotype.Service;
@Component("dataConfig")
public class DatabaseConfig {
    @Value("${jdbc.driver}")
    public String Driver;
    @Value("${jdbc.url}")
    public String Url;
    @Value("${jdbc.user}")
    public String User;
    @Value("${jdbc.password}")
    public String Password;

    public String getDriver() {
        return Driver;
    }

    public String getUrl() {
        return Url;
    }

    public String getUser() {
        return User;
    }

    public String getPassword() {
        return Password;
    }

    public void setDriver(String driver) {
        Driver = driver;
    }

    public void setUrl(String url) {
        Url = url;
    }

    public void setUser(String user) {
        User = user;
    }

    public void setPassword(String password) {
        Password = password;
    }
}
```

该bean是在`cn.fuhai`包下，会被spring容器扫描到，并注册到容器中。


通过Autowired注入

```java
public class HomeController {
    private DatabaseConfig databaseConfig;
    @Autowired
    public void setDatabaseConfig(DatabaseConfig databaseConfig) {
        this.databaseConfig = databaseConfig;
    }
    private DatabaseValueConfig databaseValueConfig;
}
```


## 注入方式

注入可以通过xml进行配置，也可以使用`@Autowire`、`@Resource`等注解进行自动装配。

>构造函数注入

>set函数注入

>字段注入

# 数据库

## jdbc

>包引用

首先要找到对应数据库的jdbc实现，这里用的mysql，所以引用下列包

```xml
<!--mysql-connector-->
<dependency>
  <groupId>mysql</groupId>
  <artifactId>mysql-connector-java</artifactId>
  <version>${mysql-connector.version}</version>
</dependency>
<dependency>
  <groupId>org.apache.commons</groupId>
  <artifactId>commons-pool2</artifactId>
  <version>2.8.0</version>
</dependency>
<dependency>
  <groupId>org.apache.commons</groupId>
  <artifactId>commons-dbcp2</artifactId>
  <version>2.7.0</version>
</dependency>
```
`mysql-connector-java`是jdbc的mysql实现，`commons-pool2`和`commons-dbcp2`这两个是连接池相关包，jdbc默认是不实现连接池的，所以这里使用连接池相关包，用连接池来创建数据库连接，可以优化连接的创建。

>配置

```conf
jdbc.driver=com.mysql.jdbc.Driver
jdbc.url=jdbc:mysql://47.106.162.159:3306/selffate
jdbc.user=root
jdbc.password=Root@123
```

>访问数据库

```java
@RequestMapping(value = "jdbc")
@ResponseBody
public User TestJdbc() throws SQLException {
    BasicDataSource dataSource = new BasicDataSource();
    dataSource.setDriverClassName(databaseConfig.getDriver());
    dataSource.setUrl(databaseConfig.getUrl());
    dataSource.setUsername(databaseConfig.getUser());
    dataSource.setPassword(databaseConfig.getPassword());
    Connection connection=dataSource.getConnection();
    String sql="select * from user";
    PreparedStatement pstm = connection.prepareStatement(sql);
    ResultSet set = pstm.executeQuery();
    set.next();
    Integer uid = set.getInt("uid");
    String uname = set.getString("uname");
    String upassword = set.getString("upassword");
    User user= new User();
    user.setUid(uid);
    user.setUname(uname);
    user.setUpassword(upassword);
    return user;
}
```

## orm框架(hibernate)
>包引用

胶水包 spring-orm

orm包 

jdbc包

xml解析包

```xml
<!--spring orm 相关包-->
<!--它增加了支持(胶水代码)以将各种ORM解决方案与Spring集成在一起,包括Hibernate 3/4,iBatis,JDO和JPA.当然,它本身并不是ORM,它只是一座桥梁.您仍然需要包括相关的库.-->
<dependency>
  <groupId>org.springframework</groupId>
  <artifactId>spring-orm</artifactId>
  <version>${springframework.version}</version>
</dependency>
<dependency>
  <groupId>org.springframework</groupId>
  <artifactId>spring-tx</artifactId>
  <version>${springframework.version}</version>
</dependency>
<dependency>
  <groupId>javax.annotation</groupId>
  <artifactId>javax.annotation-api</artifactId>
  <version>1.3.2</version>
</dependency>
<!--mysql-connector-->
<dependency>
  <groupId>mysql</groupId>
  <artifactId>mysql-connector-java</artifactId>
  <version>${mysql-connector.version}</version>
</dependency>
<dependency>
  <groupId>org.apache.commons</groupId>
  <artifactId>commons-pool2</artifactId>
  <version>2.8.0</version>
</dependency>
<dependency>
  <groupId>org.apache.commons</groupId>
  <artifactId>commons-dbcp2</artifactId>
  <version>2.7.0</version>
</dependency>
<!--xml解析-->
<dependency>
  <groupId>org.dom4j</groupId>
  <artifactId>dom4j</artifactId>
  <version>2.1.1</version>
</dependency>
<dependency>
  <groupId>xalan</groupId>
  <artifactId>xalan</artifactId>
  <version>2.7.1</version>
</dependency>
<dependency>
  <groupId>xerces</groupId>
  <artifactId>xercesImpl</artifactId>
  <version>2.9.1</version>
</dependency>
<!--hibernate-->
<dependency>
  <groupId>org.hibernate</groupId>
  <artifactId>hibernate-core</artifactId>
  <version>${hibernate.version}</version>
</dependency>
<dependency>
  <groupId>org.javassist</groupId>
  <artifactId>javassist</artifactId>
  <version>3.24.0-GA</version>
</dependency>
```

>配置

mysql.jdbc.properties

```conf
jdbc.driver=com.mysql.jdbc.Driver
# jdbc.url=jdbc:mysql://47.106.162.159:3306/selffate?user=root&password=Shifthfh@123&useUnicode=true&characterEncoding=gbk&autoReconnect=true&failOverReadOnly=false
jdbc.url=jdbc:mysql://47.106.162.159:3306/selffate?serverTimezone=GMT%2B8&useSSL=false
jdbc.user=root
jdbc.password=Shifthfh@123

jdbc.hibernate.dialect=org.hibernate.dialect.MySQL5Dialect
jdbc.hibernate.show_sql=true
# none：默认值，什么都不做，每次启动项目，不会对数据库进行任何验证和操作
# create：每次运行项目，没有表会新建表，如果表内有数据会被清空
# create-drop：每次程序结束的时候会清空表
# update：每次运行程序，没有表会新建表，但是表内有数据不会被清空，只会更新表结构。
# validate：运行程序会校验数据与数据库的字段类型是否相同，不同会报错
jdbc.hibernate.hbm2ddl.auto=none
```

applicationContext.xml
```xml
<!--连接池配置-->
<bean id="dataSource" class="org.apache.commons.dbcp2.BasicDataSource" destroy-method="close">
    <property name="driverClassName" value="${jdbc.driver}"/>
    <property name="url" value="${jdbc.url}"/>
    <property name="username" value="${jdbc.user}"/>
    <property name="password" value="${jdbc.password}"/>
</bean>
<!--sessionFactory配置-->
<bean id="sessionFactory" class="org.springframework.orm.hibernate5.LocalSessionFactoryBean">
    <property name="dataSource" ref="dataSource"></property>
    <property name="packagesToScan">
        <list>
            <value>cn.fuhai.entity</value>
        </list>
    </property>
    <property name="hibernateProperties">
        <value>
            hibernate.hbm2ddl.auto=${jdbc.hibernate.hbm2ddl.auto}
            hibernate.dialect=${jdbc.hibernate.dialect}
            hibernate.show_sql=${jdbc.hibernate.show_sql}
        </value>
    </property>
</bean>
<bean id="transactionManager" class="org.springframework.orm.hibernate5.HibernateTransactionManager">
    <property name="sessionFactory" ref="sessionFactory"/>
</bean>
<tx:annotation-driven transaction-manager="transactionManager"/>

```

配置entity、dao、service

entity
```java
@Entity
public class User {
    private Integer uid;
    private String uname;
    private String upassword;
    @Id
    public Integer getUid() {
        return uid;
    }

    public String getUname() {
        return uname;
    }

    public String getUpassword() {
        return upassword;
    }

    public void setUid(Integer uid) {
        this.uid = uid;
    }

    public void setUname(String uname) {
        this.uname = uname;
    }

    public void setUpassword(String upassword) {
        this.upassword = upassword;
    }
}


```
dao
```java
@Repository
public class UserDao {
    @Resource
    SessionFactory sessionFactory;
    public List<User> Get(){
        sessionFactory.createEntityManager();
        String hql = "select * from user";
        Session session = sessionFactory.openSession();
        Query<User> query  =  session.createSQLQuery(hql).addEntity(User.class);
        return query.getResultList();
    }
}
```
service
```java
@Service
public class UserService {
    @Resource
    UserDao userDao;
    public List<User> GetUsers(){
        return userDao.Get();
    }
}
```

这里用的是sql语句查询，hibernate也可以进行动态查询

>Criteria动态查询

```java
@Repository
public class UserDao {
    @Resource
    SessionFactory sessionFactory;
    public List<User> Get(){
        sessionFactory.createEntityManager();
        Session session = sessionFactory.openSession();
        CriteriaBuilder criteriaBuilder= session.getCriteriaBuilder();
        CriteriaQuery<User> cQuery = criteriaBuilder.createQuery(User.class);
        Root<User> user = cQuery.from(User.class);
        Predicate predicate = criteriaBuilder.equal(user.get("uid"),1);
        cQuery.where();
        Query<User> query = session.createQuery(cQuery);
        return query.getResultList();
    }
}
```

>表名

动态查询时需要指定表名，默认情况下是使用类名作为表名，也可以通过`@Entity`注解来指定,需要注意表名的大小写，mysql是大小写敏感的。
```java
@Entity(name="user")
public class User {
    private Integer uid;
    private String uname;
    @Id
    public Integer getUid() {
        return uid;
    }

    public String getUname() {
        return uname;
    }
    public void setUid(Integer uid) {
        this.uid = uid;
    }

    public void setUname(String uname) {
        this.uname = uname;
    }

}
```
也可以通过命名策略来处理


>命名策略
 
逻辑名称策略 hibernate.implicit_naming_strategy

物理名称策略 hibernate.physical_naming_strategy

编写物理命名策略,实现表名大写转小写

```java
public class SpringPhysicalNamingStrategy implements PhysicalNamingStrategy {
    @Override
    public Identifier toPhysicalCatalogName(Identifier identifier, JdbcEnvironment jdbcEnvironment) {
        return identifier;
    }

    @Override
    public Identifier toPhysicalSchemaName(Identifier identifier, JdbcEnvironment jdbcEnvironment) {
        return identifier;
    }

    @Override
    public Identifier toPhysicalTableName(Identifier identifier, JdbcEnvironment jdbcEnvironment) {
        String lower = identifier.getText().toLowerCase();
        return identifier.toIdentifier(lower);
    }

    @Override
    public Identifier toPhysicalSequenceName(Identifier identifier, JdbcEnvironment jdbcEnvironment) {
        return identifier;
    }

    @Override
    public Identifier toPhysicalColumnName(Identifier identifier, JdbcEnvironment jdbcEnvironment) {
        return identifier;
    }
}

```

配置命名规则

```xml
<bean id="springPhysicalNamingStrategy" class="cn.fuhai.model.naming.SpringPhysicalNamingStrategy">
</bean>
<!--sessionFactory配置-->
<bean id="sessionFactory" class="org.springframework.orm.hibernate5.LocalSessionFactoryBean">
    <property name="dataSource" ref="dataSource"></property>
    <property name="packagesToScan">
        <list>
            <value>cn.fuhai.entity</value>
        </list>
    </property>
    <property name="physicalNamingStrategy" ref="springPhysicalNamingStrategy">
    </property>
<!--        <property name="implicitNamingStrategy" ref="implicitNamingStrategy"></property>-->
    <property name="hibernateProperties">
        <value>
            hibernate.hbm2ddl.auto=${jdbc.hibernate.hbm2ddl.auto}
            hibernate.dialect=${jdbc.hibernate.dialect}
            hibernate.show_sql=${jdbc.hibernate.show_sql}
        </value>
    </property>
</bean>

```

## 事务操作

>openSession和getCurrentSession

openSession需要手动释放session，事务也是需要手动创建、提交、回滚。

getCurrentSession从上下文中获取session，能自动释放，还能通过注解方式使用事务。

`org.springframework.orm.hibernate5.SpringSessionContext`

>编程式事务、声明式事务 、 aop事务

编程式事务是通过编程的方式来使用事务

```java
public void Add(List<User> users) throws Exception {
    Session session = sessionFactory.openSession();
    Transaction tran = session.beginTransaction();
    try{
        for (int i =0;i<users.size();i++){
            User user = users.get(i);
            session.save(user);
        }
        tran.commit();
    }catch (Exception ex){
        tran.rollback();
        throw ex;
    }finally {
            session.close();
    }
}
```
使用openSession，需要手动实现session的释放，手动提交事务，回滚事务。

声明式事务是通过声明方式使用事务

dao
```java
public void Add(List<User> users) throws Exception {
    Session session = sessionFactory.getCurrentSession();
    for (int i =0;i<users.size();i++){
        User user = users.get(i);
        session.save(user);
    }
}
```
service
```java
@Transactional(rollbackFor=Exception.class,transactionManager = "pgTransactionManager")
public class UserService {
    @Resource
    UserDao userDao;
    public List<User> GetUsers(){
        return userDao.Get();
    }
    public void AddUsers(List<User> users) throws Exception {
        userDao.Add(users);
    }
}
```

通过使用`@Transactional`注解来声明事务，需要注意以下问题

* 1.@Transactional 注解可以被应用于接口定义和接口方法、类定义和类的 public 方法上。
* 2.@Transactional 注解只能应用到 public 可见度的方法上。 如果你在 protected、private 或者 package-visible 的方法上使用 @Transactional 注解，它也不会报错，但是这个被注解的方法将不会展示已配置的事务设置。
* 3.默认情况下，spring会对unchecked异常进行事务回滚；如果是checked异常则不回滚。

继承自Exception的异常都是checked异常，所以都不会回滚，要checked异常也回滚需要如下配置

`@Transactional(rollbackFor=Exception.class)`

而且如果try捕获到了异常，没有抛出时，spring mvc容器是察觉不到异常的，也就不会回滚。

当配置有多个事务管理器时，需要通过transactionManager属性来指定使用的事务管理器。

>JDBC事务和JTA事务

JDBC事务缺点是事务的范围局限在同一个数据库连接，不能垮库进行事务操作

JTA事务可以实现垮库连接或者跨资源的事务管理。


>TransactionTemplate、JdbcTemplate和PlatformTransactionManager



>HibernateDaoSupport

据说已过时，不推荐使用

## hibernate使用多个事务源

这里配置两个数据源，一个mysql的，一个postgresql的数据源。

之前没有使用postgresql，所以现在先添加相关的驱动包

```xml
<dependency>
  <groupId>org.postgresql</groupId>
  <artifactId>postgresql</artifactId>
  <version>42.2.5</version>
</dependency>
```

mysql配置mysql.jdbc.properties

```conf
# 数据源配置
mysql.selffate.jdbc.driver=com.mysql.jdbc.Driver
# jdbc.url=jdbc:mysql://47.106.162.159:3306/selffate?user=root&password=Shifthfh@123&useUnicode=true&characterEncoding=gbk&autoReconnect=true&failOverReadOnly=false
mysql.selffate.jdbc.url=jdbc:mysql://47.106.162.159:3306/selffate?serverTimezone=GMT%2B8&useSSL=false
mysql.selffate.jdbc.user=root
mysql.selffate.jdbc.password=Shifthfh@123

# hibernate 配置
jdbc.hibernate.dialect=org.hibernate.dialect.MySQL5Dialect
jdbc.hibernate.show_sql=true
jdbc.hibernate.hbm2ddl.auto=update
# jdbc.hibernate.naming=org.hibernate.boot.model.naming.PhysicalNamingStrategyStandardImpl
jdbc.hibernate.physical_naming_strategy=org.hibernate.boot.model.naming.PhysicalNamingStrategyStandardImpl

```
postgresql配置postgresql.jdbc.properties

```conf
pg.selffate.jdbc.driver=org.postgresql.Driver
pg.selffate.jdbc.url=jdbc:postgresql://47.106.162.159:5432/selffate
pg.selffate.jdbc.user=fuhai
pg.selffate.jdbc.password=shifthfh@123

pg.jdbc.hibernate.dialect=org.hibernate.dialect.PostgreSQL94Dialect
pg.jdbc.hibernate.show_sql=true
pg.jdbc.hibernate.hbm2ddl.auto=update
```

为了条理清晰一点，把配置文件和数据源的bean都配置在applicationContext.xml中，然后hibernate单独创建一个配置文件`spring-hibernate.xml`，再通过import来引入。
```xml
<bean id="propertyConfigurer"
      class="org.springframework.beans.factory.config.PropertyPlaceholderConfigurer">
    <property name="locations">
        <array>
            <value>classpath:mysql.jdbc.properties</value>
            <value>classpath:postgresql.jdbc.properties</value>
        </array>
    </property>
</bean>
<bean id="mysqlDataSource" class="org.apache.commons.dbcp2.BasicDataSource" destroy-method="close">
    <property name="driverClassName" value="${mysql.selffate.jdbc.driver}"/>
    <property name="url" value="${mysql.selffate.jdbc.url}"/>
    <property name="username" value="${mysql.selffate.jdbc.user}"/>
    <property name="password" value="${mysql.selffate.jdbc.password}"/>
</bean>
<bean id="pgDataSource" class="org.apache.commons.dbcp2.BasicDataSource" destroy-method="close">
    <property name="driverClassName" value="${pg.selffate.jdbc.driver}"/>
    <property name="url" value="${pg.selffate.jdbc.url}"/>
    <property name="username" value="${pg.selffate.jdbc.user}"/>
    <property name="password" value="${pg.selffate.jdbc.password}"/>
</bean>
<import resource="classpath:spring-hibernate.xml"/>
```

spring-hibernate.xml配置文件也是作为hibernate的通用配置，具体mysql和postgresql的配置再分文件，通过import来引用。

```xml
<?xml version="1.0" encoding="UTF-8"?>
<beans xmlns="http://www.springframework.org/schema/beans"
       xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
       xmlns:context="http://www.springframework.org/schema/context"
       xmlns:mvc="http://www.springframework.org/schema/mvc" xmlns:tx="http://www.springframework.org/schema/tx"
       xsi:schemaLocation="http://www.springframework.org/schema/beans http://www.springframework.org/schema/beans/spring-beans.xsd http://www.springframework.org/schema/context http://www.springframework.org/schema/context/spring-context.xsd http://www.springframework.org/schema/mvc http://www.springframework.org/schema/mvc/spring-mvc.xsd http://www.springframework.org/schema/tx http://www.springframework.org/schema/tx/spring-tx.xsd">

    <bean id="springPhysicalNamingStrategy" class="cn.fuhai.model.naming.SpringPhysicalNamingStrategy">
    </bean>
    <bean id="implicitNamingStrategy" class="org.hibernate.boot.model.naming.ImplicitNamingStrategyJpaCompliantImpl">

    </bean>
    <tx:annotation-driven transaction-manager="transactionManager"/>
    <import resource="classpath:spring-hibernate-*.xml"/>
</beans>
```

spring-hibernate-mysql.xml配置文件

```xml
<?xml version="1.0" encoding="UTF-8"?>
<beans xmlns="http://www.springframework.org/schema/beans"
       xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
       xmlns:context="http://www.springframework.org/schema/context"
       xmlns:mvc="http://www.springframework.org/schema/mvc" xmlns:tx="http://www.springframework.org/schema/tx"
       xsi:schemaLocation="http://www.springframework.org/schema/beans http://www.springframework.org/schema/beans/spring-beans.xsd http://www.springframework.org/schema/context http://www.springframework.org/schema/context/spring-context.xsd http://www.springframework.org/schema/mvc http://www.springframework.org/schema/mvc/spring-mvc.xsd http://www.springframework.org/schema/tx http://www.springframework.org/schema/tx/spring-tx.xsd">
    <!--连接池配置-->
    <!--sessionFactory配置-->
    <bean id="sessionFactory" class="org.springframework.orm.hibernate5.LocalSessionFactoryBean">
        <property name="dataSource" ref="mysqlDataSource"></property>
        <property name="packagesToScan">
            <list>
                <value>cn.fuhai.entity</value>
            </list>
        </property>
        <property name="physicalNamingStrategy" ref="springPhysicalNamingStrategy">
        </property>
        <!--通过config文件进行配置-->
        <!--<property name="configLocation" value=""></property>-->
        <!--<property name="implicitNamingStrategy" ref="implicitNamingStrategy"></property>-->
        <property name="hibernateProperties">
            <props>
                <prop key="hibernate.hbm2ddl.auto">${jdbc.hibernate.hbm2ddl.auto}</prop>
                <prop key="hibernate.dialect">${jdbc.hibernate.dialect}</prop>
                <prop key="hibernate.show_sql">${jdbc.hibernate.show_sql}</prop>
            </props>
        </property>
    </bean>

    <bean id="transactionManager" class="org.springframework.orm.hibernate5.HibernateTransactionManager">
        <property name="sessionFactory" ref="sessionFactory"/>
    </bean>
</beans>
```
spring-hibernate-postgresql.xml配置文件
```xml
<?xml version="1.0" encoding="UTF-8"?>
<beans xmlns="http://www.springframework.org/schema/beans"
       xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
       xmlns:context="http://www.springframework.org/schema/context"
       xmlns:mvc="http://www.springframework.org/schema/mvc" xmlns:tx="http://www.springframework.org/schema/tx"
       xsi:schemaLocation="http://www.springframework.org/schema/beans http://www.springframework.org/schema/beans/spring-beans.xsd http://www.springframework.org/schema/context http://www.springframework.org/schema/context/spring-context.xsd http://www.springframework.org/schema/mvc http://www.springframework.org/schema/mvc/spring-mvc.xsd http://www.springframework.org/schema/tx http://www.springframework.org/schema/tx/spring-tx.xsd">

    <!--连接池配置-->
    <!--sessionFactory配置-->
    <bean id="pgSessionFactory" class="org.springframework.orm.hibernate5.LocalSessionFactoryBean">
        <property name="dataSource" ref="pgDataSource"></property>
        <property name="packagesToScan">
            <list>
                <value>cn.fuhai.entity</value>
            </list>
        </property>
        <property name="physicalNamingStrategy" ref="springPhysicalNamingStrategy">
        </property>
        <!--通过config文件进行配置-->
        <!--<property name="configLocation" value=""></property>-->
        <!--<property name="implicitNamingStrategy" ref="implicitNamingStrategy"></property>-->
        <property name="hibernateProperties">
            <props>
                <prop key="hibernate.hbm2ddl.auto">${pg.jdbc.hibernate.hbm2ddl.auto}</prop>
                <prop key="hibernate.dialect">${pg.jdbc.hibernate.dialect}</prop>
                <prop key="hibernate.show_sql">${pg.jdbc.hibernate.show_sql}</prop>
            </props>
        </property>
    </bean>

    <bean id="pgTransactionManager" class="org.springframework.orm.hibernate5.HibernateTransactionManager">
        <property name="sessionFactory" ref="pgSessionFactory"/>
    </bean>
</beans>
```


这里`packagesToScan`包最好配置两个包，分别放mysql和postgresql的实体类，否则会两个库都创建全部得表，

由于配置了两个sessionFactory，所以在dao中使用sessionFactory时需要指定使用的哪一个

```java
@Repository
public class UserDao  {
    @Resource(name = "pgSessionFactory")
    /*@Resource*/
    SessionFactory sessionFactory;
}
```
spring-hibernate.xml配置文件中`<tx:annotation-driven transaction-manager="transactionManager"/>`是指定默认使用的TransactionManager为`transactionManager`，如果使用postgresql，需要使用postgresql配置文件中配置的`pgTransactionManager`,所以需要在``注解中指定使用的TransactionManager

```java
@Service
@Transactional(rollbackFor=Exception.class,transactionManager = "pgTransactionManager")
public class UserService {
}
```

## mybatis

>依赖

```xml
<dependency>
  <groupId>org.mybatis</groupId>
  <artifactId>mybatis</artifactId>
  <version>3.4.6</version>
</dependency>
<dependency>
  <groupId>org.mybatis</groupId>
  <artifactId>mybatis-spring</artifactId>
  <version>2.0.3</version>
</dependency>

```

>编写dao对象`UserMapper`

```java
package cn.fuhai.mapping;

import cn.fuhai.entity.User;

import java.util.List;

public interface UserMapper{
    List<User> getAllUser();
}
```

只需要编写接口类型就行了，mybatis会根据mapper映射文件来创建对象。

>配置

mybatis 配置文件mybatis-config.xml

```xml
<?xml version="1.0" encoding="UTF-8" ?>
<!DOCTYPE configuration
        PUBLIC "-//mybatis.org//DTD Config 3.0//EN"
        "http://mybatis.org/dtd/mybatis-3-config.dtd">
<configuration>

    <!-- mybatis全局设置 -->
    <settings>
        <!--使用数据库自增id-->
        <setting name="useGeneratedKeys" value="true" />
        <setting name="useColumnLabel" value="true" />
        <!-- 开启驼峰命名规范-->
        <setting name="mapUnderscoreToCamelCase" value="true" />
    </settings>

</configuration>
```

mybatis的spring 配置文件spring.mybatis.xml

```xml
<?xml version="1.0" encoding="UTF-8"?>
<beans xmlns="http://www.springframework.org/schema/beans"
       xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:p="http://www.springframework.org/schema/p"
       xmlns:context="http://www.springframework.org/schema/context"
       xmlns:mvc="http://www.springframework.org/schema/mvc"
       xsi:schemaLocation="http://www.springframework.org/schema/beans
	http://www.springframework.org/schema/beans/spring-beans-3.1.xsd
	http://www.springframework.org/schema/context
	http://www.springframework.org/schema/context/spring-context-3.1.xsd
	http://www.springframework.org/schema/mvc
	http://www.springframework.org/schema/mvc/spring-mvc-4.0.xsd">
    <bean id="sqlSessionFactory" class="org.mybatis.spring.SqlSessionFactoryBean">
        <property name="dataSource" ref="pgDataSource" />
        <property name="configLocation" value="classpath:mybatis-config.xml"/>
        <property name="typeAliasesPackage" value="cn.fuhai.entity"/>
        <property name="mapperLocations" value="classpath:mapper/*.xml"/>
        <!-- 自动扫描mapping.xml文件 -->
        <!--<property name="mapperLocations" value="classpath:cn/fuhai/mapping/*.xml"></property>-->
    </bean>
    <bean class="org.mybatis.spring.mapper.MapperScannerConfigurer"
          p:basePackage="cn.fuhai.mapping"
          p:sqlSessionFactoryBeanName="sqlSessionFactory"/>

</beans>
```
mapperLocations是指定mapper映射文件的位置，在该位置下创建mapper文件。MapperScannerConfigurer中，p:basePackage是指定dao接口的位置，扫描dao的时候会根据映射文件的配置来创建dao对象。

接下来先配置UserMapper接口对应的映射文件UserMapper.xml

```xml
<?xml version="1.0" encoding="UTF-8" ?>
<!DOCTYPE mapper
        PUBLIC "-//mybatis.org//DTD Mapper 3.0//EN"
        "http://mybatis.org/dtd/mybatis-3-mapper.dtd">
<!-- Mapper.xml 必须要包含上面的DTD头部（DOCTYPE）  -->

<!-- namespace为命名空间，应该是mapper接口的全称-->
<mapper namespace="cn.fuhai.mapping.UserMapper">
    <select id="getAllUser" resultType="cn.fuhai.entity.User">
        SELECT * FROM test1user
    </select>
</mapper>
```

这样就基本配置完成了

>使用
MapperScannerConfigurer 已经把dao对象注入到容器中了，使用 @Resource 就能获取到。
```java
@Service
@Transactional(rollbackFor=Exception.class)
public class UserService {
    @Resource
    UserMapper mapper;
    public List<User> GetUsers(){
        return mapper.getAllUser();
    }
}

```

## mybatis plus

比原生的mybatis好用点，封装了一些常用的方法，在进行简单查询、插入和更新时不用写mapper xml，也可以通过写mapper xml来进行查询。

>依赖

```xml
<dependency>
  <groupId>com.baomidou</groupId>
  <artifactId>mybatis-plus</artifactId>
  <version>3.2.0</version>
</dependency>
<dependency>
  <groupId>com.baomidou</groupId>
  <artifactId>mybatis-plus-generator</artifactId>
  <version>3.2.0</version>
</dependency>
```

>编写dao文件

```java
package cn.fuhai.mapping;

import cn.fuhai.entity.User;
import com.baomidou.mybatisplus.core.mapper.BaseMapper;

import java.util.List;

public interface UserMapper extends BaseMapper<User> {
    List<User> getAllUser();
}
```
BaseMapper中会定义有一些常用的查询、插入和更新的方法。
>配置

配置和原来的没有区别，只需要修改SqlSessionFactoryBean的实现类

mybatis:`org.mybatis.spring.SqlSessionFactoryBean`

mybatis plus:`com.baomidou.mybatisplus.extension.spring.MybatisSqlSessionFactoryBean`

```xml
<bean id="sqlSessionFactory" class="com.baomidou.mybatisplus.extension.spring.MybatisSqlSessionFactoryBean">
    <property name="dataSource" ref="pgDataSource" />
    <property name="configLocation" value="classpath:mybatis-config.xml"/>
    <property name="typeAliasesPackage" value="cn.fuhai.entity"/>
    <property name="mapperLocations" value="classpath:mapper/UserMapper.xml"/>
</bean>
```

映射文件UserMapper.xml配置

```xml
<?xml version="1.0" encoding="UTF-8" ?>
<!DOCTYPE mapper
        PUBLIC "-//mybatis.org//DTD Mapper 3.0//EN"
        "http://mybatis.org/dtd/mybatis-3-mapper.dtd">
<!-- Mapper.xml 必须要包含上面的DTD头部（DOCTYPE）  -->

<!-- namespace为命名空间，应该是mapper接口的全称-->
<mapper namespace="cn.fuhai.mapping.UserMapper">
    <select id="getAllUser" resultType="cn.fuhai.entity.User">
        SELECT * FROM test1user
    </select>
</mapper>
```

>使用

getAllUser 是通过映射文件来配置的，insert方法是其父类BaseMapper的方法,这样一些简单的查询、插入、更新和删除操作都可以不必配置映射文件，一些复杂操作可以配置映射文件来实现。

```java
@Service
@Transactional(rollbackFor=Exception.class)
public class UserService {
    @Resource
    UserMapper mapper;
    public List<User> GetUsers(){
        return mapper.getAllUser();
    }
    public void AddUsers(List<User> users) throws Exception {
        for (int i =0;i<users.size();i++){
            User user = users.get(i);
            mapper.insert(user);
        }
    }
}
```

## mongodb

>添加依赖

```
<dependency>
  <groupId>org.mongodb</groupId>
  <artifactId>mongo-java-driver</artifactId>
  <version>3.12.0</version>
</dependency>
<dependency>
  <groupId>org.springframework.data</groupId>
  <artifactId>spring-data-mongodb</artifactId>
  <version>2.1.1.RELEASE</version>
</dependency>
```

`spring-data-mongodb`封装了一些操作在`MongoTemplate`，否则要自己写`CodecRegistry`


>配置

写配置文件mongod.properties
```conf
mongo.host=localhost
mongo.port=27017
mongo.username=xxxx
mongo.password=xxxx
mongo.db.literature=literature
```

编写mongodb bean配置文件spring-mongo.xml
```xml
<?xml version="1.0" encoding="UTF-8"?>
<beans xmlns="http://www.springframework.org/schema/beans"
       xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
       xmlns:mongo="http://www.springframework.org/schema/data/mongo"
       xsi:schemaLocation="http://www.springframework.org/schema/beans
       http://www.springframework.org/schema/beans/spring-beans.xsd
       http://www.springframework.org/schema/context
       http://www.springframework.org/schema/context/spring-context.xsd
       http://www.springframework.org/schema/data/mongo
       http://www.springframework.org/schema/data/mongo/spring-mongo.xsd">
    <mongo:mongo-client host="${mongo.host}" port="${mongo.port}" id="mongoClient" credentials="${mongo.username}:${mongo.password}@admin">
        <!--<mongo:client-options
                connections-per-host="${mongo.connectionsPerHost}"
                write-concern="SAFE"
                threads-allowed-to-block-for-connection-multiplier="${mongo.threadsAllowedToBlockForConnectionMultiplier}"
                connect-timeout="${mongo.connectTimeout}"
                max-wait-time="${mongo.maxWaitTime}"
                socket-keep-alive="${mongo.socketKeepAlive}"
                socket-timeout="${mongo.socketTimeout}" />-->
    </mongo:mongo-client>
    <mongo:db-factory dbname="${mongo.db.literature}" id="dbFactory" mongo-ref="mongoClient" />
    <bean id="literature" class="org.springframework.data.mongodb.core.MongoTemplate">
        <constructor-arg name="mongoDbFactory" ref="dbFactory"/>
    </bean>
</beans>
```

`credentials`后面的admin是指用于认证的数据库，一般在mongodb中创建用户都是在admin库中创建的，所以要验证的时候也是要在该库中验证。

applicationContext.xml中导入mongo的bean配置。
```xml
<import resource="classpath:spring.mongo.xml"/>
```
>使用

```java
@Service
public class AuthorService {
    @Resource(name = "literature")
    MongoTemplate literature;
    public List<Author> GetAllAuthors(){
        Query query = new Query(Criteria.where("dynasty").is("宋朝"));
        query.limit(10);
        List<Author> users = literature.find(query,Author.class,"authors");
        return users;
    }
}
```
通过`MongoTemplate`来操作mongodb 

## 静态文件

在webapp目录下创建一个静态文件夹static，把html等静态文件放在里面。

然后在`dispatcher-servlet.xml`配置文件中配置静态资源

```xml
<mvc:resources mapping="/static/**" location="static/" />
```
`/static/**`路由会直接从location中查找，现在location指向了当前的`static`目录下

`http://localhost/static/index.html`会指向`static`目录下的`index.html`


也可以把静态文件夹创建到`resource`目录下，这样的话要修改一样静态资源配置如下。
```xml
<mvc:resources mapping="/static/**" location="classpath:static/" />
```




## 一些异常

>javax.net.ssl.SSLException: closing inbound before receiving peer's close_notify

数据库url后面加上`useSSL=false`

```conf
jdbc.url=jdbc:mysql://localhost:3306/selffate?serverTimezone=GMT%2B8&useSSL=false
```

>Cannot resolve table 'user'

这个是配置Entity使用@Table注解时出现的异常。尚且不知原因。不影响使用



# idea问题

## 调试

点击`Run->Debug 'Tomcat'` 或者按 `F9`快捷键

`Run->Run 'Tomcat'` 是直接运行项目，不会进入断点调试

## 断点只能进入一次

这个是因为点击`Run To Cursor`来跳到下一个断点导致的，跳断点应该使用快捷键`F9`


## 绿色波浪线警告

有时候有些个人项目的包名不是正常单词，IDE会检查出拼写问题，在引用包时也不会智能提示，这时候可以`save to project-level dictionary`,把该单词添加到字典中

# rest web api

@RestController

# idea 使用 git

## JetBrains.gitignore

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



# 异步

# 拦截器

# Hibernate

# 单元测试

# 打包部署

# docker部署

# yml使用

# spring cloud

# swagger

# 权限控制问题

# 多项目管理

pom.xml `<modules></modules>`

# spring boot