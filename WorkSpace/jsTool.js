﻿/*
*json格式的datetime转为js格式的datetime
*/
function timeJson2Date(time) {
  var regex = new RegExp("/Date\\((-?\\d+)\\)/");
  var matchs = regex.exec(json);
  var mseconds = parseInt(matchs[1]);
  return new Date(mseconds);
}

// 根据format把time字符串转换为Date对象
//把format转换为正则表达式
//yyyy-MM-dd hh:mm:ss 转换为 
//(\d{4})-\d{2}-\d{2} \d{2}:\d{2}:\d{2} RegExp.$1能获取到year
//\d{4}-(\d{2})-\d{2} \d{2}:\d{2}:\d{2} RegExp.$1能获取到month
//\d{4}-\d{2}-(\d{2}) \d{2}:\d{2}:\d{2} RegExp.$1能获取到day
//\d{4}-\d{2}-\d{2} (\d{2}):\d{2}:\d{2} RegExp.$1能获取到hour
//\d{4}-\d{2}-\d{2} \d{2}:(\d{2}):\d{2} RegExp.$1能获取到minute
//\d{4}-\d{2}-\d{2} \d{2}:\d{2}:(\d{2}) RegExp.$1能获取到second
//\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}
const parse_format = function(time,format){
  var o = {
    year:"yyyy",
    month:"MM",
    day:"dd",
    hour:"hh",
    minute:"mm",
    second:"ss",
    msecond:"SSS"
  };
  var data ={};
  for(var k in o){
    var fmt = format;
    for(var c in o){
      if (new RegExp("(" + o[c] + ")").test(fmt)){
        if(k==c){
          fmt = fmt.replace(RegExp.$1,"(\\d{"+RegExp.$1.length+"})");
        }else{
          fmt = fmt.replace(RegExp.$1,"\\d{"+RegExp.$1.length+"}");
        }
      }
    }
    if(new RegExp(fmt).test(time)&&RegExp.$1!=''){
      data[k] = parseInt(RegExp.$1);
    }else{
      data[k] = 0;
    }
  }
  return new Date(data.year,data.month,data.day,data.hour,data.minute,data.second,data.msecond);
}

// 对Date的扩展，将 Date 转化为指定格式的String
// 月(M)、日(d)、小时(h)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符， 
// 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字) 
// 例子： 
// (new Date()).Format("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423 
// (new Date()).Format("yyyy-M-d h:m:s.S")      ==> 2006-7-2 8:9:4.18 
Date.prototype.Format = function (fmt) { //author: meizz 
    var o = {
        "M+": this.getMonth() + 1, //月份 
        "d+": this.getDate(), //日 
        "h+": this.getHours(), //小时 
        "m+": this.getMinutes(), //分 
        "s+": this.getSeconds(), //秒 
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
        "S": this.getMilliseconds() //毫秒 
    };
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}

Date.prototype.addHours = function (h) {
    this.setHours(this.getHours() + h);
};

Date.prototype.addDays = function (d) {
    this.setDate(this.getDate() + d);
};

Date.prototype.addWeeks = function (w) {
    this.addDays(w * 7);
};

Date.prototype.addMonths = function (m) {
    var d = this.getDate();
    this.setMonth(this.getMonth() + m);
    if (this.getDate() < d)
        this.setDate(0);
};

Date.prototype.addYears = function (y) {
    var m = this.getMonth();
    this.setFullYear(this.getFullYear() + y);
    if (m < this.getMonth()) {
        this.setDate(0);
    }
};

function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null)
        return unescape(r[2]);
    return null;
}

String.prototype.format = function () {
    var args = arguments;
    return this.replace(/{(\d+)}/g, function (match, number) {
	//number为分组里的内容
        return typeof args[number] != 'undefined'
          ? args[number]
          : match
        ;
    });
};

var getRandomColor = function () {
    return '#' + (function (color) {
        //这个写法比较有意思,Math.floor(Math.random()*16);返回的是一个小于或等于16的数.然后作为0123456789abcdef的下标,这样每次就会得到一个这个字符串当中的一个字符
        return (color += '0123456789abcdef'[Math.floor(Math.random() * 16)])
            //然后判断这个新字符串的长度是否到6,因为16进制的颜色是由6个字符组成的,如果到6了,就返回这6个字符拼成的字符串,如果没有就执行arguments.callee(color)也就是函数本身.
            && (color.length == 6) ? color : arguments.callee(color); //将''字符串传给color
    })('');
}

/*
* 把字符串转换为适用于.net的unicode编码
*/
String.prototype.ToUnicodeBytes = function () {
    var ch, re = [];
    for (var i = 0; i < this.length; i++) {
        ch = this.charCodeAt(i);  // get char   
        if (ch > 255)// set up "stack"  
        {
            re.push(ch & 0xFF);  // push byte to stack  
            ch = ch >> 8;          // shift value down by 1 byte  
            re.push(ch & 0xFF);
        }
        else {
            re.push(ch & 0xFF);
            re.push(0);
        }
    }
    // return an array of bytes  
    return re;
}

/*
* 把unicode编码转换为字符串
*/
UnicodeToString = function (array) {
    if (array.length & 1 == 1) return;
    var charCode = [];
    for (var i = 0; i < array.length; i = i + 2) {
        var code = array[i + 1];
        code = code << 8;
        code = code | array[i];
        var str = String.fromCharCode(code)
        charCode.push(str);
    }
    var res = charCode.join('');
    return res;
}

/** 
*@param {string} url 完整的URL地址 
*@returns {object} 自定义的对象 
*@description 用法示例：var myURL = parseURL('http://abc.com:8080/dir/index.html?id=255&m=hello#top'); 

myURL.file='index.html' 

myURL.hash= 'top' 

myURL.host= 'abc.com:8081' 

myURL.hostname= 'abc.com' 

myURL.query= '?id=255&m=hello' 

myURL.params= Object = { id: 255, m: hello } 

myURL.path= '/dir/index.html' 

myURL.segments= Array = ['dir', 'index.html'] 

myURL.port= '8081' 

myURL.protocol= 'http' 

myURL.source= 'http://abc.com:8080/dir/index.html?id=255&m=hello#top' 

*/
function parseURL(url) {
    var a = document.createElement('a');
    a.href = url;
    return {
        source: url,
        protocol: a.protocol.replace(':', ''),
        hostname: a.hostname,
        host: a.host,
        port: a.port,
        query: a.search,
        params: (function () {
            var ret = {},
                seg = a.search.replace(/^\?/, '').split('&'),
                len = seg.length, i = 0, s;
            for (; i < len; i++) {
                if (!seg[i]) { continue; }
                s = seg[i].split('=');
                ret[s[0]] = s[1];
            }
            return ret;
        })(),
        file: (a.pathname.match(/\/([^\/?#]+)$/i) || [, ''])[1],
        hash: a.hash.replace('#', ''),
        path: a.pathname.replace(/^([^\/])/, '/$1'),
        relative: (a.href.match(/tps?:\/\/[^\/]+(.+)/) || [, ''])[1],
        segments: a.pathname.replace(/^\//, '').split('/')
    };
}

/*
*iframe自适应子页面内容
*@down：iframe元素ID
*/
function dyniframesize(down) {
    var pTar = null;
    if (document.getElementById) {
        pTar = document.getElementById(down);
    }
    else {
        eval('pTar = ' + down + ';');
    }
    if (pTar && !window.opera) {
        //begin resizing iframe 
        pTar.style.display = "block"
        if (pTar.contentDocument && pTar.contentDocument.body.offsetHeight) {
            //ns6 syntax 
            pTar.height = pTar.contentDocument.body.scrollHeight + 20;
            //pTar.height = pTar.contentDocument.documentElement.scrollHeight + 20;
            //pTar.width = pTar.contentDocument.body.scrollWidth + 20;
        }
        else if (pTar.Document && pTar.Document.body.scrollHeight) {
            //ie5+ syntax 
            pTar.height = pTar.Document.body.scrollHeight;
            pTar.width = pTar.Document.body.scrollWidth;
        }
    }
}


function getBrowser()
{
    var userAgent = navigator.userAgent,
    ua = userAgent.toLowerCase(), 
    rMsie = /(msie\s|trident.*rv:)([\w.]+)/,
    rFirefox = /(firefox)\/([\w.]+)/,
    rOpera = /(opera).+version\/([\w.]+)/,
    rChrome = /(chrome)\/([\w.]+)/,
    rSafari = /version\/([\w.]+).*(safari)/;
    var match = rMsie.exec(ua);
    if (match != null) {
        return { browser: "IE", version: match[2] || "0" };
    }
    var match = rFirefox.exec(ua);
    if (match != null) {
        return { browser: match[1] || "", version: match[2] || "0" };
    }
    var match = rOpera.exec(ua);
    if (match != null) {
        return { browser: match[1] || "", version: match[2] || "0" };
    }
    var match = rChrome.exec(ua);
    if (match != null) {
        return { browser: match[1] || "", version: match[2] || "0" };
    }
    var match = rSafari.exec(ua);
    if (match != null) {
        return { browser: match[2] || "", version: match[1] || "0" };
    }
    if (match != null) {
        return { browser: "", version: "0" };
    }
}

//给不支持forEach的浏览器数组添加forEach函数
if (!Array.prototype.forEach) {

    Array.prototype.forEach = function forEach(callback, thisArg) {

        var T, k;

        if (this == null) {
            throw new TypeError("this is null or not defined");
        }
        var O = Object(this);
        var len = O.length >>> 0;
        if (typeof callback !== "function") {
            throw new TypeError(callback + " is not a function");
        }
        if (arguments.length > 1) {
            T = thisArg;
        }
        k = 0;

        while (k < len) {

            var kValue;
            if (k in O) {

                kValue = O[k];
                callback.call(T, kValue, k, O);
            }
            k++;
        }
    };
}