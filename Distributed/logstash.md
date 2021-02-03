# 一个简单示例

```yml
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

# logstash

>input、filter、output

    日志采集器的三个部分，日志入口、日志过滤器、日志出口

>日志时间替换@timestamp
```yml
filter {
    date {
        #time是匹配的日志时间的名称
        #使用日志中的time替换@timestamp
        match => ["time","yyyy-MM-dd HH:mm:ss.SSSS"]
        target => "@timestamp"
    }
}
```

>自定义pattern

    （1）匿名SYNTAX

    将%{SYNTAX:SEMANTIC} 写为(?<SEMANTIC>regexp)

    （2）命名SYNTAX

    在dir下创建一个文件，文件名随意

    将dir加入grok路径： patterns_dir => "./dir"

    将想要增加的SYNTAX写入： SYNTAX_NAME regexp

    使用方法和使用默认SYNTAX相同：%{SYNTAX_NAME:SEMANTIC}

```yml
filter {
    grok {
        #自定义pattern的路径
        patterns_dir => "custompattern"
    }
}
```

>分支结构（匹配异常处理）
```yml
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
            password => "******"
        }
    }
    #如果level存在且level==ERROR，则
    else if [level] and [level]=="ERROR"{
        elasticsearch {
            hosts => ["http://localhost:9200"]
            index => "regularerror_%{+YYYY.MM.dd}"
            user => "elastic"
            password => "******"
        }
    }
}
```
>数据格式转换、字段增删

```yml
filter {
    mutate{
        #把mpid转换为integer格式，否则存储到es中会是string格式
        convert => {"mpid"=>"integer"}
        #获取远程ip地址
        add_field => { "remote_ip" => "%{[@metadata][ip_address]}" }
        #time字段已经替换了@timestamp了，所以这里已经不需要time字段了
        remove_field => ["time"]
    }
}
```
>multiline

    多行日志，多用于异常日志

>设置动态mapping



>领域专用语言（domain specific language / DSL） 



# 插件安装

>安装logstash-output-mongodb

logstash bin目录下执行以下命令
```sh
logstash-plugin install logstash-output-mongodb

# 指定版本安装
logstash-plugin install --version=3.1.5 logstash-output-mongodb
```
这个需要java环境下执行。

mongodb-3.6.4 需要安装logstash-output-mongodb-3.1.5，安装logstash-output-mongodb-3.1.6出现异常
