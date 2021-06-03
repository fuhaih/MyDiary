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

控制器是通过调用ExecuteResultAsync方法来获取返回的content的

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