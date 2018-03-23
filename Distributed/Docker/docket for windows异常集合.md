# Docker for Windows error: Hardware assisted virtualization and data execution protection must be enabled in the BIOS)
 意思就是需要在bios中开启cpu虚拟化技术

docker在linux上运行不需要虚拟化，但是，如果在windows上安装运行就需要虚拟化，找到答案了。



    Docker 底层的核心技术包括 Linux 上的名字空间（ Namespaces） 、 控制组（ Control groups） 、 Union 文
    件系统（ Union file systems） 和容器格式（ Container format） 。
    我们知道， 传统的虚拟机通过在宿主主机中运行 hypervisor 来模拟一整套完整的硬件环境提供给虚拟机的
    操作系统。 虚拟机系统看到的环境是可限制的， 也是彼此隔离的。 这种直接的做法实现了对资源最完整的
    封装， 但很多时候往往意味着系统资源的浪费。 例如， 以宿主机和虚拟机系统都为 Linux 系统为例， 虚拟
    机中运行的应用其实可以利用宿主机系统中的运行环境。
    我们知道， 在操作系统中， 包括内核、 文件系统、 网络、 PID、 UID、 IPC、 内存、 硬盘、 CPU 等等， 所有
    的资源都是应用进程直接共享的。 要想实现虚拟化， 除了要实现对内存、 CPU、 网络IO、 硬盘IO、 存储空
    间等的限制外， 还要实现文件系统、 网络、 PID、 UID、 IPC等等的相互隔离。 前者相对容易实现一些， 后
    者则需要宿主机系统的深入支持。
    随着 Linux 系统对于名字空间功能的完善实现， 程序员已经可以实现上面的所有需求， 让某些进程在彼此
    隔离的名字空间中运行。 大家虽然都共用一个内核和某些运行时环境（ 例如一些系统命令和系统库） ， 但
    是彼此却看不到， 都以为系统中只有自己的存在。 这种机制就是容器（ Container） ， 利用名字空间来做权
    限的隔离控制， 利用 cgroups 来做资源分配。