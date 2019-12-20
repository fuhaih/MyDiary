


## box

mp4格式是由多个box组合成的，box可以嵌套

box分为basic box和full box，每个box由header和data两部分组成。basic box和full box主要是header有点区别。

>header

```http
size        4byte
type        4byte
version     1byte(full box独有)
flags       3byte(full box独有)
largesize   8byte(当size==1时，也就是4byte不足以描述长度时，才会有largesize)
UUIDs       16(当type==uuid时有这个header，uuid是自定义的box)

```
>data

header之后就是data，header中描述的size大小是指header+data的大小。

data也可以是嵌套的box。

## boxtype
>ftyp

长度一般为32byte

```
#header
size                  4byte
type                  4byte
major_brand           4byte     isom
minor_version         4byte     isom的版本号
compatible_brands     12byte    支持协议
brands                4byte     文件格式(mp41，mp42)
```
>moov

是个container box，data包含多个box


>mvhd

是个full box,描述文件的基本信息

data
```
creation_time         version==1?8byte:4byte 创建时间
modification_time     version==1?8byte:4byte 修改时间
timescale             4byte
duration              version==1?8byte:4byte
rate                  4byte
volume                2byte
reserved              2byte
reserved              4bye * 2(有两个)
matrix                4byte * 9(有九个)
pre_defined           4byte * 6(有六个)
next_track_ID         4byte
```

mvhd长度一般为108byte，版本1长度为120byte

header一般是12byte，剩下是data

`duration / timescale = 可播放时长（s）`

`创建时间和修改时间是UTC时间的1904年1月1日0点至今的秒数`
>trak

rak中的一系列子box描述了每个媒体轨道的具体信息

>moof

moofbox，这个box是视频分片的描述信息。并不是MP4文件必须的部分，但在我们常见的可在线播放的MP4格式文件中（例如Silverlight Smooth Streaming中的ismv文件）确是重中之重。

>mdat

实际媒体数据

>mfra

一般在文件末尾，媒体的索引文件，可通过查询直接定位所需时间点的媒体数据

>MP4基本的结构
```
--ftype
--moov
  --mvhd
  --trak
  --trak
  --udta
--free
--mdat
```