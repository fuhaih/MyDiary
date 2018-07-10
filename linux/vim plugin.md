# vim 配置文件.vimrc 路径
```vim
$ vim
:version
   system vimrc file: "/etc/vimrc"
     user vimrc file: "$HOME/.vimrc"
 2nd user vimrc file: "~/.vim/vimrc"
      user exrc file: "$HOME/.exrc"
  fall-back for $VIM: "/etc"
 f-b for $VIMRUNTIME: "/usr/share/vim/vim74"
```
通过vim的version命令能看到vim的配置文件.vimrc路径，这里有三个路径，同时vim还支持vi的.exrc配置文件
# 插件管理器vundle
* 下载并安装Vundle  
    ```vim
    $ git clone https://github.com/VundleVim/Vundle.vim.git ~/.vim/bundle/Vundle.vim
    ```
* 修改vim的配置文件vimrc    
    这里我们配置的是root用户下的vim，如果要配置全局vim，修改/etc/.vimrc
    ```vim
    vim .vimrc
    ```
    在配置文件中输入
    ```vim   
    set nocompatible    " be iMproved, required 
    filetype off " required 
    " 启用vundle来管理vim插件
    set rtp+=~/.vim/bundle/Vundle.vim
    call vundle#begin()
    " 安装插件写在这之后
    " let Vundle manage Vundle, required
    Plugin 'VundleVim/Vundle.vim'
    " 安装插件写在这之前
    call vundle#end() " required
    filetype plugin on

    " required" 常用命令
    " :PluginList - 查看已经安装的插件
    " :PluginInstall - 安装插件
    " :PluginUpdate - 更新插件
    " :PluginSearch - 搜索插件，例如 :PluginSearch xml就能搜到xml相关的插件
    " :PluginClean - 删除插件，把安装插件对应行删除，然后执行这个命令即可
    " h: vundle - 获取帮助
    ```

* 查看安装情况  
    ```vim
    $ vim
    :PluginInstall
    ```
    PluginInstall会进入插件安装界面
