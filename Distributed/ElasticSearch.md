# 使用问题
> 无法远程访问9200端口

修改配置文件 config/elasticsearch.yml   
network.host: 0.0.0.0

>x-pack

>id

`PUT /{index}/{type}/{id}`  
es在进行put操作的时候可以指定一个id，很多驱动（如 `ElasticEearch.Net`）会默认把类中的id字段作为这个id进行put操作，如果相同id进行put操作，后面的会覆盖前面的document，然后version加1。  
如果没有指定id，es会自动生成一个id

>[index](https://www.elastic.co/guide/en/elasticsearch/reference/2.4/mapping.html)

>[mapping type](https://www.elastic.co/guide/en/elasticsearch/reference/6.0/removal-of-types.html)

[连接]
Elasticsearch 6.0.0以及之后的版本中，每个index只有一个mapping type  
之前的版本中，每个index可以有多个mapping type   
mapping type将会在Elasticsearch 7.0.0.中移除



