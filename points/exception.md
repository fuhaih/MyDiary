## try catch的使用

需要看业务需求，一般web项目的话使用过滤器或者中间件就行了，但是不要过度依赖过滤器，也就是不要随便手动抛出异常，很多时候也是可以不使用异常来处理。

使用try catch的情况一般是需要处理异常，比如说使用事务的时候，需要在异常发生时做回滚操作

有些业务捕获异常后不需要处理，比如很多后台业务，需要在当前任务异常时，不影响后续任务进行，这时候可以使用try catch捕获异常，打印日志记录异常就行了