# c# 垃圾回收
Finalize 和 Dispose() 和Dispose(bool disposing)
[来源](http://www.cnblogs.com/caomao/archive/2005/05/10/152505.html)
* Finalize是CRL提供的一个机制, 它保证如果一个类实现了Finalize方法,那么当该类对象被垃圾回收时,垃圾回收器会调用Finalize方法.而该类的开发者就必须在Finalize方法中处理 非托管资源的释放. 但是什么时候会调用Finalize由垃圾回收器决定,该类对象的使用者(客户)无法控制.从而无法及时释放掉宝贵的非托管资源.由于非托管资源是比较宝贵了,所以这样会降低性能.
* Dispose(bool disposing)不是CRL提供的一个机制, 而仅仅是一个设计模式(作为一个IDisposable接口的方法),它的目的是让供类对象的使用者(客户)在使用完类对象后,可以及时手动调用非托管资源的释放,无需等到该类对象被垃圾回收那个时间点.这样类的开发者就只需把原先写在Finalize的释放非托管资源的代码,移植到Dispose(bool disposing)中.  而在Finalize中只要简单的调用 "Dispose(false)"(为什么传递false后面解释)就可以了.
* 为什么还需要一个Dispose()方法?难道只有一个Dispose(bool disposing)或者只有一个Dispose()不可以吗? 

        答案是:
        只有一个Dispose()不可以. 为什么呢?因为如果只有一个Dispose()而没有Dispose(bool disposing)方法.那么在处理实现非托管资源释放的代码中无法判断该方法是客户调用的还是垃圾回收器通过Finalize调用的.无法实现 判断如果是客户手动调用,那么就不希望垃圾回收器再调用Finalize()(调用GC.SupperFinalize方法).另一个可能的原因(:我们知道如果是垃圾回收器通过Finalize调用的,那么在释放代码中我们可能还会引用其他一些托管对象,而此时这些托管对象可能已经被垃圾回收了, 这样会导致无法预知的执行结果(千万不要在Finalize中引用其他的托管对象). 
        所以确实需要一个bool disposing参数, 但是如果只有一个Dispose(bool disposing),那么对于客户来说,就有一个很滑稽要求,Dispose(false)已经被Finalize使用了,必须要求客户以Dispose(true)方式调用,但是谁又能保证客户不会以Dispose(false)方式调用呢?所以这里采用了一中设计模式:重载  把Dispose(bool disposing)实现为 protected, 而Dispose()实现为Public,那么这样就保证了客户只能调用Dispose()(内部调用Dispose(true)//说明是客户的直接调用),客户无法调用Dispose(bool disposing).