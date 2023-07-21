```csharp
csharp
using System;
using System.IO;
using System.Net;
using System.ServiceModel.Description;

class Program
{
static void Main(string[] args)
{
// WSDL地址
string wsdlUrl = "http://example.com/your-service?wsdl";

    try
    {
        // 创建MetadataExchangeClient实
        MetadataExchangeClient mexClient = new MetadataExchangeClient(new Uri(wsdlUrl), MetadataExchangeClientMode.HttpGet);

        // 获取元数据
        MetadataSet metadata = mexClient.GetMetadata();

        // 将元数据写入流
        using (MemoryStream stream = new MemoryStream())
        {
            using (StreamWriter writer = new StreamWriter(stream))
            {
                metadata.WriteTo(writer);
                writer.Flush();
                stream.Position = 0;

                // 读取流内容并构建POST请求的消息体
                using (StreamReader reader = new StreamReader(stream))
                {
                    string postBody = reader.ReadToEnd();
                    Console.WriteLine(postBody);
                }
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error: " + ex.Message);
    }

    Console.ReadLine();
}
}
```

```csharp
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Net;
using System.Reflection;
using System.Web.Services.Description;

class Program
{
    static void Main(string[] args)
    {
        WSDL 文件的 URL
        string wsdlUrl = "http://example.com/your_wsdl_file.wsdl";

        // 创建一个 WebClient 对象，下载 WSDL 文件
        WebClient webClient = new WebClient();
        Stream wsdlStream = webClient.OpenRead(wsdlUrl);

        // 创建一个 ServiceDescription 对象，从 WSDL 文件中读取描述信息
        ServiceDescription serviceDescription = ServiceDescription.Read(wsdlStream);

        // 创建一个 CodeNamespace 对，用于存储生成的代码
        CodeNamespace codeNamespace = new CodeNamespace("GeneratedNamespace");

        // 创建一个 CodeCompileUnit 对象，表示整个编译单元
        CodeCompileUnit codeCompileUnit = new CodeCompileUnit();
        codeCompileUnit.Namespaces.Add(codeNamespace);

        // 创建一个 WsdlImporter 对象，用导入 WSDL 描述信息
        WsdlImporter wsdlImporter = new WsdlImporter(serviceDescription);

        // 导入所有的服务协定（Service Contracts）
        ServiceContractGenerator contractGenerator = new ServiceContractGenerator();
        wsdlImporter.ImportAllContracts(contractGenerator);

        // 生成客户端代类的代码
        foreach (CodeTypeDeclaration codeType in contractGenerator.GenerateCode())
        {
            codeNamespace.Types.Add(codeType);
        }

        // 设置代码生成选项
        CodeGeneratorOptions options = new CodeGeneratorOptions();
        optionsacingStyle = "C";

        // 创建一个 StringWriter 对象，用于生成的代码写字符串中
        StringWriter stringWriter = new StringWriter();

        // 创建一个 C# 代码提供程序
        CodeDomProvider codeProvider = CodeDomProvider.CreateProvider("C#");

        // 将生成的代码写入字符串中
        codeProvider.GenerateCodeFromCompileUnit(codeCompileUnit, stringWriter, options);

        // 获取生成的代码字符串
        string generatedCode = stringWriter.ToString();

        // 输出生成的代码
        Console.WriteLine(generatedCode);

        // 关闭流 WebClient 对象
        wsdlStream.Close();
        webClient.Dispose();
    }
}
```

```csharp
using System;
using System.IO;
using System.Netusing System.ServiceModel.Description;
using System.Text;
using System.Xml;

class Program
{
 static void Main(string[] args)
    {
        // W 文件的 URL
        string wsdlUrl = "http://example.com/your-wsdl-file.wsdl";

        // 创建 ServiceDescription 对象
        ServiceDescription serviceDescription = ServiceDescription.Read(wsdl);

        // 遍历个服务
        foreach (Service service in serviceDescription.Services)
        {
            // 遍历每个端点
            foreach (Port port in service.Ports)
            {
                // 获取绑定和操作信息
                Binding binding = port.Binding;
                PortType portType = serviceDescription.PortTypes[binding.Type.Name];

                // 遍历每个操作
                foreach (Operation operation in portType.Operations)
                {
                    // 构造请求体
                    string requestBody = ConstructRequestBody(operation);

                    // 发送 POST 请求
                    SendPostRequest(requestBody);
                }
            }
        }

        Console.WriteLine("All requests sent.");
        Console.ReadLine();
    }

    static string ConstructRequestBody(Operation operation)
    {
        StringBuilder sb = new StringBuilder();

        // 添加 SOAP Envelope 头部
        sb.AppendLine("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" " +
                      "xmlns:web=\"http://example.com/your-web-namespace\">");
        sb.AppendLine("<soapenv:Header/>");
        sb.AppendLine("<soapenv:Body>");

        // 添加操作名称
        sb.AppendLine($"<web:{operation.Name}>");

        // 添加操作参数（如果）
        foreach (OperationMessage message in operation.Messages)
        {
            MessagePart part = message.Parts.Values.FirstOrDefault();
            if (part != null)
            {
                sb.AppendLine($"<{part.Name}>VALUE</{part.Name}>");
            }
        }

        // 关闭操作标签和 SOAP Envelope 标签
       .AppendLine($"</web:{operation.Name}>");
        sb.AppendLine("</soapenv:Body>");
        sb.AppendLine("</soapenv:Envelope>");

        return sb.ToString();
    }

    static void SendPostRequest(string requestBody)
    {
        string endpointUrl = "http://example.com/your-endpoint-url";

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endpointUrl);
        request.Method = "POST";
        request.ContentType = "text/xml;charset=utf-8";

        using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
        {
            writer.Write(requestBody);
        }

        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        {
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string responseText = reader.ReadToEnd();
                Console.WriteLine(responseText);
            }
        }
    }
}
```