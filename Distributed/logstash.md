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
        # 这里的(?<msg>[^|]+)也是自定义pattern，是用匿名pattern来写的
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

>日志时间替换@timestamp

>自定义pattern

>分支结构（匹配异常处理）

>数据格式转换

>领域专用语言（domain specific language / DSL） 

