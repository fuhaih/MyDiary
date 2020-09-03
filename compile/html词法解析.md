
[html词法解析](https://blog.csdn.net/dlmu2001/article/details/5998130)

# HTMLToken

使用状态机把html解析为HTMLToken

html有六种token类型

* StartTag 起始标签

* EndTag 结束标签

* Character 元素内容 (需要处理特殊符号 )

* DOCTYPE 文档类型

* Comment 注释

* Uninitialized 默认类型

* EndOfFile 文档结束

* CDATA cdata类型 (常用在xml中，html没有意义，所以这里可以不考虑)

* Character references



每种类型的token可以看做是一个状态机。

```html
<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<!--comment -->

<html>

<body>

<a href=”w3c.org”>w3c</a>

</body>

</html>
```

上面html代码中包含了9个token，分别是文档类型声明，注释，html的起始标签，body的起始标签，a的起始标签，a的元素内容，a的介绍标签，body的结束标签，html的结束标签。

## DOCTYPE

>DataState：

`<!DOCTYPE`，碰到’<’，进入TagOpenState

>TagOpenState

`<!DOCTYPE`, 碰到’!’，进入MarkupDeclarationOpenState状态

>MarkupDeclarationOpenState：

`<!DOCTYPE`,碰到’D’，匹配DOCTYPE和--字数都不够，保持现状

>MarkupDeclarationOpenState：

`<!DOCTYPE`,匹配doctype，进入DOCTYPEState状态(HTMLToken的type为DOCTYPE)

>DOCTYPEState: 

`<!DOCTYPE html PUBL`，碰到空格，进入BeforeDOCTYPENameState状态

>BeforeDOCTYPENameState: 

`<!DOCTYPE html PUBL`，碰到’h’，进入DOCTYPENameState

>DOCTYPENameState: 

`<!DOCTYPE html PUBL`，碰到’t’,保持原状态，提取html作为文档类型

>DOCTYPENameState: 

`<!DOCTYPE html PUBL`，碰到空格，进入AfterDOCTYPENameState状态。(HTMLToken的m_data为html)

>AfterDOCTYPENameState：

`<!DOCTYPE html PUBLIC`，碰到’P’，还未能匹配Public或者system，保持状态

>AfterDOCTYPENameState：

`<!DOCTYPE html PUBLIC`,匹配public，进入AfterDOCTYPEPublicKeywordState

>AfterDOCTYPEPublicKeywordState：

`<!DOCTYPE html PUBLIC "-/`，碰到空格，进入BeforeDOCTYPEPublicIdentifierState

>BeforeDOCTYPEPublicIdentifierState：

`<!DOCTYPE html PUBLIC "-/`，碰到’”’，进入DOCTYPEPublicIdentifierDoubleQuotedState

>DOCTYPEPublicIdentifierDoubleQuotedState：

`<!DOCTYPE html PUBLIC "-/`，碰到’-‘，保持状态，提取m_publicIdentifier

>DOCTYPEPublicIdentifierDoubleQuotedState：

`<!DOCTYPE html PUBLIC "-/…nal//EN">`,碰到’”’,进入AfterDOCTYPEPublicIdentifierState状态。（HTMLToken的m_publicIdentifier确定）

>AfterDOCTYPEPublicIdentifierState：

`<!DOCTYPE html PUBLIC "-/…nal//EN">` ，碰到’>’,进入DataState状态，完成文档类型的解析

## COMMENT

>DataState：

`<!--comment -->`，碰到’<’，进入TagOpenState

>TagOpenState：

`<!--comment -->`, 碰到’!’，进入MarkupDeclarationOpenState状态

>MarkupDeclarationOpenState：

`<!--comment -->`,碰到’-’，匹配DOCTYPE和--字数都不够，保持现状

>MarkupDeclarationOpenState：

`<!--comment -->`,匹配--，进入CommentStartState状态(HTMLToken的type为COMMENT)

>CommentStartState: 

`<!--comment -->`,碰到’c’，进入CommentState

>CommentState：

`<!--comment -->`，碰到’-‘，进入CommentEndDashState状态(HTMLToken的m_data为comment)

>CommentEndDashState: 

`<!--comment -->`,碰到’-‘，进入CommentEndState状态

>CommentEndState：

`<!--comment -->`,碰到’>‘，进入DataState状态，完成解析。

## 起始标签a

>DataState：

`<a href=”w3c.org”>`，碰到’<’,进入TagOpenState状态

>TagOpenState：

`<a href=”w3c.org”>`，碰到’a’,进入TagNameState状态（HTMLToken的type为StartTag）

>TagNameState：

`<a href=”w3c.org”>`，碰到空格，进入BeforeAttributeNameState状态（HTMLToken的m_data为a）

>BeforeAttributeNameState：

`<a href=”w3c.org”>`，碰到‘h’,进入AttributeNameState状态

>AttributeNameState：

`<a href=”w3c.org”>`，碰到‘=’，进入BeforeAttributeValueState状态（HTMLToken属性列表中加入一个属性，属性名为href)

>BeforeAttributeValueState: 

`<a href=”w3c.org”>`，碰到‘“’，进入AttributeValueDoubleQuotedState状态

>AttributeValueDoubleQuotedState：

`<a href=”w3c.org”>`，碰到‘w’，保持状态，提取属性值

>AttributeValueDoubleQuotedState：

`<a href=”w3c.org”>`，碰到‘“’，进入AfterAttributeValueQuotedState(HTMLToken当前属性的值为w3c.org).

>AfterAttributeValueQuotedState: 

`<a href=”w3c.org”>`，碰到‘>’，进入DataState，完成解析。在完成startTag的解析的时候，会在解析器中存储与之匹配的end标签（m_appropriateEndTagName），等到解析end标签的时候，会同它进行匹配（语法解析的时候）。
html，body起始标签类似a起始标签，但没有属性解析

## a元素

>DataState：

`w3c</a>`，碰到’w’,维持原状态，提取元素内容(HTMLToken的type为character)。

>DataState：

`w3c</a>`，碰到’<’,完成解析，不consume ’<’。(HTMLToken的m_data为w3c)。

## a结束标签

>DataState：

`w3c</a>`，碰到’<’,进入TagOpenState。

>TagOpenState：

`w3c</a>`，碰到’/’,进入到EndTagOpenState。（HTMLToken的type为endTag）。

>EndTagOpenState：

`w3c</a>`，碰到’a’,进入到TagNameState。

>TagNameState：

`w3c</a>`，碰到’>’,进入到DataState，完成解析。

# html document

状态



