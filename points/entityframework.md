>三种设计模式

code-first 、model-first、database-first

>UnitOfWork 设计模式

>Repository设计模式

>实体跟踪

五中状态：

Detached-游

UnChanged-没有变

Added-添加

Deleted-删除

Modified-编辑

Detached状态下的Entity不会被上下文（context）所捕获（track）


-->延伸问题 实体跟踪导致的多线程调用问题

* Attach

获取受跟踪的实体，默认获取的entity也是受跟踪的，Attach只是显示的调用
```csharp
var entry = _userDbContext.Users.Attach<User>(user);
```

使用方法：

如果想更新某条数据，又不想先进行一次查询，可以使用这样的方式进行更新

```csharp
var entry = _userDbContext.Users.Attach<User>(user);
entry.State = EntityState.Modified;
_userDbContext.SaveChanges();
```

但是，这样使用的话，如果用户不存在的情况下，会直接报异常，

* AsNoTracking

获取不受跟踪实体，只是用于查询数据的时候，使用该方式来获取数据，实体不受跟踪的时候，没办法进行更新和删除等操作
```csharp
var context = new Entities(connectStr);
var contentlist = context.Set<Content>().AsQueryable().AsNoTracking();

```

NoTracking得到的Detached实体有一个小问题。由NoTracking查询得到的实体和我们直接调用Detach方法得到的实体不同。前者内部会仍然保留一个对当前context的引用，以便在调用Load方法可以explicitly load相关的实体。而后者则完全脱离了相应的context，所以属于真正的Detached状态。细心的读者可能会觉得NoTracking得到的 Detached实体会导致context一直被引用，从而不能及时被垃圾处理（GC）。确实，这个被产品组证实是by design的，如果context不被引用，则explicit load则无法被支持

[NoTracking问题](https://social.msdn.microsoft.com/Forums/en-US/906c0cad-840b-4eb8-ba11-5348d407df73/notracking-quotmemory-leakquot?forum=adodotnetentityframework)