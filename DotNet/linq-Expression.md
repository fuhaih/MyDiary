# [常用方法](https://msdn.microsoft.com/zh-cn/library/system.linq.expressions.expression.aspx)

>一个简单例子
```csharp
//定义一个变量i
ParameterExpression variableExpr = Expression.Variable(typeof(int), "i");
//给变量i赋值 i=17
Expression assignExpr = Expression.Assign(
                variableExpr,
                Expression.Constant(17)
            );
//代码块 {} 如果有返回值，最后一个Expression作为返回值
Expression blockExpr = Expression.Block(
new ParameterExpression[] { variableExpr },
assignExpr,
variableExpr
);
//生成表达式 ()=>{int i=17;return i;}
Expression.Lambda<Func<int>>(blockExpr).Compile()          
```

> Constant 常量

> Variable 变量

> Assign 赋值操作

> Block 代码块

> Lambda 生成Lambda表达式

> 比较
```csharp
//定义参数
ParameterExpression input = Expression.Parameter(typeof(int), "input");
//比较表达式 input>5
BinaryExpression btweenExpr1 = Expression.GreaterThan(input, Expression.Constant(5));
//比较表达式input<10
BinaryExpression btweenExpr2 = Expression.LessThan(input, Expression.Constant(10));
// 与操作表达式 input<5 & input>10
BinaryExpression btweenExpr = Expression.And(btweenExpr1, btweenExpr2);
//生成表达式 (input)=> input<5 & input>10
Expression.Lambda<Func<int,bool>>(btweenExpr, input).Compile();
```
> TypeAs 显示转换，只适用于引用类型

> Unbox 拆箱，拆箱只适用于值类型

>Property
```csharp
MemberExpression memberExpression = Expression.Property(input, "Id");
```

> Parameter 参数
```csharp
//定义参数
ParameterExpression input = Expression.Parameter(typeof(string), "input");
MethodInfo method= typeof(string).GetMethod("ToUpper", new Type[] { });
//参数调用ToUpper() 方法
MethodCallExpression methodExpr = Expression.Call(input, method);
//由于这里只有一个表达式，所以也可以不用代码块括起来
Expression blockExpr = Expression.Block(
    methodExpr
);
//生成带参数的lambda表达式 (input)=>{return input.ToUpper()}
Expression.Lambda<Func<string,string>>(blockExpr, input).Compile();
//生成带参数的lambda表达式 (input)=>input.ToUpper();
Expression.Lambda<Func<string,string>>(methodExpr, input).Compile();
```
> Add 不进行溢出检测的计算
```csharp
//定义一个变量i
ParameterExpression variableExpr = Expression.Variable(typeof(int), "i");
//给变量i赋值 i=17
Expression assignExpr = Expression.Assign(
    variableExpr,
    Expression.Constant(17)
);
//创建一个加法表达式 i+17；
BinaryExpression addExpr= Expression.Add(variableExpr, Expression.Constant(17));

//把表达式结果赋值给变量 i=i+17;
Expression assignAddExpr = Expression.Assign(
    variableExpr,
    addExpr
);
//某个变量进行自身的add操作时，可以用AddAssign来替换 Add 、Assign这两个步骤
//代码块 {} 如果有返回值，最后一个Expression作为返回值
Expression blockExpr = Expression.Block(
new ParameterExpression[] { variableExpr },
    assignExpr,
    assignAddExpr,
    variableExpr
);
//生成表达式 ()=>{int i=17;i=i+17;return i;}
Expression.Lambda<Func<int>>(blockExpr).Compile()         
```

>ArrayIndex 根据数组下标取值
```csharp
//定义一个变量i
ParameterExpression variableExpr = Expression.Variable(typeof(int[]), "i");
//给变量i赋值 i=new int[] { 17,18}
Expression assignExpr = Expression.Assign(
    variableExpr,
    Expression.Constant(new int[] { 17,18})
);
//取 i[1]值
BinaryExpression indexExpr = Expression.ArrayIndex(variableExpr, Expression.Constant(1));

//代码块 {} 如果有返回值，最后一个Expression作为返回值
Expression blockExpr = Expression.Block(
new ParameterExpression[] { variableExpr },
    assignExpr,
    indexExpr
);
//生成表达式 ()=>{int i=new int[] { 17,18}；return i[1];}
Expression.Lambda<Func<int>>(blockExpr).Compile()        
```

> Call 调用方法
```csharp
ParameterExpression variableExpr = Expression.Variable(typeof(int), "i");
//给变量i赋值 i=new int[] { 17,18}
Expression assignExpr = Expression.Assign(
    variableExpr,
    Expression.Constant(17)
);
//获取ToString方法信息,后面是方法参数类型，现在想获取的是无参的ToString，
//所以传递一个空类型数组
MethodInfo methrod= typeof(int).GetMethod("ToString", new Type[] { });
//让参数i调用方法 i.ToString()
MethodCallExpression methodExpr= Expression.Call(variableExpr, methrod);

//代码块 {} 如果有返回值，最后一个Expression作为返回值
Expression blockExpr = Expression.Block(
new ParameterExpression[] { variableExpr },
    assignExpr,
    methodExpr
);
//生成表达式 ()=>{int i=17;return i.ToString();}
Expression.Lambda<Func<string>>(blockExpr).Compile()
```

# 问题

>PropertyExpression

该类是内部类，只能dll内部使用，需要构造一个用到属性的表达式时，用MemberExpressiont替换