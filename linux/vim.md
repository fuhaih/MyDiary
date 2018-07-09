# 常用功能
## vim常用命令
在命令行模式下进行操作

### i 进入编辑状态
在编辑状态下按ctrl+s键的时候，vim会进入停止输出的状态，这时候不能使用命令行，也不能进行编辑，ctrl+q就能解除该状态
### ESC 退出编辑状态
在编辑状态下，按ESC键能退出编辑状态
### :q 退出不保存
### :wq 保存退出
### :q! 强制退出
### /keyword 进行关键字搜索
命令行输入/keyword后回车，光标会指向第一个匹配的文本，按N/n查看下一个匹配


## vim非正常退出
vim在编辑的时候会生成一个.filename.swp的文件，如在编辑/etc/mongod.conf文件时，如果异常退出，会生产一个/etc/.mongod.conf.swp的文件，如果时正常退出，会自动删除该问题。
异常退出恢复就是靠该文件：
```vim shell
# 异常退出恢复
vim -r /etc/mongod.conf

# 恢复完成后可以删除swp文件
rm /etc/.mongod.conf.swp
```
