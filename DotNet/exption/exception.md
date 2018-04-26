# 工作

## 异常处理思考

* 从.NET2.0开始，任何线程上未处理的异常都会导致应用程序的退出（先会触发APPDomain的UnhandledException）

* 如果发生异常，CLR会根据调用栈一层层找到try{}catch{}模块，如果没有找到，就会发生未处理异常,程序终止。

* 主线程无法捕获子线程的异常，具体看子线程和主线程的调用栈。

```c#
string stackInfo = new StackTrace().ToString();
```
### 异常统一处理
#### windows form程序
```c#
static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Exception error = e.Exception as Exception;
            //记录日志  
        }

        //有异常未被捕获时触发
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception error = e.ExceptionObject as Exception;
            
            //记录日志  
        }
    }
```

#### mvc
* 方法一，重写OnException方法
```c#
public class BaseController : Controller
{
    // GET: Base
    protected override void OnException(ExceptionContext filterContext)
    {
        //base.OnException(filterContext);
        filterContext.ExceptionHandled = true;//标记已处理异常，不会重定向页面到异常页
        filterContext.HttpContext.Response.Clear();
        filterContext.HttpContext.Response.Write(filterContext.Exception.Message);
    }
}

public class HomeController : BaseController
{
    public ActionResult Index()
    {
        throw new Exception("test");
        return View();
    }
}
```
* 方法二，自定义Attribute
```c#
public class MyHandleErrorAttribute:HandleErrorAttribute
{
    public override void OnException(ExceptionContext filterContext)
    {
        filterContext.ExceptionHandled = true;//标记已处理异常，不会重定向页面到异常页
        filterContext.HttpContext.Response.Clear();
        filterContext.HttpContext.Response.Write(filterContext.Exception.Message);
    }
}

public class HomeController : Controller
{
    [MyHandleError]
    public ActionResult Index()
    {
        throw new Exception("test");
        return View();
    }
}
```
* 方法三，在Global.asax中添加Application_Error方法
```c#
public class MvcApplication : System.Web.HttpApplication
{
    protected void Application_Start()
    {
        AreaRegistration.RegisterAllAreas();
        FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        RouteConfig.RegisterRoutes(RouteTable.Routes);
        BundleConfig.RegisterBundles(BundleTable.Bundles);
    }
    void Application_Error(object sender, EventArgs e)
    {
        Exception objErr = Server.GetLastError();
        HttpContext.Current.Response.Write(objErr.Message);
        Server.ClearError();
    }
}
```
**注意**：方法一、二不能同时使用，不然两种方法都会触发，这样就处理了两次异常了
而方法三可以和方法一、二同时使用，如果有使用方法一、二，方法三不会触发。

该统一处理只能捕获主线程中的异常，其他异常看下面

### task中异常处理
**task中异常处理机制**

taks可以用来创建任务

async await是.net4.5开始用来实现异步的操作。

这两种方式写异步程序的时候，如果任务发生，一般情况下是在内部被捕获了（从程序输出信息中可以看出来），所以不会导致程序的崩溃，但是这样也导致了异常不被察觉发现。

当task用WaitAll、Wait、result等方法的时候，异常会抛出到主线程，这时就能用异常统一处理来处理他们
当任务抛出多个异常时有两种情况
1、任务是用task直接创建的，就会抛出AggregateException异常，这是一个异常集合，可以通过他的InnerExceptions属性获取所有的异常。
2、任务是用async await来实现的话只会默认只会抛出第一个异常，当然也可以手动把所有异常抛出来

WhenAll方法并不会传递异常到主线程，会抛出第一个异常，然后task会捕获，赋值到Exception属性中返回。

异步方法是用async修饰，返回值有Task，Task<T>
没有返回值可以用async void来实现方法，但是async void的异常机制和Task，Task<T>又不一样（微软是要作死吗）

还有就是根据资料，用async修饰的异步方法（async void除外），如果有未捕获的异常，会触发绑定的TaskScheduler.UnobservedTaskException事件，然而我的winform demo中并没有触发，而直接用task来创建的任务在有未捕获异常的时候是会触发该事件的。在控制台demo中，两种情况都触发了该事件，目前原因未明

**总结**
还是用ContinueWith来处理异常

处理方法1：try{}catch{}捕获处理

处理方法2：result、WaitAll、waith和WhenAll。在用这几个方法的时候，异常会传递到主线程，然后在主线程捕获异常。
```c#
Task getBuildInfo = new Task(() => BuildBaseTB = CommonMethod.GetBuildBaseInfo());
Task getCityInfo = new Task(() => CityUpdateInfo = Adapter.GetCityUpdateInfo_Defalut());
getBuildInfo.Start();
getCityInfo.Start();
try
{
    Task.WaitAll(getBuildInfo, getCityInfo);
}
catch (AggregateException ex) {
    foreach (var item in ex.InnerExceptions)
    {
        _log.Error(item.InnerException.Message);
    }
}
```
处理方法3：ContinueWith

TaskContinuationOptions.OnlyOnFaulted表示只要在有异常发生的时候才调用ContinueWith

```c#
Task.Factory.StartNew(() =>
    {
        testError();
    }).ContinueWith(t => { 
        var exp = t.Exception; 
        //异常处理代码
    }, TaskContinuationOptions.OnlyOnFaulted); 
```

处理方法4：任务调度器TaskScheduler的UnobservedTaskException委托

UnobservedTaskException只有在GC垃圾回收器执行的时候才触发，所以该方法不是实时性的。
```c#
TaskScheduler.UnobservedTaskException += (s, ex) =>
{
    //设置所有未觉察异常被觉察
    ex.SetObserved();
    //异常处理代码
};
Task.Factory.StartNew(() =>
{
    throw new Exception("test");
});
GC.Collect();
GC.WaitForPendingFinalizers();
```

### thread多线程中异常处理
处理方法1：内部进行捕获处理
处理方法2：window form中可以使用窗体的BeginInvoke方法将异常传递给主窗体线程，由主窗体线程进行处理。
```c#
private void test()
{
    Thread thread = new Thread(new ThreadStart(ThreadStart));
    thread.Start();
}

private void ThreadStart()
{
    try
    {
        throw new Exception("test");
    }
    catch (Exception ex)
    {
        this.BeginInvoke((Action)delegate
        {
            throw ex;
        });
    }

}
```

处理方法3：使用事件回调的方式将工作线程的异常包装到主线程。用事件回调的方式处理异常的好处是提供了统一的入口进行异常处理



