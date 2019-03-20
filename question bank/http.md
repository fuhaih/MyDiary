# 使用HttpWebRequest模仿form文件上传。

form提交时的数据[格式](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Disposition)。是通过boundary来进行数据分割的。
```http
POST /test.html HTTP/1.1
Host: example.org
Content-Type: multipart/form-data;boundary="boundary"

--boundary
Content-Disposition: form-data; name="field1"

value1
--boundary
Content-Disposition: form-data; name="field2"; filename="example.txt"

value2
--boundary--
```

c#代码
```csharp
public async Task<string> UploadFile(string filepath)
{
    string url = "http://localhost:8889/api/upload";
    HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);
    var boundary = "---------------" + DateTime.Now.Ticks.ToString("x");
    //webrequest.CookieContainer = cookies;
    webrequest.ContentType = "multipart/form-data; boundary="+ boundary;
    webrequest.Method = "POST";
    Stream requestStream = webrequest.GetRequestStream();         
    string postHeader = "\r\n--"+ boundary + "\r\nContent-Disposition: form-data; name=\"field1\"; filename=\"example.xlsx\"\r\nContent-Type: application/octet-stream\r\n\r\n"; 
    byte[] postHeaderBytes = Encoding.ASCII.GetBytes(postHeader);
    await requestStream.WriteAsync(postHeaderBytes, 0, postHeaderBytes.Length);
    byte[] filebuffer = await ReadFile(filepath);
    await requestStream.WriteAsync(filebuffer, 0, filebuffer.Length);
    string postFooter = "\r\n--" + boundary + "--\r\n";
    byte[] postFooterBytes= Encoding.ASCII.GetBytes(postFooter);
    await requestStream.WriteAsync(postFooterBytes, 0, postFooterBytes.Length);
    HttpWebResponse respon = (HttpWebResponse)await webrequest.GetResponseAsync();
    AjaxResult<Link> link =await GetViewLink<AjaxResult<Link>>(respon);
    string emptylink = GetEmptyLink();
    string viewlink = link.State == 0 ? emptylink : link.Data.Url;
    return viewlink;
}
```
