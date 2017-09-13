# GitHelper
## .gitignore(忽略某些文件)
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


