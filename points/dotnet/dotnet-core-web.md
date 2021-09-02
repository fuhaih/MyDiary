# 1 mvc

## 1.1 概念

>mvc概念

模型(model)、视图(view)、控制器(controller)
## 1.2 控制器

### 1.2.1 ControllerBase

`ControllerBase`是控制器基类，mvc控制器和api控制器都是继承自`ControllerBase`
```csharp
//
// 摘要:
//     A base class for an MVC controller without view support.
[Controller]
public abstract class ControllerBase
{
    protected ControllerBase();

    //
    // 摘要:
    //     Gets the Microsoft.AspNetCore.Routing.RouteData for the executing action.
    public RouteData RouteData { get; }
    //
    // 摘要:
    //     Gets the Microsoft.AspNetCore.Http.HttpResponse for the executing action.
    public HttpResponse Response { get; }
    //
    // 摘要:
    //     Gets the Microsoft.AspNetCore.Http.HttpRequest for the executing action.
    public HttpRequest Request { get; }
    public ProblemDetailsFactory ProblemDetailsFactory { get; set; }
    //
    // 摘要:
    //     Gets or sets the Microsoft.AspNetCore.Mvc.ModelBinding.Validation.IObjectModelValidator.
    public IObjectModelValidator ObjectValidator { get; set; }
    //
    // 摘要:
    //     Gets the Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary that contains
    //     the state of the model and of model-binding validation.
    public ModelStateDictionary ModelState { get; }
    //
    // 摘要:
    //     Gets or sets the Microsoft.AspNetCore.Mvc.ModelBinding.IModelBinderFactory.
    public IModelBinderFactory ModelBinderFactory { get; set; }
    //
    // 摘要:
    //     Gets or sets the Microsoft.AspNetCore.Mvc.ModelBinding.IModelMetadataProvider.
    public IModelMetadataProvider MetadataProvider { get; set; }
    //
    // 摘要:
    //     Gets the Microsoft.AspNetCore.Http.HttpContext for the executing action.
    public HttpContext HttpContext { get; }
    //
    // 摘要:
    //     Gets or sets the Microsoft.AspNetCore.Mvc.ControllerContext.
    //
    // 言论：
    //     Microsoft.AspNetCore.Mvc.Controllers.IControllerActivator activates this property
    //     while activating controllers. If user code directly instantiates a controller,
    //     the getter returns an empty Microsoft.AspNetCore.Mvc.ControllerContext.
    [ControllerContext]
    public ControllerContext ControllerContext { get; set; }
    //
    // 摘要:
    //     Gets or sets the Microsoft.AspNetCore.Mvc.IUrlHelper.
    public IUrlHelper Url { get; set; }
    //
    // 摘要:
    //     Gets the System.Security.Claims.ClaimsPrincipal for user associated with the
    //     executing action.
    public ClaimsPrincipal User { get; }
}

```

### 1.2.2 api 控制器

```csharp
[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
}
```

### 1.2.3 mvc 控制器

mvc控制器是继承自`Controller`，而`Controller`是继承自`ControllerBase`,`Controller`相对于`ControllerBase`多了一些视图相关的字段和方法。

```csharp
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
}
```

`Controller`
```csharp
//
// 摘要:
//     A base class for an MVC controller with view support.
public abstract class Controller : ControllerBase, IActionFilter, IFilterMetadata, IAsyncActionFilter, IDisposable
{
    protected Controller();

    //
    // 摘要:
    //     Gets or sets Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataDictionary used by
    //     Microsoft.AspNetCore.Mvc.ViewResult.
    public ITempDataDictionary TempData { get; set; }
    //
    // 摘要:
    //     Gets the dynamic view bag.
    [Dynamic]
    public dynamic ViewBag { get; }
    //
    // 摘要:
    //     Gets or sets Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary used by
    //     Microsoft.AspNetCore.Mvc.ViewResult and Microsoft.AspNetCore.Mvc.Controller.ViewBag.
    //
    // 言论：
    //     By default, this property is initialized when Microsoft.AspNetCore.Mvc.Controllers.IControllerActivator
    //     activates controllers.
    //     This property can be accessed after the controller has been activated, for example,
    //     in a controller action or by overriding Microsoft.AspNetCore.Mvc.Controller.OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext).
    //     This property can be also accessed from within a unit test where it is initialized
    //     with Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider.
    [ViewDataDictionary]
    public ViewDataDictionary ViewData { get; set; }
}
```



## 1.3 视图

## 1.4 模型

# 2 Result

## 2.1 IActionResult(api&mvc)

>AcceptedResult

>AcceptedAtActionResult

>AcceptedAtRouteResult

> **BadRequestObjectResult**

> **BadRequestResult** 

>ChallengeResult

>ConflictObjectResult

>ConflictResult

> **ContentResult**

>CreatedResult

>CreatedAtActionResult

>CreatedAtRouteResult

> **VirtualFileResult**

> **FileStreamResult**

> **FileContentResult**

>ForbidResult

>LocalRedirectResult

>NoContentResult

>NotFoundObjectResult

>NotFoundResult

> **OkResult**

> **OkObjectResult**

> **PhysicalFileResult**

> **ObjectResult**

> **RedirectResult**

> **RedirectToActionResult**

> **RedirectToPageResult**

> **RedirectToRouteResult**

> **SignInResult**

> **SignOutResult**

> **StatusCodeResult**

> **UnauthorizedObjectResult**

> **UnauthorizedResult**

>UnprocessableEntityObjectResult

>UnprocessableEntityResult

> **ActionResult**

## 2.2 IActionResult(mvc)

>JsonResult

>PartialViewResult

>ViewResult

>ViewComponentResult

## 2.3 自定义IActionResult

继承IActionResult并实现ExecuteResultAsync方法

### 2.3.1 IActionResult

```csharp
//
// 摘要:
//     Defines a contract that represents the result of an action method.
public interface IActionResult
{
    //
    // 摘要:
    //     Executes the result operation of the action method asynchronously. This method
    //     is called by MVC to process the result of an action method.
    //
    // 参数:
    //   context:
    //     The context in which the result is executed. The context information includes
    //     information about the action that was executed and request information.
    //
    // 返回结果:
    //     A task that represents the asynchronous execute operation.
    Task ExecuteResultAsync(ActionContext context);
}
```

IActionResult定义了一个`ExecuteResultAsync`方法，控制器在获取到IActionResult对象后，调用该方法来写入Respon数据，`ActionContext`类中包含了HttpContent对象，通过该对象写入响应数据




### 2.3.2 Response

IActionResult方便了响应数据的写入，如果是返回的json数据，直接返回一个JosnResult就行了，也可以自己写Respon，如下Response方式的接口编写。

```csharp
[Route("respon")]
[HttpGet]
public async Task GetRespon() {
    UserRespon user = new UserRespon();
    string result = JsonConvert.SerializeObject(user);
    byte[] data = Encoding.UTF8.GetBytes(result);
    
    HttpContext.Response.ContentType = "application/json";
    HttpContext.Response.StatusCode = 200;
    //Body.WriteAsync之后respon已经开始了，所以ContentType和StatusCode需要在之前设置
    await HttpContext.Response.Body.WriteAsync(data, 0, data.Length);
}
```

### 2.3.3 自定义IActionResult

定义MyJsonResult
```csharp
public class MyJsonResult: ActionResult
{
    private object Data;
    public MyJsonResult(object data) {
        this.Data = data;
    }
    public override async Task ExecuteResultAsync(ActionContext context)
    {
        string result = JsonConvert.SerializeObject(Data);
        byte[] data = Encoding.UTF8.GetBytes(result);
        context.HttpContext.Response.ContentType = "application/json";
        context.HttpContext.Response.StatusCode = 200;
        //Body.WriteAsync之后respon已经开始了，所以ContentType和StatusCode需要在之前设置
        await context.HttpContext.Response.Body.WriteAsync(data, 0, data.Length);
        //await base.ExecuteResultAsync(context);
    }
}

```

使用MyJsonResult

```csharp
[Route("action")]
[HttpGet]
public async Task<IActionResult> GetMyAction() {

    UserRespon user = new UserRespon();
    MyJsonResult json = new MyJsonResult(user);
    return json;
}
```

## 2.4IActionResult总结

mvc控制器包含的方法中多了几个视图类型的Result方法(ViewResult...)以及JsonResult返回值的方法`Json`

### 2.4.1 ObjectResult and JsonResult

这两个都可以进行数据格式化

`JsonResult` 返回json格式数据

`ObjectResult` 会进行内容协商，然后找到合适的格式化程序进行格式化

* Accept Header

请求头中设置了Accept后，会优先匹配Accept的类型，从`OutputFormatters`中找到设置的格式化器，如果没有匹配到合适的格式化器，就会使用默认的json格式化器

* Content-Type

ObjectResult中的Content-Type字段？


添加一个xml格式化器

```csharp
services.AddControllersWithViews(options=> {
    //options.ReturnHttpNotAcceptable = true;
    //options.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();
    //options.InputFormatters.Add(new XmlSerializerInputFormatter(options));
    options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
    //options.FormatterMappings.SetMediaTypeMappingForFormat("xml", "application/xml");
});
```
使用`OkObjectResult`,该类型继承自`ObjectResult`
```csharp
[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
    [Route("user")]
    [HttpGet]
    public async Task<IActionResult> GetUser() {
        UserRespon user = new UserRespon();
        return Ok(user);
    }
}
```
获取json数据
```s
GET http://localhost:20583/api/values/user HTTP/1.1
# 可以忽略Accept，默认就是json格式
Accept：application/json
```
```json
{
    "id": 1,
    "name": "test"
}
```

获取xml数据,设置请求头`Accept`为`application/xml`

```s
GET http://localhost:20583/api/values/user HTTP/1.1
Accept：application/xml
```

```xml
<UserRespon xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <Id>1</Id>
    <Name>test</Name>
</UserRespon>
```

ObjectResult的好处就是可以很方便的返回各种不同格式的数据，有些客户端由于历史遗留原因或者是使用不支持json格式的语音时候，可以在请求头中通过设置`Accept`来规定返回数据的格式，以获取到自己支持的格式的数据。

也可以使用`FormatFilter`来协商内容，`FormatFilter`是一种筛选器，它将使用路由数据或查询字符串中的格式值来设置从操作返回的的内容类型 ObjectResult

[FormatFilter](#324-FormatFilterAttribute)

这样就不用设置Accept头了，通过url来指定获取的数据格式

### 2.4.2 ContentResult

格式化输出除了上面的ObjectResult和JsonResult之外，还能用ContentResult

```csharp
[Route("content")]
[HttpGet]
public async Task<IActionResult> GetContent() {
    UserRespon user = new UserRespon();
    string result = JsonConvert.SerializeObject(user);
    ContentResult content = new ContentResult()
    {
        Content = result,
        ContentType = "application/json",
        StatusCode = 200
    };
    return Content(result, "application/json");
    //return Content(result, "application/json");
    //return Content(result,new MediaTypeHeaderValue("application/json"));
}
```

把需要返回的对象先格式化，然后赋值给Content，指定格式化的类型ContentType,指定状态码

控制器内置的方法`Content`默认状态码是200

### 2.4.3 StatusCodeResult

AcceptedResult(202)、BadRequestResult(400)、ForbidResult(403)、ConflictResult(409)、OkResult(200)、RedirectResult(301/302)、NoContentResult(204)、UnauthorizedResult(401)、NotFoundResult(404)、UnprocessableEntityResult(422)

301 redirect: 301 代表永久性转移(Permanently Moved)、

302 redirect: 302 代表暂时性转移(Temporarily Moved )

重定向时会把重定向的地址写入到响应头`location`中

其他http状态可以使用`StatusCodeResult`

```csharp
//500 服务器错误
return StatusCode(500)
```

### 2.4.4ChallengeResult

`An ActionResult that on execution invokes HttpContext.ChallengeAsync.`

`HttpContext.ChallengeAsync`是和第三方认证服务相关的方法，所以ChallengeResult应该也是和第三方认证服务有关


### 2.4.5 FileResult

`VirtualFileResult`、`FileStreamResult`、`PhysicalFileResult`、`FileContentResult`

这四个类型都是继承自`FileResult`，只是传递的参数有所区别，分别需要 虚拟路径、stream流、物理路径、byte[]数组

```csharp
//
// 摘要:
//     Represents an Microsoft.AspNetCore.Mvc.ActionResult that when executed will write
//     a file as the response.
public abstract class FileResult : ActionResult
{
    //
    // 摘要:
    //     Creates a new Microsoft.AspNetCore.Mvc.FileResult instance with the provided
    //     contentType.
    //
    // 参数:
    //   contentType:
    //     The Content-Type header of the response.
    protected FileResult(string contentType);

    //
    // 摘要:
    //     Gets the Content-Type header for the response.
    public string ContentType { get; }
    //
    // 摘要:
    //     Gets or sets the value that enables range processing for the Microsoft.AspNetCore.Mvc.FileResult.
    public bool EnableRangeProcessing { get; set; }
    //
    // 摘要:
    //     Gets or sets the etag associated with the Microsoft.AspNetCore.Mvc.FileResult.
    public EntityTagHeaderValue EntityTag { get; set; }
    //
    // 摘要:
    //     Gets the file name that will be used in the Content-Disposition header of the
    //     response.
    public string FileDownloadName { get; set; }
    //
    // 摘要:
    //     Gets or sets the last modified information associated with the Microsoft.AspNetCore.Mvc.FileResult.
    public DateTimeOffset? LastModified { get; set; }
}
```
* ContentType

文件下载时候的`ContentType`一般为`application/octet-stream`或者对应的mime类型

* EnableRangeProcessing

是否支持Range处理，这个是和断点续传有关的，涉及到的请求头和响应头有`Accept-Ranges`、`Range`、`Content-Range`，设置为true将能支持文件的断点下载。

[.net framwork中文件断点下载实现]()

[range请求头]()

* FileDownloadName

文件下载名称，会写在`Content-Disposition`响应头中

* EntityTag、LastModified

`EntityTag`和`LastModified`这连个字段涉及到http请求资源变更问题，其中涉及到的http 请求头和响应头有以下四个

`Last-Modified`、`ETag`、`If-Modified-Since`、`If-None-Match`

[资源变更]()



### 2.4.5 


# 3 Filter

## 3.1工作原理和默认顺序

### 3.1.1filter管道

在请求进来后，会执行各种中间件，最后会选择控制器的action进行操作，Filter就是在选择好action后的filter管道中运行的。

![这是图片](filter-pipeline-1.png)

下面是filter管道

![这是图片](filter-pipeline-2.png)

### 3.1.2 IAuthorizationFilter

### 3.1.3 IResourceFilter

```csharp
public class MyResourceFilter : IResourceFilter
{
    public void OnResourceExecuted(ResourceExecutedContext context)
    {
        UserRespon user = new UserRespon();
        user.Name = "MyResourceFilter_OnResourceExecuted";
        context.Result = new JsonResult(user);
    }

    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        UserRespon user = new UserRespon();
        user.Name = "MyResourceFilter_OnResourceExecuting";
        context.Result = new JsonResult(user);
        //throw new NotImplementedException();
    }
}
```

IResourceFilter 有两个方法，`OnResourceExecuting`和`OnResourceExecuted`

`OnResourceExecuting`中如果设置了Result，将会中断后续的Filter以及控制器的Action，直接返回该Result，相当于Filter管道直接结束了。

如果没有设置Result，那么就会继续正常执行Filter管道，但是`OnResourceExecuted`中设置的Result将不起作用，`OnResourceExecuted`在执行之前，Result就已经写入到Response中了。


### 3.1.4 IActionFilter

```csharp
// 执行顺序
// OnActionExecuting、action、OnActionExecuted
// result的设置权重
// OnActionExecuting、OnActionExecuted、action

public class MyActionFilter : IActionFilter
{
    // 在controller.action执行之后执行
    public void OnActionExecuted(ActionExecutedContext context)
    {
        UserRespon user = new UserRespon();
        user.Name = "OnActionExecuted";
        context.Result = new JsonResult(user);
    }
    // 在controller.action执行之前执行
    public void OnActionExecuting(ActionExecutingContext context)
    {
        UserRespon user = new UserRespon();
        user.Name = "OnActionExecuting";
        context.Result = new JsonResult(user);
    }
}
```

* OnActionExecuting中设置了result后，action和OnActionExecuted的设置将无效

* OnActionExecuting没有设置时，OnActionExecuted中设置的result会覆盖action的设置



### 3.1.5 IExceptionFilter

用来处理异常

### 3.1.6 IResultFilter

`ResultExecutingContext`中的`Result`是只读的，也就是不能够在`IResultFilter`中修改Result

`IResultFilter`的作用一般是用来修改请求的Respon头信息。可以用来配置跨域等功能。

```csharp
public class AddHeaderResultServiceFilter : IResultFilter
{
    private ILogger _logger;
    public AddHeaderResultServiceFilter(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<AddHeaderResultServiceFilter>();
    }

    public void OnResultExecuting(ResultExecutingContext context)
    {
        var headerName = "OnResultExecuting";
        context.HttpContext.Response.Headers.Add(
            headerName, new string[] { "ResultExecutingSuccessfully" });
        _logger.LogInformation("Header added: {HeaderName}", headerName);
    }

    public void OnResultExecuted(ResultExecutedContext context)
    {
        // Can't add to headers here because response has started.
        _logger.LogInformation("AddHeaderResultServiceFilter.OnResultExecuted");
    }
}

```

### 3.1.7 多个同一类filter执行顺序

当filter有两个方法时，一般一个是先调用的Executing方法(before)和后调用的Executed方法(after)

filter的方法调用顺序如下

* The before code of global filters.
* The before code of controller and Razor Page filters.
* The before code of action method filters.
* The after code of action method filters.
* The after code of controller and Razor Page filters.
* The after code of global filters.


**例子：**

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllersWithViews(options =>
   {
        options.Filters.Add(typeof(MySampleActionFilter));
    });
}
```

```csharp
public class TestController : Controller
{
    [SampleActionFilter(Order = int.MinValue)]
    public IActionResult FilterTest2()
    {
        return ControllerContext.MyDisplayRouteInfo();
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // Do something before the action executes.
        MyDebug.Write(MethodBase.GetCurrentMethod(), HttpContext.Request.Path);
        base.OnActionExecuting(context);
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        // Do something after the action executes.
        MyDebug.Write(MethodBase.GetCurrentMethod(), HttpContext.Request.Path);
        base.OnActionExecuted(context);
    }
}
```
给`TestController`配置两个ActionFilter，一个是全局配置的`MySampleActionFilter`,另一个是在Action中配置的`SampleActionFilter`，通过`OnActionExecuting`和`OnActionExecuted`回调查看两个Filter中方法的执行顺序如下


* TestController.OnActionExecuting
  * MySampleActionFilter.OnActionExecuting
    * SampleActionFilterAttribute.OnActionExecuting
      * TestController.FilterTest2
    * SampleActionFilterAttribute.OnActionExecuted
  * MySampleActionFilter.OnActionExecuted
* TestController.OnActionExecuted
## 3.2 内置相关类

### 3.2.1 ActionFilterAttribute

### 3.2.2 ExceptionFilterAttribute

### 3.2.3 ResultFilterAttribute

### 3.2.4 FormatFilterAttribute 


FormatFilter不属于上面的五中类型的Filter，但是肯定也是在Filter管道中执行的

1、FormatFilter是在IActionFilter之后执行的。

2、FormatFilter需要添加格式化器，同时添加format和格式化器的映射，FormatFilter会找到url中的format，然后通过map映射找到对应的格式化器，对返回数据进行格式化。

3、FormatFilter只对ObjectResult类型或者其子类生效。因为和设置Accept头类似，都是通过格式协商来进行格式化的，只有ObjectResult类型支持格式协商。

**例子：**

添加一个xml格式化器，并添加mapper映射，FormatFilter会通过map找mediatype，再通过mediatype找到Formatter

```csharp
services.AddControllersWithViews(options=> {
    //options.ReturnHttpNotAcceptable = true;
    //options.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();
    //options.InputFormatters.Add(new XmlSerializerInputFormatter(options));
    options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
    options.FormatterMappings.SetMediaTypeMappingForFormat("xml", "application/xml");
});
```

使用`FormatFilter`
```csharp
[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
    [Route("value.{format}")]
    [HttpGet()]
    [FormatFilter()]
    public async Task<IActionResult> GetValues() {
        UserRespon user = new UserRespon();
        return Ok(user);
    }
}
```

获取json数据
```s
GET http://localhost:20583/api/values/value.json HTTP/1.1
```
```json
{
    "id": 1,
    "name": "test"
}
```

获取xml数据
```s
GET http://localhost:20583/api/values/value.xml HTTP/1.1
```
```xml
<UserRespon xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <Id>1</Id>
    <Name>test</Name>
</UserRespon>
```

### 3.2.5 ServiceFilterAttribute

用来添加Filter的，有些Filter需要使用到依赖注入，不能直接通过中括号进行使用，需要用到ServiceFilter

先注册MyActionFilterAttribute对象
```csharp
services.AddSingleton<MyActionFilterAttribute>();
```
然后使用ServiceFilter来注入MyActionFilterAttribute
```csharp
[ServiceFilter(typeof(MyActionFilterAttribute))]
public class IndexModel : PageModel
{
    public void OnGet()
    {
    }
}
```

### 3.2.6 TypeFilterAttribute

和ServiceFilter类似，但是会每个请求都构建一个对象，这样开销比较大

```csharp
[TypeFilter(typeof(LogConstantFilter),
    Arguments = new object[] { "Method 'Hi' called" })]
public IActionResult Hi(string name)
{
    return Content($"Hi {name}");
}
```



## 
# 4 Parameter绑定

[http传参](../http.md#12-传参)有两种方式，一种是通过url传参，一种是通过body来传参，通过body传参又有三种常用的格式(ContentType) `application/json`、`application/x-www-form-urlencoded`、`multipart/form-data` ,对应的几种不同的传参方式和格式，dotnet core后端需要做不同的处理

## 4.1 QueryString

`FromQuery`可以把`QueryString`中的传递过来的数据绑定到方法参数中

```csharp
[Route("querystring")]
[HttpGet]
public async Task<IActionResult> QueryString([FromQuery] string name,[FromQuery]string action)
{
    return Ok(new { name = name ,action = action});
}
```

## 4.2 Json

`FromBody`可以绑定`application/json`参数

```csharp
[Route("json")]
[HttpPost]
public async Task<IActionResult> QueryJson([FromBody] UserRespon user) {
    return Ok(user);
}
```

## 4.3 UrlEncoded、FormData

`FromForm`可以绑定`application/x-www-form-urlencoded`、`multipart/form-data`数据

```csharp
public class UserRespon
{
    public int Id { get; set; } = 1;
    public string Name { get; set; } = "test";
}

public class UserFile : UserRespon
{ 
    public IFormFile file { get; set; }
}

[Route("urlencode")]
[HttpPost]
public async Task<IActionResult> QueryUrlEncode([FromForm] UserRespon user)
{
    return Ok(user);
}
[Route("formdata")]
[HttpPost]
public async Task<IActionResult> QueryFormData([FromForm] UserFile user)
{
    return Ok();
}

```

## 4.5 自定义参数绑定

```csharp

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
public class ModelBinderAttribute : Attribute, IBinderTypeProviderMetadata, IBindingSourceMetadata, IModelNameProvider
{
    public ModelBinderAttribute();
    public ModelBinderAttribute(Type binderType);
    public Type BinderType { get; set; }
    public virtual BindingSource BindingSource { get; protected set; }
    public string Name { get; set; }
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
public class FromFormAttribute : Attribute, IBindingSourceMetadata, IModelNameProvider
{
    public FromFormAttribute();

    public BindingSource BindingSource { get; }
    public string Name { get; set; }
}

public interface IBindingSourceMetadata
{
    BindingSource BindingSource { get; }
}
[DebuggerDisplay("Source: {DisplayName}")]
public class BindingSource : IEquatable<BindingSource>
{
    public static readonly BindingSource Body;
    public static readonly BindingSource Custom;
    public static readonly BindingSource Form;
    public static readonly BindingSource FormFile;
    public static readonly BindingSource Header;
    public static readonly BindingSource ModelBinding;
    public static readonly BindingSource Path;
    public static readonly BindingSource Query;
    public static readonly BindingSource Services;
    public static readonly BindingSource Special;
    public BindingSource(string id, string displayName, bool isGreedy, bool isFromRequest);
    public string DisplayName { get; }
    public string Id { get; }
    public bool IsFromRequest { get; }
    public bool IsGreedy { get; }
    public virtual bool CanAcceptDataFrom(BindingSource bindingSource);
    public bool Equals(BindingSource other);
    public override bool Equals(object obj);
    public override int GetHashCode();

    public static bool operator ==(BindingSource s1, BindingSource s2);
    public static bool operator !=(BindingSource s1, BindingSource s2);
}

```

从上面源码可以看出[ModelBinder]和[FromForm]都实现了`IBindingSourceMetadata`接口，

## 4.6 modelstate

### ModelBinderProvider、BindingSourceValueProvider、IInputFormatters

数据源：

`BindingSourceValueProvider` 解析参数值


```csharp
public interface IValueProvider
{
    bool ContainsPrefix(string prefix);
    ValueProviderResult GetValue(string key);
}
public abstract class BindingSourceValueProvider : IBindingSourceValueProvider, IValueProvider
{
    public BindingSourceValueProvider(BindingSource bindingSource);
    protected BindingSource BindingSource { get; }
    public abstract bool ContainsPrefix(string prefix);
    public virtual IValueProvider Filter(BindingSource bindingSource);
    public abstract ValueProviderResult GetValue(string key);
}
public class FormValueProvider : BindingSourceValueProvider, IEnumerableValueProvider, IValueProvider
{
    public FormValueProvider(BindingSource bindingSource, IFormCollection values, CultureInfo culture);
    public CultureInfo Culture { get; }
    protected PrefixContainer PrefixContainer { get; }
    public override bool ContainsPrefix(string prefix);
    public virtual IDictionary<string, string> GetKeysFromPrefix(string prefix);
    public override ValueProviderResult GetValue(string key);
}
```

`ModelBinderProvider` 把参数值绑定到参数中

`FromForm`里面就是指定了模型绑定用的`IModelBinder`

`InputFormatters`暂时不知道用法


`BindingSource`

### IModelBinder



# 5 HttpContext

在.net framework中，mvc和webapi是返回的`IHttpActionResult`对象
```csharp
//
// 摘要:
//     定义一个用于以异步方式创建 System.Net.Http.HttpResponseMessage 的命令。
public interface IHttpActionResult
{
    //
    // 摘要:
    //     以异步方式创建 System.Net.Http.HttpResponseMessage。
    //
    // 参数:
    //   cancellationToken:
    //     要监视的取消请求标记。
    //
    // 返回结果:
    //     在完成时包含 System.Net.Http.HttpResponseMessage 的任务。
    Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken);
}
```

该接口的`ExecuteAsync`方法是返回一个`HttpResponseMessage`对象

```csharp
//
// 摘要:
//     表示包括状态代码和数据的 HTTP 响应消息。
public class HttpResponseMessage : IDisposable
{
    //
    // 摘要:
    //     初始化 System.Net.Http.HttpResponseMessage 类的新实例。
    public HttpResponseMessage();
    //
    // 摘要:
    //     初始化指定的 System.Net.Http.HttpResponseMessage.StatusCode 的 System.Net.Http.HttpResponseMessage
    //     类的新实例。
    //
    // 参数:
    //   statusCode:
    //     HTTP 响应的状态代码。
    public HttpResponseMessage(HttpStatusCode statusCode);

    //
    // 摘要:
    //     获取或设置 HTTP 响应消息的内容。
    //
    // 返回结果:
    //     返回 System.Net.Http.HttpContent。HTTP 响应消息的内容。
    public HttpContent Content { get; set; }
    //
    // 摘要:
    //     获取 HTTP 响应标头的集合。
    //
    // 返回结果:
    //     返回 System.Net.Http.Headers.HttpResponseHeaders。HTTP 响应标头的集合。
    public HttpResponseHeaders Headers { get; }
    //
    // 摘要:
    //     获取一个值，该值指示 HTTP 响应是否成功。
    //
    // 返回结果:
    //     返回 System.Boolean。指示 HTTP 响应是否成功的值。如果 System.Net.Http.HttpResponseMessage.StatusCode
    //     在 200-299 范围中，则为 true；否则为 false。
    public bool IsSuccessStatusCode { get; }
    //
    // 摘要:
    //     获取或设置服务器与状态代码通常一起发送的原因短语。
    //
    // 返回结果:
    //     返回 System.String。服务器发送的原因词组。
    public string ReasonPhrase { get; set; }
    //
    // 摘要:
    //     获取或设置导致此响应消息的请求消息。
    //
    // 返回结果:
    //     返回 System.Net.Http.HttpRequestMessage。导致此响应信息的请求消息。
    public HttpRequestMessage RequestMessage { get; set; }
    //
    // 摘要:
    //     获取或设置 HTTP 响应的状态代码。
    //
    // 返回结果:
    //     返回 System.Net.HttpStatusCode。HTTP 响应的状态代码。
    public HttpStatusCode StatusCode { get; set; }
    //
    // 摘要:
    //     获取或设置 HTTP 消息版本。
    //
    // 返回结果:
    //     返回 System.Version。HTTP 消息版本。默认值为 1.1。
    public Version Version { get; set; }

    //
    // 摘要:
    //     释放由 System.Net.Http.HttpResponseMessage 使用的非托管资源。
    public void Dispose();
    //
    // 摘要:
    //     如果 HTTP 响应的 System.Net.Http.HttpResponseMessage.IsSuccessStatusCode 属性为 false，
    //     将引发异常。
    //
    // 返回结果:
    //     返回 System.Net.Http.HttpResponseMessage。如果调用成功则 HTTP 响应消息。
    public HttpResponseMessage EnsureSuccessStatusCode();
    //
    // 摘要:
    //     返回表示当前对象的字符串。
    //
    // 返回结果:
    //     返回 System.String。当前对象的字符串表示形式。
    public override string ToString();
    //
    // 摘要:
    //     释放由 System.Net.Http.HttpResponseMessage 使用的非托管资源，并可根据需要释放托管资源。
    //
    // 参数:
    //   disposing:
    //     如果为 true，则释放托管资源和非托管资源；如果为 false，则仅释放非托管资源。
    protected virtual void Dispose(bool disposing);
}
```

所以在mvc和webapi方法中，也能直接返回一个`HttpResponseMessage`对象，然后通过设置该对象的HttpContent来返回数据

例子：

视频断点下载

```csharp
var stream = await bucket.OpenDownloadStreamAsync(id, options);
if (Request.Headers.Range != null)
{
    HttpResponseMessage partialResponse = Request.CreateResponse(HttpStatusCode.PartialContent);
    partialResponse.Content = new ByteRangeStreamContent(stream, Request.Headers.Range, type, 3 * 1024 * 1024);
    return partialResponse;
}
else
{
    HttpResponseMessage fullResponse = new HttpResponseMessage(HttpStatusCode.OK);
    fullResponse.Content = new StreamContent(stream, 3 * 1024 * 1024);
    fullResponse.Content.Headers.ContentType = new MediaTypeHeaderValue(type);
    fullResponse.Headers.AcceptRanges.Add("bytes");
    return fullResponse;
}
```
而在dotnet core中已经修改了数据返回的方式，通过IActionResult对象想Response中写入返回数据，所以暂时用不到HttpContent，但是如果使用dotnet core来进行http请求，也是需要使用到HttpContent的

[HttpContent](./http-client.md#12-HttpContent)的使用在dotnet framework和dotnet core都是类似的。




# 6 场景

## 6.1 授权认证

[链接](../授权和认证.md)

[dotnet core 授权认证](./dotnet-core-授权认证.md)

## 6.2 在其他服务中获取授权认证用户信息

在进行授权认证后，可以通过`HttpContext.User`获取到用户信息(ClaimsPrincipal),这在Controller中是很方便就能获取到的，但是在其他服务中需要通过参数传递的方式获取，这样比较麻烦

dotnet core提供了`IHttpContextAccessor`来帮助我们获取HttpContext对象，我们可以根据需求扩展自己的服务

方案1：

注册`IHttpContextAccessor`
```csharp
services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
```

创建一个服务来获取HttpContext对象

```csharp
public class ContextService
{
    private IHttpContextAccessor httpContextAccessor;
    public ContextService(IServiceProvider serviceProvider) {
        httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();
    }
    public HttpContext HttpContext{
        get { return httpContextAccessor.HttpContext; }
    }
}
// IHttpContextAccessor能够解析当前的上下文，所以不需要使用AddScoped来确保上下文正确，直接使用单例模式(AddSingleton)就能针对不同请求获取响应的上下文信息
services.AddSingleton<ContextService>();
```
需要使用时注入到对应的服务就行了

缺点：不太建议在其他服务中直接使用HttpContext对象，特别是在后台服务中，可能请求已经结束，HttpContext对象已经释放。

改进：

```csharp
public class ContextService
{
    public ClaimsPrincipal User;
    public ContextService(IServiceProvider serviceProvider) {
        var httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();
        User = httpContextAccessor.HttpContext.User;
    }
}
services.AddScoped<ContextService>();
```

改进后的ContextService是使用`IHttpContextAccessor`获取到所需要的数据即可，这时候需要使用`AddScoped`来注册服务，确保每个请求获取到的ContextService对象都不一样。


方案2：

```csharp
services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
services.AddScoped(x =>
{
    var context = x.GetService<IHttpContextAccessor>();
    return context.HttpContext.User;
});
```

通过这种方式注册`ClaimsPrincipal`对象，在需要使用的服务中可以直接注入`ClaimsPrincipal`对象，包括各种Filter中也是可以注入。

# 7 内置注入对象

IMemoryCache

ISession

# 8 Middleware 中间件

# 9 AOP编程

## Middleware

## filter

## 动态代理(AspectCore)

# websocket

dotnet 中的websocket是使用原有的http管道的，所以和http请求挂钩，由http请求升级为websocket，当请求结束时，websocket会关闭，所以如果需要保持连接，需要保证请求管道一直运行着，直到客户端关闭时再关闭管道。

