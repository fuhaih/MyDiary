# mvc

## 概念

>mvc概念

模型(model)、视图(view)、控制器(controller)
## 控制器

>ControllerBase

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

>api 控制器

```csharp
[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
}
```

>mvc 控制器

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



## 视图

## 模型

# Result
## IActionResult(api&mvc)

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

## IActionResult(mvc)


>JsonResult

>PartialViewResult

>ViewResult

>ViewComponentResult

## 自定义IActionResult

继承IActionResult并实现ExecuteResultAsync方法


>IActionResult

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




>Response

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

>自定义IActionResult

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

## IActionResult总结

mvc控制器包含的方法中多了几个视图类型的Result方法(ViewResult...)以及JsonResult返回值的方法`Json`

>ObjectResult and JsonResult

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

添加一个xml格式化器，并添加mapper映射，FormatFilter会通过map找mediatype

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

获取xml数据,设置请求头`Accept`为`application/xml`
```s
GET http://localhost:20583/api/values/value.xml HTTP/1.1
```
```xml
<UserRespon xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <Id>1</Id>
    <Name>test</Name>
</UserRespon>
```

这样就不用设置Accept头了，通过url来指定获取的数据格式

>ContentResult

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

>

>状态码相关的Result

>ChallengeResult

`An ActionResult that on execution invokes HttpContext.ChallengeAsync.`

`HttpContext.ChallengeAsync`是和第三方认证服务相关的方法，所以ChallengeResult应该也是和第三方认证服务有关




# Filter

## 工作原理和默认顺序

## 内置相关类

>ActionFilterAttribute

>ExceptionFilterAttribute

>ResultFilterAttribute

>FormatFilterAttribute

>ServiceFilterAttribute

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

>TypeFilterAttribute

和ServiceFilter类似，但不是使用容器来构造对象

```csharp
[TypeFilter(typeof(LogConstantFilter),
    Arguments = new object[] { "Method 'Hi' called" })]
public IActionResult Hi(string name)
{
    return Content($"Hi {name}");
}
```

## Filter

>


# Parameter

# webapi


* 参数：

各种参数类型json、urlencode、formdata、parameter等对应的特性

* 返回值：

各种类型返回值

* 异常处理

TypeFilter


## mvc

* 认证

[链接](../授权和认证.md)

[dotnet core 授权认证](./dotnet-core-授权认证.md)

* 内置对象

HttpContext

* 内置注入对象

IMemoryCache

ISession