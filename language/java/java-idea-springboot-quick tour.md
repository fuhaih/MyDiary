# spring boot 和spring mvc

用maven创建webapp项目的时候，要配置spring mvc还需要很多繁琐重复的操作，spring boot会自动进行配置，包括常用的日志、连接池、路由等，就可以开箱即用。

# spring boot创建web程序

这个使用`spring-boot-starter-web` 依赖

# spring boot创建console程序

console程序的主要依赖是`spring-boot-starter`


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

>创建项目方式2(Spring Initializr 推荐)

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


# Spring boot 使用

>数据库配置

把配置文件修改为yaml文件 `application.yml`

添加数据源配置

```yaml
spring:
  datasource:
    driver-class-name: dm.jdbc.driver.DmDriver
    url: jdbc:dm://192.168.68.104:5236/TTBEMS_HONGKOU
    username: SYSDBA
    password: xxxxxxxx
```

添加mybatis项目引用

```xml
<dependency>
    <groupId>com.baomidou</groupId>
    <artifactId>mybatis-plus</artifactId>
    <version>3.2.0</version>
</dependency>
<dependency>
    <groupId>com.baomidou</groupId>
    <artifactId>mybatis-plus-boot-starter</artifactId>
    <version>3.2.0</version>
</dependency>
```
`mybatis-plus-boot-starter`是mybatis的spring boot启动相关的库，添加这个库后，使用spring boot时可以减少mybatis的配置

在 `application.yml`中添加上mybatis的配置

```yaml
mybatis-plus:
  mapper-locations: classpath:mapper/*.xml
  type-aliases-package: com.ttbems.xml2db.entity
  configuration:
    map-underscore-to-camel-case: true
```

创建Mapper时需要添加`@Mapper`注解

```java
@Mapper
public interface BuildDictMapper extends BaseMapper<BuildDict> {

}
```


>多数据源数据库配置

修改配置文件`application.yml`

```yaml
spring:
  datasource:
    landmark:
      driver-class-name: org.sqlite.JDBC
      jdbc-url: jdbc:sqlite://D:\sqlite-tools\security.db
    log:
      driver-class-name: dm.jdbc.driver.DmDriver
      jdbc-url: jdbc:dm://192.168.68.104:5236/TTBEMS_LANDMARK
      username: SYSDBA
      password: xxxx
```

多数据源配置和单数据源配置有所不同，如上修改配置文件

添加两个配置类

数据源1配置
```java
@Configuration
@MapperScan(basePackages = "com.ttbems.landmarkserver.mapping.landmark",sqlSessionFactoryRef = "landmarkSqlSessionFactory")
public class LandmarkDataSourceConfig {
    private final String MAPPER_LOCATION="classpath:mapper/landmark/*.xml";
    private final String DOMAIN_PACKAGE ="com.ttbems.landmarkserver.entity.landmark";

    @Bean("landmarkSource")
    @ConfigurationProperties(prefix = "spring.datasource.landmark")
    public DataSource dataSource(){
        return DataSourceBuilder.create().build();
    }

    @Bean(name = "landmarkSqlSessionFactory")
    @Primary
    public SqlSessionFactory sqlSessionFactory(@Qualifier("landmarkSource") DataSource ds) throws Exception{
        MybatisSqlSessionFactoryBean bean = new MybatisSqlSessionFactoryBean ();
        bean.setDataSource(ds);
        bean.setMapperLocations(new PathMatchingResourcePatternResolver()
                .getResources(MAPPER_LOCATION));
        bean.setTypeAliasesPackage(DOMAIN_PACKAGE);
        return bean.getObject();
    }

    @Bean(name = "landmarkSqlSessionTemplate")
    @Primary
    public SqlSessionTemplate sqlSessionTemplate(@Qualifier("landmarkSqlSessionFactory") SqlSessionFactory sessionFactory){
        return  new SqlSessionTemplate(sessionFactory);
    }
    @Bean(name = "landmarkTransactionManager")
    @Primary
    public DataSourceTransactionManager transactionManager(@Qualifier("landmarkSource") DataSource ds){
        return new DataSourceTransactionManager(ds);
    }
}
```

`MapperScan`指定了Mapper类包名，`MAPPER_LOCATION`是Mapper的xml文件路径，`DOMAIN_PACKAGE`是实体类包名

数据源2配置类

```java
@Configuration
@MapperScan(basePackages = "com.ttbems.landmarkserver.mapping.logs",sqlSessionFactoryRef = "logSqlSessionFactory")
public class LogDataSourceConfig {
    private final String MAPPER_LOCATION="classpath:mapper/logs/*.xml";
    private final String DOMAIN_PACKAGE ="com.ttbems.landmarkserver.entity.logs";
    @Bean("logSource")
    @ConfigurationProperties(prefix = "spring.datasource.log")
    public DataSource dataSource(){
        return DataSourceBuilder.create().build();
    }

    @Bean(name = "logSqlSessionFactory")
    @Primary
    public SqlSessionFactory sqlSessionFactory(@Qualifier("logSource") DataSource ds) throws Exception{
        MybatisSqlSessionFactoryBean bean = new MybatisSqlSessionFactoryBean ();
        bean.setDataSource(ds);
        /*bean.setMapperLocations(new PathMatchingResourcePatternResolver()
                .getResources(MAPPER_LOCATION));
        bean.setTypeAliasesPackage(DOMAIN_PACKAGE);*/
        return bean.getObject();
    }

    @Bean(name = "logSqlSessionTemplate")
    @Primary
    public SqlSessionTemplate sqlSessionTemplate(@Qualifier("logSqlSessionFactory") SqlSessionFactory sessionFactory){
        return  new SqlSessionTemplate(sessionFactory);
    }
    @Bean(name = "logTransactionManager")
    @Primary
    public DataSourceTransactionManager transactionManager(@Qualifier("logSource") DataSource ds){
        return new DataSourceTransactionManager(ds);
    }
}
```

这样配置后

`com.ttbems.landmarkserver.mapping.landmark`包下的Mapper会使用配置文件中`spring.datasource.landmark`的配置

`com.ttbems.landmarkserver.mapping.logs`包下的Mapper会使用配置文件中`spring.datasource.log`的配置

>数据库连接池

Spring Boot 2.+的默认连接池是HikariCP，所以在使用时候不需要配置数据库连接池，也不需要添加相关引用，可以看运行日志下面Hikari在程序开启时已经启动

```log
2020-12-30 14:52:53,613 main INFO com.zaxxer.hikari.HikariDataSource HikariPool-1 - Starting...
2020-12-30 14:52:53,757 main INFO com.zaxxer.hikari.HikariDataSource HikariPool-1 - Start completed.
```

再获取DataSource实例，查看类型，是`HikariDataSource`类型


>打包发布

新建一个程序集配置文件`assembly.xml`

```xml
<assembly xmlns="http://maven.apache.org/ASSEMBLY/2.1.0"
          xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
          xsi:schemaLocation="http://maven.apache.org/ASSEMBLY/2.1.0 http://maven.apache.org/xsd/assembly-2.1.0.xsd">
    <id>package</id>
    <formats>
        <format>zip</format>
    </formats>
    <includeBaseDirectory>true</includeBaseDirectory>
    <fileSets>
        <fileSet>
            <directory>${basedir}/src/main/resources</directory>
            <includes>
                <include>*.yml</include>
                <include>*.properties</include>
            </includes>
            <filtered>true</filtered>
            <outputDirectory>${file.separator}config</outputDirectory>
        </fileSet>
        <fileSet>
            <directory>src/main/resources/runScript</directory>
            <outputDirectory>${file.separator}bin</outputDirectory>
        </fileSet>
        <fileSet>
            <directory>${project.build.directory}/lib</directory>
            <outputDirectory>${file.separator}lib</outputDirectory>
            <includes>
                <include>*.jar</include>
            </includes>
        </fileSet>
        <fileSet>
            <directory>${project.build.directory}</directory>
            <outputDirectory>${file.separator}</outputDirectory>
            <includes>
                <include>*.jar</include>
            </includes>
        </fileSet>
    </fileSets>
</assembly>
```

包含有四个`fileSet`，分别是指

把.yml和.properties配置文件移动到打包目录的config文件夹下。

把resources下的runScript文件夹下文件移动到打包目录的bin文件夹下。

把lib下文件移动到打包目录的lib文件夹下，也就是把依赖都挪动到打包目录的lib文件夹下

把程序生成的jar包移动到打包目录下


然后再修改pom.xml配置文件

```xml
<build>
    <plugins>
        <!--<plugin>
            <groupId>org.springframework.boot</groupId>
            <artifactId>spring-boot-maven-plugin</artifactId>
        </plugin>-->
        <plugin>
            <groupId>org.apache.maven.plugins</groupId>
            <artifactId>maven-jar-plugin</artifactId>
            <version>3.0.2</version>
            <configuration>
                <archive>
                    <manifest>
                        <addClasspath>true</addClasspath>
                        <classpathPrefix>lib/</classpathPrefix>
                        <mainClass>com.ttbems.landmarkserver.LandmarkserverApplication</mainClass>
                    </manifest>
                </archive>
            </configuration>
        </plugin>
        <plugin>
            <groupId>org.apache.maven.plugins</groupId>
            <artifactId>maven-dependency-plugin</artifactId>
            <version>3.0.2</version>
            <executions>
                <execution>
                    <id>copy-lib</id>
                    <phase>prepare-package</phase>
                    <goals>
                        <goal>copy-dependencies</goal>
                    </goals>
                    <configuration>
                        <outputDirectory>${project.build.directory}/lib</outputDirectory>
                        <overWriteReleases>false</overWriteReleases>
                        <overWriteSnapshots>false</overWriteSnapshots>
                        <overWriteIfNewer>true</overWriteIfNewer>
                        <includeScope>compile</includeScope>
                    </configuration>
                </execution>
            </executions>
        </plugin>
        <plugin>
            <groupId>org.apache.maven.plugins</groupId>
            <artifactId>maven-assembly-plugin</artifactId>
            <version>3.3.0</version>
            <configuration>
                <appendAssemblyId>false</appendAssemblyId>
                <descriptors>
                    <descriptor>src/main/resources/assembly.xml</descriptor>
                </descriptors>
            </configuration>
            <executions>
                <execution>
                    <id>make-assembly</id>
                    <phase>package</phase>
                    <goals>
                        <goal>single</goal>
                    </goals>
                </execution>
            </executions>
        </plugin>
    </plugins>
<!--        <resources>
        <resource>
            <directory>src/main/resources</directory>
            <excludes>
                <exclude>**/*.yml</exclude>
                <exclude>**/*.properties</exclude>
            </excludes>
        </resource>
    </resources>-->
</build>
```

maven->Lifecycle->package 

右键->build

会直接把项目编译好，并打包成zip文件，文件下有项目jar包、lib文件夹、bin文件夹、config文件夹。