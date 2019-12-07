## IHttpActionResult

>HttpResponseMessage

这个不属于IHttpActionResult的实现类，但是也可以作为webapi方法的返回值。

>void 

也就是没有返回值，这时候会给前端返回状态码204   

204意思等同于请求执行成功，但是没有数据，浏览器不用刷新页面.也不用导向新的页面。如何理解这段话呢。还是通过例子来说明吧，假设页面上有个form，提交的url为http-204.htm，提交form，正常情况下，页面会跳转到http-204.htm，但是如果http-204.htm的相应的状态码是204，此时页面就不会发生转跳，还是停留在当前页面。另外对于a标签，如果链接的页面响应码为204，页面也不会发生跳转。
>JsonResult

方法：`Json<T>()`

webapi中使用的是`Newtonsoft.Json`库，所以需要自定义时可以参考`Newtonsoft.Json`官方文档。

这里写上两个比较常用的配置。

* MongoDB ObjectId处理。

ObjectId类型在进行序列号和反序列化的时候不太好处理，这里可以通过自定义`JsonConverter`来进行操作

```csharp
//定义类
public class ObjectIdConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(ObjectId);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        string value = reader.Value.ToString();
        return new ObjectId(value);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString());
    }
}

//使用

public class QuestionView
{
    /// <summary>
    /// 题目编号
    /// </summary>
    [JsonConverter(typeof(ObjectIdConverter))]
    [DisplayName("编号")]
    public ObjectId _id { get; set; }
}
```

* 多态


```csharp
//定义类型
public class KnownTypesBinder : ISerializationBinder
{
    public Dictionary<string, RuntimeTypeHandle> KnownTypes { get; set; } = new Dictionary<string, RuntimeTypeHandle>();

    public Type BindToType(string assemblyName, string typeName)
    {
        RuntimeTypeHandle handle;
        if (KnownTypes.TryGetValue(typeName, out handle))
        {
            return Type.GetTypeFromHandle(handle);
        }
        throw new Exception(string.Format("没有找到类型 {0} ,{1}", assemblyName, typeName));
    }

    public void BindToName(Type serializedType, out string assemblyName, out string typeName)
    {
        assemblyName = null;
        typeName = serializedType.Name;
    }

    public void SetTypeOf<T>()
    {
        Type type = typeof(T);
        var types = (JsonKnownTypesAttribute)type.GetCustomAttributes(typeof(JsonKnownTypesAttribute),false).FirstOrDefault();
        if (types != null)
        {
            foreach(var item in types.KnownTypes)
            {
                KnownTypes.Add(item.Name, item.TypeHandle);
            }
        }
    }

    public void SetTypeOf(Type type)
    {
        var types = (JsonKnownTypesAttribute)type.GetCustomAttributes(typeof(JsonKnownTypesAttribute), false).FirstOrDefault();
        if (types != null)
        {
            foreach (var item in types.KnownTypes)
            {
                KnownTypes.Add(item.Name, item.TypeHandle);
            }
        }
    }
}

public class JsonKnownTypesAttribute : Attribute
{
    public Type[] KnownTypes { get; set; }
    public JsonKnownTypesAttribute(params Type[] knownTypes)
    {
        this.KnownTypes = knownTypes;
    }
}


[JsonKnownTypes(typeof(CorpusAudio), typeof(CorpusImage), typeof(CorpusText), typeof(CorpusVideo))]
[BsonKnownTypes(typeof(CorpusAudio),typeof(CorpusImage),typeof(CorpusText),typeof(CorpusVideo))]
/// <summary>
/// 语料基类
/// </summary>
public class CorpusBase
{
    /// <summary>
    /// id
    /// </summary>
    [BsonId]
    [JsonConverter(typeof(ObjectIdConverter))]
    public ObjectId _id { get; set; }

    /// <summary>
    /// 语料名称
    /// </summary>
    public string Name { get; set; }
    [JsonConverter(typeof(ObjectIdConverter))]
    public ObjectId SourceID { get; set; }
}

// 序列化
KnownTypesBinder knownTypesBinder = new KnownTypesBinder();
//knownTypesBinder.SetTypeOf<ExamQuestions>(); 反序列化要用
JsonSerializerSettings setting = new JsonSerializerSettings();
setting.SerializationBinder = knownTypesBinder;
setting.TypeNameHandling = TypeNameHandling.Auto;//表示当实际类型和类型描述不一致时，就添加$type字段。
setting.ContractResolver = new CamelCasePropertyNamesContractResolver();
return Json(content, setting);

//反序列化
string json = parameter.json;
KnownTypesBinder knownTypesBinder = new KnownTypesBinder();
knownTypesBinder.SetTypeOf<CorpusBase>(); //反序列化要用
JsonSerializerSettings setting = new JsonSerializerSettings();
setting.SerializationBinder = knownTypesBinder;
setting.TypeNameHandling = TypeNameHandling.Auto;
setting.ContractResolver = new CamelCasePropertyNamesContractResolver();
var corpus = JsonConvert.DeserializeObject<CorpusBase>(json,setting);

```


>BadRequestResult

方法：`BadRequest`

错误请求      
会返回400状态码
```csharp
[Route("BadRequestResult")]
public async Task<IHttpActionResult> GetBadRequestResult()
{
    return BadRequest("参数异常");
}
```



>InvalidModelStateResult

模型验证异常，返回状态码400

```csharp
[Route("InvalidModelStateResult")]
public async Task<IHttpActionResult> GetInvalidModelStateResult()
{
    ModelState.AddModelError("test", new Exception("test model error"));
    return BadRequest(ModelState);
}
```
返回信息
```json
{
  "Message": "请求无效。",
  "ModelState": {
    "test": [
      "test model error"
    ]
  }
}
```

>BadRequestErrorMessageResult

返回400状态码和Message信息
```csharp
[Route("BadRequestErrorMessageResult")]
public async Task<IHttpActionResult> GetBadRequestErrorMessageResult()
{
    return BadRequest("BadRequestErrorMessageResult");
}
```

>ConflictResult

返回冲突状态码409，这个也可以使用StatusCodeResult来替换

```csharp
[Route("ConflictResult")]
public async Task<IHttpActionResult> GetConflictResult()
{
    return Conflict();
}
```

>NegotiatedContentResult

内容协商

>FormattedContentResult

方法：`Content<T>()`

MediaTypeFormatter --- 媒体类型格式器   
也就是把webpai 返回的实体，通过格式器来序列化为某种格式，然后返回给前端。   
现在一般使用的是Json格式，所以`Content<T>()`方法在未指定MediaTypeFormatter时默认使用   `JsonMediaTypeFormatter`，返回Json格式一般用`JsonResult`就行了。

有些格式化器可以处理多种格式，这时候需要指定`content-type`

下列是返回xml格式的例子。

```csharp
[Route("FormattedContentResult")]
public async Task<IHttpActionResult> GetFormattedContentResult()
{
    return Content(HttpStatusCode.OK, AjaxResult<string>.Success("ok"), new XmlMediaTypeFormatter(), "application/xml");
}
```




>CreatedNegotiatedContentResult

>CreatedAtRouteNegotiatedContentResult

>InternalServerErrorResult

方法：`InternalServerError()`     
返回状态码500和异常信息
```csharp
[Route("InternalServerErrorResult")]
public async Task<IHttpActionResult> GetInternalServerErrorResult()
{
    return InternalServerError();
}
```

>ExceptionResult

方法：`InternalServerError(Exception exception)`    
返回状态码500和异常信息,和`InternalServerErrorResult`类似，多了异常信息返回给前端
```csharp
[Route("ExceptionResult")]
public async Task<IHttpActionResult> GetExceptionResult()
{
    return InternalServerError(new Exception("test error"));
}
```

>NotFoundResult

方法: `NotFound`

当服务器没有该资源时，返回NotFoundResult，会给前端返回状态码404
```csharp
[Route("NotFoundResult")]
public async Task<IHttpActionResult> GetNotFoundResult()
{
    return NotFound();
}
```

>OkResult

方法：`Ok`、`Ok<T>`     
返回状态码：200 ,不会返回内容

```csharp
[Route("Ok")]
public async Task<IHttpActionResult> GetOk()
{
    return Ok();
}
//返回有内容的200
[Route("Ok")]
public async Task<IHttpActionResult> GetOk()
{
    //默认是json格式的，所以结果和JsonResult相同
    return Ok(AjaxResult<string>.Success("ok"));
}
```


>OkNegotiatedContentResult

>RedirectResult   

返回状态码302     
HTTP 302 Found 重定向状态码表明请求的资源被暂时的移动到了由Location 头部指定的 URL 上。浏览器会重定向到这个URL， 但是搜索引擎不会对该资源的链接进行更新 (In SEO-speak, it is said that the link-juice is not sent to the new URL)
>RedirectToRouteResult

同RedirectResult，返回302状态码并在Location 头部指定跳转URL 
```csharp
[Route("NotFoundResult", Name = "test")]
public async Task<IHttpActionResult> GetNotFoundResult()
{
    return NotFound();
}
[Route("RedirectToRoute")]
public async Task<IHttpActionResult> GetRedirectToRoute()
{
    //跳转到路由名为test的路由器，也就是方法GetNotFoundResult上。
    return RedirectToRoute("test",null);
}
```

>ResponseMessageResult

和直接返回`HttpResponseMessage`效果一致。下面是使用`HttpResponseMessage`来返回json数据的例子，效果和JsonResult是一样的。

返回Josn
```csharp
[Route("ResponseMessageResult")]
public async Task<IHttpActionResult> GetResponseMessageResult()
{
    HttpResponseMessage respon = new HttpResponseMessage();
    string json = JsonConvert.SerializeObject(AjaxResult<string>.Success("ok"));
    respon.Content = new StringContent(json);
    respon.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
    return ResponseMessage(respon);
}
```
返回Stream
```csharp
[Route("filecache/{fileid}")]
[HttpGet]
public HttpResponseMessage GetFile(string fileid)
{
    
    HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
    SourceFileInfo file = SourceFileManager.GetFiles(fileid);
    var tag = Request.Headers.IfNoneMatch.FirstOrDefault();
    string compare = string.Format("\"{0}\"", file.MD5);
    if (tag != null && tag.Tag == compare)
    {
        resp = new HttpResponseMessage(HttpStatusCode.NotModified);
        var code = resp.StatusCode;
    }
    else
    {
        resp = new HttpResponseMessage(HttpStatusCode.OK);
    }
    MemoryStream stream = new MemoryStream(file.Data);
    resp.Content = new StreamContent(stream,3*1024*1024);//设置buffer-size ，否则传输速度会比较慢
    resp.Content.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
    resp.Content.Headers.LastModified = file.Modified;
    resp.Headers.ETag = new EntityTagHeaderValue(string.Format("\"{0}\"", file.MD5));
    resp.Headers.CacheControl = CacheControlHeaderValue.Parse("public");
    return resp;
}
```

>StatusCodeResult

方法：`StatusCode`

返回状态码

```csharp
[Route("StatusCodeResult")]
public async Task<IHttpActionResult> GetStatusCodeResult()
{
    //返回状态码400，这个和BadRequestResult的结果是一样的
    return StatusCode(HttpStatusCode.BadRequest);
}
```

>UnauthorizedResult

返回状态码401   
HTTP401错误代表用户没有访问权限，需要进行身份认证。与这个错误一同返回的还有认证使用的方式（Basic或者Digest）和认证时使用的字段(realm)名称

摘要盘问（digest challenge）    
 相关请求头`WWW-Authenticate`、`Authorization`

 其他相关术语：Access Token、Refresh Token    
 Access Token又包含了Bearer Token、MAC Token



 `Authorization: Scheme parameter`

scheme -- 授权方案    
parameter -- 授权信息，不同的授权方案信息不同。

 亚马逊认证例子
 ```http
Authorization: AWS4-HMAC-SHA256 
Credential=AKIAIOSFODNN7EXAMPLE/20130524/us-east-1/s3/aws4_request, 
SignedHeaders=host;range;x-amz-date,
Signature=fe5f80f77d5fa3beca038a248ff027d0445342fe2855ddc963176630326f1024
 ```

 |授权方案|引用|
 |---|---|
 |Basic|[[RFC7617]](https://www.iana.org/go/rfc7617)|
 |Bearer|[[RFC6750]](https://www.iana.org/go/rfc6750)|
 |Digest|[[RFC7616]](https://www.iana.org/go/rfc7616)|
 |HOBA|[[RFC7486, Section 3]](https://www.iana.org/go/rfc7486)|
 |Mutual|[[RFC8120]](https://www.iana.org/go/rfc8120)|
 |Negotiate|[[RFC4559, Section 3]](https://www.iana.org/go/rfc4559)|
 |OAuth|[[RFC5849, Section 3.5.1]](https://www.iana.org/go/rfc5849)|
 |SCRAM-SHA-1|[[RFC7804]](https://www.iana.org/go/rfc7804)|
 |SCRAM-SHA-256|[[RFC7804]](https://www.iana.org/go/rfc7804)|
 |vapid|[[RFC 8292, Section 3]](https://www.iana.org/go/rfc8292)