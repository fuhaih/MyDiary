>spring mvc

spring mvc程序是没有程序入口的，需要并没有内置容器管理，所以部署时候需要打包成war包，然后单独配置程序容器tomcat来部署。

>spring boot

spring boot程序是含有程序入口main函数的，他是使用tomcat或者jetty当做他内置的容器，所以不需要再单独配置tomcat。
不需要打成war包， 直接打成jar包然后java -jar 就可以运行了。