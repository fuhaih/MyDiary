>IHttpClientFactory


[guidelines](https://docs.microsoft.com/en-us/dotnet/fundamentals/networking/httpclient-guidelines)
[IHttpClientFactory](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-6.0)

HttpClient 最好不要用完就释放，容易耗尽socket资源，所以需要弄一个单例，IHttpClientFactory就是用来管理HttpClient资源的

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddHttpClient(HttpClientName.NAME)
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
    {
        MaxConnectionsPerServer = 10,
        ServerCertificateCustomValidationCallback = (sender, cert, chain, error) => true
    });
}
```

```csharp
[Route("api/[controller]/[action]")]
[ApiController]
[NoLogin]
public class TestController : ControllerBase
{

    private IHttpClientFactory _httpClientFactory;
    public TestController(IConfiguration configuration,IHttpClientFactory factory)
    {
        this._httpClientFactory = factory;
    }
    
    [HttpGet]
    public async Task<string> GetRtu()
    {
        var client = _httpClientFactory.CreateClient(HttpClientName.NAME);
        ModbusRtu rtu = new ModbusRtu
        {
            DataBits = 8,
            Address = 3,
            BaudRate = 9600,
            Parity = 0,
            PortName = "COM1",
            Protocol =  Shared.Model.Protocols.ProtocolType.ModbusRtu,
            StopBits =  System.IO.Ports.StopBits.One
        };
        
        var content = new StringContent(JsonConvert.SerializeObject(rtu),Encoding.UTF8,Application.Json);
        var respon =await client.PostAsync("http://localhost:8090/api/test/TestHttpClient", content);
        return await respon.Content.ReadAsStringAsync();
    }

    [HttpPost]
    public async Task<ModbusRtu> TestHttpClient(ModbusRtu modbus)
    {
        await Task.Delay(1000);
        return modbus;
    }

}
```