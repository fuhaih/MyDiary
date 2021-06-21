# 1 HttpClient

## 1.1 IHttpClientFactory 和 HttpClient MaxConnectionsPerServer

## 1.2 HttpContent

### 1.2.1 StringContent

文本格式(text、json等都行)

```csharp
string data = JsonConvert.SerializeObject(parameter);
var content = new StringContent(data);
content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
var respon = await client.PostAsync(url, content);
```


### 1.2.2 MultipartFormDataContent

`multipart/form-data`对应的Content

```csharp
var content = new MultipartFormDataContent();
content.Add(new StringContent(name), "name");
content.Add(new StringContent(sourceid), "sourceid");
content.Add(new StreamContent(stream),"file", filename);
var respon = await client.PostAsync(url, content);
```

### 1.2.3 FormUrlEncodedContent

`application/x-www-form-urlencoded`对应的Content

```csharp
List<KeyValuePair<string, string>> parameter = new List<KeyValuePair<string, string>>() {
    new KeyValuePair<string, string>("user","admin"),
    new KeyValuePair<string, string>("pwd","admin")
};
FormUrlEncodedContent content = new FormUrlEncodedContent(parameter);
var respon = await client.PostAsync(url, content);
```
### 1.2.4 StreamContent

返回流


