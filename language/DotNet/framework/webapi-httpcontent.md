>ByteRangeStreamContent

这个是Range请求时使用

```csharp
if (Request.Headers.Range != null)
{
    HttpResponseMessage partialResponse = Request.CreateResponse(HttpStatusCode.PartialContent);
    partialResponse.Content = new ByteRangeStreamContent(stream, Request.Headers.Range, type, 3 * 1024 * 1024);
    return partialResponse;
}
else {
    HttpResponseMessage fullResponse = new HttpResponseMessage(HttpStatusCode.OK);
    fullResponse.Content = new StreamContent(stream,3 * 1024 * 1024);
    fullResponse.Content.Headers.ContentType = new MediaTypeHeaderValue(type);
    fullResponse.Headers.AcceptRanges.Add("bytes");
    return fullResponse;
}
```

[文章](https://devblogs.microsoft.com/aspnet/asp-net-web-api-and-http-byte-range-support/)
>StringContent

文本格式(text、json等都行)

```csharp
string data = JsonConvert.SerializeObject(parameter);
var content = new StringContent(data);
content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
var respon = await client.PostAsync(url, content);
```

>MultipartFormDataContent

`multipart/form-data`对应的Content

```csharp
var content = new MultipartFormDataContent();
content.Add(new StringContent(name), "name");
content.Add(new StringContent(sourceid), "sourceid");
content.Add(new StreamContent(stream),"file", filename);
var respon = await client.PostAsync(url, content);
```

>FormUrlEncodedContent

`application/x-www-form-urlencoded`对应的Content

```csharp
List<KeyValuePair<string, string>> parameter = new List<KeyValuePair<string, string>>() {
    new KeyValuePair<string, string>("user","admin"),
    new KeyValuePair<string, string>("pwd","admin")
};
FormUrlEncodedContent content = new FormUrlEncodedContent(parameter);
var respon = await client.PostAsync(url, content);
```
>StreamContent

返回流