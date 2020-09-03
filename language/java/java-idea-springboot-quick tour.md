# spring boot 和spring mvc

用maven创建webapp项目的时候，要配置spring mvc还需要很多繁琐重复的操作，spring boot会自动进行配置，包括常用的日志、连接池、路由等，就可以开箱即用。

# spring boot创建web程序

这个使用`spring-boot-starter-web` 依赖

# spring boot创建console程序

## 入门

>创建项目

file->new->project,使用maven，选择`maven-archetype-quickstart`项目模板，配置maven路径和setting

>调试

配置调试模板，选择Application，配置好调试运行的JDK版本

当版本不对应是会出现异常`java错误,不支持发行版本5`

有三个地方需要注意JDK版本

* Project Structure

* Debug

* pom.xml

最有可能是pom.xml导致的JDK版本不一致，所以需要重新配置

```xml
  <properties>
    <project.build.sourceEncoding>UTF-8</project.build.sourceEncoding>
    <maven.compiler.source>13</maven.compiler.source>
    <maven.compiler.target>13</maven.compiler.target>
  </properties>
```

>创建项目方式2(Spring Initializr)

file->new ->project ，使用Spring Initializr
用这种方式可以直接选择依赖包，手动配置依赖。

这里创建一个最简单的spring boot项目，后续不用选择包，最终会默认引用`spring-boot-starter`依赖

主程序`DemoApplication`

```java
@SpringBootApplication
public class DemoApplication {

    public static void main(String[] args) {
        SpringApplication.run(DemoApplication.class, args);
    }     
}
```
这里是程序启动入口

添加服务：实现`CommandLineRunner`接口

```java
@Component
@Order(1)
public class TestService1 implements CommandLineRunner {
    @Override
    public void run(String... args) throws Exception {
        System.out.println(">>>>>>>>>>>>>>>服务启动第一个开始执行的任务，执行加载数据等操作<<<<<<<<<<<<<");
    }
}

```
在程序启动之后会执行`TestService1`的run方法

可以添加多个服务，多个服务根据`Order`注解来决定执行顺序。

同时也可以使用`ApplicationRunner`来实现，也是复写run方法，这两种方式的区别是参数不一样。

>使用spring boot

console程序的话使用`spring-boot-starter`依赖就行了。

