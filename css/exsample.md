>子元素浮动后，撑开父元素

使用`overflow:hidden`来清除浮动
```html
<div id="all1">

<div id="left1">1</div>

<div id="left2">1</div>
</div>
```
```css
.left1 {float:left;}
.left2 {float:left;}
.all1{ overflow:hidden;}
```

>绝对定位 position absolute

绝对定位经常使用`top`等样式来定位元素的位置，绝对定位的元素的位置相对于最近的已定位祖先元素，已定位祖先元素是指定位为absolute、relative、fixed的元素

```html
<div style="width:200px;margin: 40px">
  <div class="checkgroup">
    <span>类型</span>
    <div class="checkitem">
        <p>类型1</p>
        <input type="radio" name="Type" value="" data-key="1" checked="">
    </div>
    <div class="checkitem">
        <p>类型2</p>
        <input type="radio" name="Type" data-key="2" value="">
    </div>
    <div class="checkitem">
        <p>类型3</p>
        <input type="radio" name="Type" data-key="3" value="">
    </div>
  </div>
</div>
```

```css
.checkitem p, .checkitem input {
    display: inline-block;
}

.checkitem input {
    float: left;
    margin-right: 10px;
}

.checkgroup {
    border: 1px solid #C3C3C3;
    padding: 25px 20px 10px 20px;
    margin-bottom: 20px;
    position: relative;
}

.checkgroup > span {
    position: absolute;
    top: -10px;
    z-index: initial;
    background: white;
    padding: 0px 10px;
}
```

![效果](absolute.jpg)
> tooltip效果

<style>
  .tooltip .desc {
      height:20px;
      display: block;
      position:relative;
  }
  .tooltip .desc p {
    overflow: hidden;
    white-space: nowrap;
    text-overflow: ellipsis;
  }
  .tooltip .desc:before {
    content: attr(data-desc);
    display: none;
    /* border: 1px solid #C3C3C3; */
    position: absolute;
    bottom: 40px;
    background: black;
    z-index: 10000;
    opacity: 0.6;
    color: white;
    padding: 15px 15px;
    border-radius: 10px;
    width:100%;
    border-radius: 8px;
  }
  .tooltip .desc:after{
    content: "";
    display: none;
    position: absolute;
    border-top: 20px solid black;
    border-left: 15px solid transparent;
    bottom: 20px;
    left: 40px;
    opacity: 0.6;
    border-right: 15px solid transparent;
  }
  .tooltip .desc:hover:before {
    display: block;
  } 
  .tooltip .desc:hover:after {
    display: block;
  } 
</style>
<div class="tooltip" style="width:400px;margin: 120px auto auto 40px">
  <div class="desc" data-desc="描述：测试描述信息的信息的四季豆is金佛ID瑟吉欧if就打死傲娇浮动is阿奇偶if的数据OAif激动死傲娇佛ID是数据都筛分机度搜为金佛你打算">
    <p>描述：测试描述信息的信息的四季豆is金佛ID瑟吉欧if就打死傲娇浮动is阿奇偶if的数据OAif激动死傲娇佛ID是数据都筛分机度搜为金佛你打算</p>
  </div>
</div>