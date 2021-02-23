## 盒子模型

> box-sizing

> margin

**margin共享问题**

所有毗邻的两个或更多盒元素的margin将会合并为一个margin共享之。 毗邻的定义为：同级或者嵌套的盒元素，并且它们之间没有非空内容、Padding或Border分隔。

margin-left和margin-right不存在这种情况，可能是设置其中一个值时，另一个值也固定了，所以不会出现margin-left和margin-right共享问题


解决嵌套的父子元素margin共享办法

* 父级或子元素使用浮动或者绝对定位absolute（浮动或绝对定位不参与margin的折叠）
* 父级overflow:hidden;
* 父级设置padding（破坏非空白的折叠条件）
* 父级设置border（破坏非空白的折叠条件）

解决毗邻元素margin共享问题

* 使用浮动解决
* 设置border

>float

float会导致父元素高度塌陷

解决方法：



> border

> padding

* content


>display

***

## 定位

> position

* fixed 

相对于窗口进行定位，这个比较适合使用在导航栏和页脚这样要在窗口中进行定位的元素

* absolute

相对于最近position不为static的父级元素来定位，使用top，left，right，bottom这几个元素来进行定位，定位的位置实际上还和父级元素的box-sizing有关

* relative	

生成相对定位的元素，相对于其正常位置进行定位。

因此，"left:20" 会向元素的 LEFT 位置添加 20 像素。

如果使用多个relative元素，他们也都是相对于原来的位置进行的定位偏移

[理解例子](demo/relative.html)

假设content1，content2，content other三个元素没有设置`position: relative;`那么这三个元素会紧挨着，并且content1是挨着body顶部的，再给content1，content2设置`position: relative;`并且设置`top: 20px;` 那么content1，content2这两个元素会相对于原来的位置向下偏移20个像素，而content other元素没有偏移，还在原来的位置上，那么content2就会覆盖掉content other一部分内容。

* static
 
 默认值。没有定位，元素出现在正常的流中（忽略 top, bottom, left, right 或者 z-index 声明）。

 * inherit 

 规定应该从父元素继承 position 属性的值
***

## 其他基本元素

>height

>width

>background-color

>color

>transform
***

## 字体
***

## 表格
***

## 动画

>animation

>@keyframes 

***



## 伪类
***


## 伪元素

## 一些常常有坑的标签

有坑一般是因为一些元素默认会有一些css属性，导致在使用时与预想的不一样

>body

body 默认css属性

```css
margin: 8px
```


>p

p标签默认css属性

```css
display: block;
margin-block-start: 1em;
margin-block-end: 1em;
margin-inline-start: 0px;
margin-inline-end: 0px;
```

margin-top = margin-block-start

margin-right = margin-inline-end

margin-bottom = margin-block-end

margin-left = margin-inline-start


在使用的时候一般会把html、body等元素的margin和padding都设置为0

```css
html,body,p{
  margin:0px;
  padding:0px;
}

```




