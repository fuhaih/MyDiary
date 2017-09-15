# GitHelper
## 入门
准备工作：
* 申请一个github账户
* 下载git for windows

### **安装git for windows**
在windows中安装git for windows，安装的时候，在选完安装路径之后，一直点击下一步就行了

### **基本配置**
    
    git config --global user.name "你的用户名" #配置用户名

    git config --global user.email "你的邮箱" #配置邮箱

### **ssh配置**
    ssh-keygen -t rsa -C "你的邮箱" #生成ssh

    clip < ~/.ssh/id_rsa.pub #复制ssh 复制完成之后把ssh添加到github中

    ssh -T git@github.com #测试ssh连接 

### **初始化本地仓库**
    git init

### **配置远程仓库**
    git remote add origin 你复制的地址
    #地址是以git@github.com开头的，一般git@github.com:username/origin.git

### **添加仓库中所有文件到缓存库**
    git add --all　 #添加到缓存库

    git status   #查看添加状态

    git commit -m 'add solution code'    #提交更改

### **同步好仓库之后可以上传到远程仓库**

    git push origin master
### **获取远程仓库更新**
    git pull
    git pull --rebase/git pull --rebase origin master
    #取回远程主机某个分支的更新，再与本地的指定分支合并

## 进阶

### **合并Meger**
    git fetch origin master:temp 
    #这句命令的意思是：从远程的origin仓库的master分支下载到本地并新建一个分支temp
    git diff temp
    #命令的意思是：比较master分支和temp分支的不同
    git merge temp
    #合并分支
    git branch -d temp
    #删除temp临时分区

合并分区时会出现
Please enter a commit message to explain why this merge is necessary.

git 在pull或者合并分支的时候有时会遇到这个界面。可以不管(直接下面3,4步)，
如果要输入解释的话就需要:

1.按键盘字母 i 进入insert模式

2.修改最上面那行黄色合并信息,可以不修改

3.按键盘左上角"Esc"

4.输入":wq",注意是冒号+wq,按回车键即可

有些状态可以按Q键退出
### **临时修改暂时存放起来**
    git stash #临时修改暂时存放起来
    git stash clear #清空临时缓存
    git stash list #显示缓存

有时候合并会出现异常，需要把当前的更改先缓存起来才能进行合并
### **合并异常**
git 合并异常：
The following untracked working tree files would be overwritten by merge

    git clean -d -fx
    #删除没有被跟踪的文件

### **放弃修改**
    git reset --hard
git reset是以提交名称作为参数的，默认值是HEAD，可以用^和~作为提交名称的修饰符来指定某个版本。

HEAD^是指把版本库复位到当前HEAD之前的那个节点上，把HEAD这个版本的修改扔到工作目录树中，

540ecb7~3是指要复位到540ecb7之前的三个节点上，即把该提交和之前的两个提交（共三个提交）扔到工作目录树中。

    git reset --hard HEAD^ #强制复位前一个提交。
    git reset --hard HEAD~3 #强制复位到前第三个3提交
    git reset HEAD #可以用来清除已经add到缓存区但是不想进一步提交的内容。
### **修改别人的项目**

如果Fork别人的项目或者多人合作项目，最好每人都拥有一个独立分支，然后由项目维护人合并。如何建立自己的分支？

* 分支的创建和合并

    git branch yourbranch 

    git checkout yourbranch  
  
    切换到yourbranch

* 开发yourbranch分支，然后开发之后与master分支合并

    git checkout master

    git merge yourbranch

    git branch -d yourbranch    

    合并完后删除本地分支
* 如何将牛人的远程分支更新到自己的本地分支？

    查看当前项目下远程

    git remote

    增加新的分支链接，例如

    git remote add niuren giturl…

    获取牛人的远程更新

    git fetch niuren

    将牛人的远程更新合并到本地分支

    git merge niuren/master

### **.gitignore(忽略某些文件)**
格式：

    # 忽略所有 .a 结尾的文件
    *.a
    # 但 lib.a 除外
    !lib.a
    # 仅仅忽略项目根目录下的 TODO 文件，不包括 subdir/TODO
    /TODO
    # 忽略 build/ 目录下的所有文件
    build/
    # 会忽略 doc/notes.txt 但不包括 doc/server/arch.txt
    doc/*.txt
编写.gitignore文件

    $vim .gitignore 

会打开创建.gitignore文件
然后输入忽略文件规则，结束后按Esc退出编辑模式，:wq命令退出文本编辑
    
    :wq


