# jquery ajax不支持接收二进制流数据
## 原因
jquery尚未支持HTML5 XMLHttpRequest Level 2的二进制流数据请求。
## 解决方案
[github地址](https://github.com/henrya/js-jquery/tree/master/BinaryTransport)

jquery 允许我们用 Ajax transportsa方法来自定义ajax请求
```javascript
 /**
 *
 * jquery.binarytransport.js
 *
 * @description. jQuery ajax transport for making binary data type requests.
 * @version 1.0 
 * @author Henry Algus <henryalgus@gmail.com>
 *
 */
// use this transport for "binary" data type
$.ajaxTransport("+binary", function(options, originalOptions, jqXHR){
    // check for conditions and support for blob / arraybuffer response type
    if (window.FormData && ((options.dataType && (options.dataType == 'binary')) || (options.data && ((window.ArrayBuffer && options.data instanceof ArrayBuffer) || (window.Blob && options.data instanceof Blob)))))
    {
        return {
            // create new XMLHttpRequest
            send: function(headers, callback){
		// setup all variables
                var xhr = new XMLHttpRequest(),
		url = options.url,
		type = options.type,
		async = options.async || true,
		// blob or arraybuffer. Default is blob
		dataType = options.responseType || "blob",
		data = options.data || null,
		username = options.username || null,
		password = options.password || null;
					
                xhr.addEventListener('load', function(){
			var data = {};
			data[options.dataType] = xhr.response;
			// make callback and send data
			callback(xhr.status, xhr.statusText, data, xhr.getAllResponseHeaders());
                });
 
                xhr.open(type, url, async, username, password);
				
		// setup custom headers
		for (var i in headers ) {
			xhr.setRequestHeader(i, headers[i] );
		}
				
                xhr.responseType = dataType;
                xhr.send(data);
            },
            abort: function(){
                jqXHR.abort();
            }
        };
    }
});
```

在调用的时候，processData 必须要设置为false，不允许解析数据
```javascript
$.ajax({
  url: "/my/image/name.png",
  type: "GET",
  dataType: "binary",
  //responseType:'arraybuffer'
  processData: false,
  success: function(result){
	  // do something with binary data
  }
});
```
还可以自定义请求头
```javascript
$.ajax({
          url: "image.png",
          type: "GET",
          dataType: 'binary',
          headers:{'Content-Type':'image/png','X-Requested-With':'XMLHttpRequest'},
          processData: false,
          success: function(result){
          }
}); 
```
