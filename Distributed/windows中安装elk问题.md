
# [安装elasticsearch](https://www.elastic.co/guide/en/elasticsearch/reference/current/windows.html)

使用msi文件安装，在选择license时选择Trial，然后填写几个用户的密码   
分别有elastic、kibana和logstash system

1、索引ttl

给索引添加ttl，设置过期时间
# 安装logstash

1、下载解压[logstash](https://www.elastic.co/downloads/logstash)

2、进入bin目录中，新建一个配置文件 logstash_default.conf

```conf
input {
  tcp {
    port => 5044
  }
}
filter {
    grok {
        #自定义pattern的路径
        patterns_dir => "custompattern"
        #多个匹配模式
        match => {"message" => "%{TIMESTAMP_ISO8601:time}\|%{LOGLEVEL:level}\|%{WORD:cycle}\|%{WORD:build}\|%{WORD:meter}\|%{INT:mpid}\|%{GREEDYDATA:state}"}
        # 这里的(?<msg>[^|]+)也是自定义pattern，是用匿名pattern来写的，THEREST是在custompattern中自定义的SYNTAX
	    match => { 
            "message" => "%{TIMESTAMP_ISO8601:time}\|%{LOGLEVEL:level}\|(?<msg>[^|]+)\|%{THEREST:ex}"
        }
	    match => { 
            "message" => "%{TIMESTAMP_ISO8601:time}\|%{LOGLEVEL:level}\|(?<msg>[^|]+)"
        }  
    }
    date {
        #使用日志中的time替换@timestamp
        match => ["time","yyyy-MM-dd HH:mm:ss.SSSS"]
        target => "@timestamp"
    }
    mutate{
        #把mpid转换为integer格式，否则存储到es中会是string格式
        convert => {"mpid"=>"integer"}
        #获取远程ip地址
        add_field => { "remote_ip" => "%{[@metadata][ip_address]}" }
        #time字段已经替换了@timestamp了，所以这里已经不需要time字段了
        remove_field => ["time"]
    }
}
output {
    # 解析异常时，从控制台中打印出来
    if [tags] and "_grokparsefailure" in [tags]{
        stdout{}
    }
    # 如果 level存在且level==INFO且build存在，则
    else if [level] and [level]=="INFO" and [build]{
        elasticsearch {
            hosts => ["http://localhost:9200"]
            index => "regularprocess_%{+YYYY.MM.dd}"
            user => "elastic"
            password => "tt@52415188"
        }
    }
    #如果level存在且level==ERROR，则
    else if [level] and [level]=="ERROR"{
        elasticsearch {
            hosts => ["http://localhost:9200"]
            index => "regularerror_%{+YYYY.MM.dd}"
            user => "elastic"
            password => "tt@52415188"
        }
    }
}


```
在配置文件中注意`#`号，该符号是注释符号

3、进入conf目录中修改logstash.yml配置文件

```yml
http.host: "0.0.0.0"
http.port: 9600-9700
```

4、在bin目录，新文件文件  run_default.bat

```sh
logstash -f logstash_default.conf
```

5、双击运行run_default.bat

6、浏览器访问:http://localhost:9600/

7、自定义grok-pattern

（1）匿名SYNTAX

将%{SYNTAX:SEMANTIC} 写为(?\<SEMANTIC>regexp)

（2）命名SYNTAX

在dir下创建一个文件，文件名随意

将dir加入grok路径： patterns_dir => "./dir"

将想要增加的SYNTAX写入： SYNTAX_NAME regexp

使用方法和使用默认SYNTAX相同：%{SYNTAX_NAME:SEMANTIC}

8、es中index健康状态为yellow的问题

主要是logstash在创建index的时候是设置index的number_of_replicas=1的，由于es没有创建集群，所以健康状态为yellow，可以两种方式进行修改
（1）创建集群
（2）设置动态mapping 到es 配置

```conf
output {
  elasticsearch{
    hosts => ["192.168.103.3:9200""]
    index => "appl-%{+YYYY.MM.dd}"
    template => "/home/bigdata/apps/logstash/config/appl_mapping.json"
    template_name => "appl-%{+YYYY.MM.dd}"
    template_overwrite => true
    manage_template => true
    user => "elastic"
    password => "******"
  }
}
```
然后在appl_mapping.json中进行number_of_replicas等信息的配置

[坑](https://blog.csdn.net/u012516166/article/details/75106184)



9、multiline

多行日志，多用于异常日志

# 安装kibana

1、下载解压[kibana](https://www.elastic.co/downloads/kibana)

2、conf目录下进行配置

```yml
server.port: 5601
server.host: "0.0.0.0"
elasticsearch.url: "http://localhost:9200"
#这个是默认的配置，把index更改为已存在的索引的时候会异常
kibana.index: ".kibana"
elasticsearch.username: "elastic"
elasticsearch.password: "******"
```
3、进入解压目录，运行程序

```cmd
cd C:\kibana-6.5.4-windows-x86_64
.\bin\kibana
```

4、打开控制台

http://localhost:5601

5、[security settings in Kibana](https://www.elastic.co/guide/en/kibana/current/security-settings-kb.html#security-settings-kb)

用户名密码为：elastic ******
# 其他
>服务问题

elasticsearch使用msi文件安装，会直接安装成服务，所以不用做额外操作  
把kibana、logstash安装成服务需要用到nssm