>cas 单点登录系统


>有关ticket

ticket是在CAS Sserver重定向时携带的，并不需要保密性，在app server通过ticket获取登录信息时，检测ticket的来源ip是否对应就行了。

>cas 代理模式

多个有cas逻辑的app server间内部调用时，会直接跳转到cas系统
cas代理模式是解决这个问题。