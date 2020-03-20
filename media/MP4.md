
## MP4基本的结构

| 1 | 2 | 3 | 4 | 5 | 6 | 必要 | boxtype | 9 | 10|
|:--|:--|:--|:--|:--|:--|:--|:--|:--|:--|
|ftyp|    |    |    |    |    |  * |box    |4.3      |filetype and compatibility|
|pdin|    |    |    |    |    |    |box    |8.1.3    |progressive download information|
|moov|    |    |    |    |    |  * |box    |8.2.1    | container for all the metadata|
|    |mvhd|    |    |    |    |  * |box    |8.2.2    |movie header,overall declarations|
|    |meta|    |    |    |    |    |box    |8.11.1   |metadata|
|    |trak|    |    |    |    |  * |box    |8.3.1    |container for an individual track or stream
|    |    |tkhd|    |    |    |  * |fullbox|8.3.2    |track header,overall information about the track
|    |    |tref|    |    |    |    |box    |8.3.3    |track reference container
|    |    |trgr|    |    |    |    |box    |8.3.4    |track grouping indication
|    |    |edts|    |    |    |    |box    |8.6.4    |edit list container
|    |    |    |elst|    |    |    |box    |8.6.6    | an edit list
|    |    |meta|    |    |    |    |box    |8.11.1   |metadata
|    |    |mdia|    |    |    |  * |box    |8.4      |container for the media information in a track
|    |    |    |mdhd|    |    |  * |fullbox|8.4.2    |media header,overall information about the media
|    |    |    |hdlr|    |    |  * |fullbox|8.4.3    |handler,declares the media(handler)type
|    |    |    |elng|    |    |    |box    |8.4.6    |extended language container
|    |    |    |minf|    |    |  * |box    |8.4.4    | media information container
|    |    |    |    |vmhd|    |    |fullbox|12.1.2   |video media header,overall information(video track only)
|    |    |    |    |smhd|    |    |fullbox|12.2.2   |sound media header,overall infomation(sound track only) 
|    |    |    |    |hmhd|    |    |box    |12.4.2   |hint media header,overall information(hint track only)
|    |    |    |    |sthd|    |    |box    |12.6.2   |subtitle media header,overall information(subtitle tracks only)
|    |    |    |    |nmhd|    |    |box    |8.4.5.2  |Null media header,overall information(some tracks only)
|    |    |    |    |dinf|    |  * |box    |8.7.1    | data information box,container
|    |    |    |    |    |dref|  * |fullbox|8.7.2    | data reference box,declares source(s) of media data in track
|    |    |    |    |stbl|    |  * |box    |8.5.1    |sample table box,container for the time/space map
|    |    |    |    |    |stsd|  * |box    |8.5.2    |sample descriptions(codec types,initialization etc.)
|    |    |    |    |    |stts|  * |fullbox|8.6.1.2  |(decoding)time-to-sample
|    |    |    |    |    |ctts|    |fullbox|8.6.1.3  |(composition)time to sample
|    |    |    |    |    |cslg|    |box    |8.6.1.4  |composition to decode timeline mapping
|    |    |    |    |    |stsc|  * |fullbox|8.7.4    |sample-to-chunk,partial data-offset information 
|    |    |    |    |    |stsz|    |fullbox|8.7.3.2  |sample sizes(framing)
|    |    |    |    |    |stz2|    |box    |8.7.3.3  |compact sample sizes(framing)
|    |    |    |    |    |stco|  * |fullbox|8.7.5    |chunk offset,partial data-offset information
|    |    |    |    |    |co64|    |fullbox|8.7.5    |64-bit chunk offset
|    |    |    |    |    |stss|    |box    |8.6.2    |sync sample table
|    |    |    |    |    |stsh|    |box    |8.6.3    |shadow sync sample table
|    |    |    |    |    |padb|    |box    |8.7.6    |sample padding bits
|    |    |    |    |    |stdp|    |box    |8.7.6    |sample degradation priority
|    |    |    |    |    |sdtp|    |fullbox|8.6.4    |independent and disposable samples
|    |    |    |    |    |sbgp|    |box    |8.9.2    |sample-to-group
|    |    |    |    |    |sgpd|    |box    |8.9.3    |sample group description
|    |    |    |    |    |subs|    |box    |8.7.7    |sub-sample information
|    |    |    |    |    |saiz|    |box    |8.7.8    |sample auxiliary information sizes
|    |    |    |    |    |saio|    |box    |8.7.9    |sample auxiliary information offsets
|    |    |udta|    |    |    |    |box    |8.10.1   |user-data
|    |mvex|    |    |    |    |    |box    |8.8.1    |movie extends box
|    |    |mehd|    |    |    |    |box    |8.8.2    |movie extends header box
|    |    |trex|    |    |    |  * |fullbox|8.8.3    |track extends defaults
|    |    |leva|    |    |    |    |box    |8.8.13   |level assignment
|moof|    |    |    |    |    |    |box    |8.8.4    |movie fragment
|    |mfhd|    |    |    |    |  * |box    |8.8.5    |movie fragment header
|    |meta|    |    |    |    |    |box    |8.11.1   |metadata
|    |traf|    |    |    |    |    |box    |8.8.6    |track fragment
|    |    |tfhd|    |    |    |  * |fullbox|8.8.7    |track fragment header
|    |    |trun|    |    |    |    |fullbox|8.8.8    |track fragment run
|    |    |sbgp|    |    |    |    |box    |8.9.2    |sample-to-group
|    |    |sgpd|    |    |    |    |box    |8.9.3    |sample group description
|    |    |subs|    |    |    |    |box    |8.7.7    |sub-sample information
|    |    |saiz|    |    |    |    |box    |8.7.8    |sample auxiliary information sizes
|    |    |saio|    |    |    |    |box    |8.7.9    |sample auxiliary information offsets
|    |    |tfdt|    |    |    |    |fullbox|8.8.12   |track fragment decode time
|    |    |meta|    |    |    |    |box    |8.11.1   |metadata
|mfra|    |    |    |    |    |    |box    |8.8.9    |movie fragment random access
|    |tfra|    |    |    |    |    |box    |8.8.10   |track fragment random access
|    |mfro|    |    |    |    |  * |box    |8.8.11   |movie fragment random access offset
|mdat|    |    |    |    |    |    |box    |8.2.2    |media data container
|free|    |    |    |    |    |    |box    |8.1.2    |free space
|skip|    |    |    |    |    |    |box    |8.1.2    |free space
|    |udta|    |    |    |    |    |box    |8.10.1   |user-data
|    |    |cprt|    |    |    |    |box    |8.10.2   |copyright etc.
|    |    |tsel|    |    |    |    |box    |8.10.3   |track selection box
|    |    |strk|    |    |    |    |box    |8.14.3   |sub track box
|    |    |    |stri|    |    |    |box    |8.14.4   |sub track information box
|    |    |    |strd|    |    |    |box    |8.14.5   |sub track definition box
|meta|    |    |    |    |    |    |box    |8.11.1   |metadata
|    |hdlr|    |    |    |    |  * |box    |8.4.3    |handler,declares the metadata(handler)type
|    |dinf|    |    |    |    |    |box    |8.7.1    |data information box,container
|    |    |dref|    |    |    |    |box    |8.7.2    |data reference box,declares source(s) of metadata items
|    |iloc|    |    |    |    |    |box    |8.11.3   |item location
|    |ipro|    |    |    |    |    |box    |8.11.5   |item protection
|    |    |sinf|    |    |    |    |box    |8.12.1   |protection scheme information box
|    |    |    |frma|    |    |    |box    |8.12.2   |original format box
|    |    |    |schm|    |    |    |box    |8.12.5   |scheme type box
|    |    |    |schi|    |    |    |box    |8.12.6   |scheme information box
|    |iinf|    |    |    |    |    |box    |8.11.6   |item information
|    |xml |    |    |    |    |    |box    |8.11.2   |xml container
|    |bxml|    |    |    |    |    |box    |8.11.2   |binary item reference
|    |pitm|    |    |    |    |    |box    |8.11.4   |primary item reference
|    |fiin|    |    |    |    |    |box    |8.13.2   |file delivery item information
|    |    |paen|    |    |    |    |box    |8.13.2   |partition entry
|    |    |    |fire|    |    |    |box    |8.13.7   |file reservoir
|    |    |    |fpar|    |    |    |box    |8.13.3   |file partition
|    |    |    |fecr|    |    |    |box    |8.13.4   |FEC reservoir
|    |    |segr|    |    |    |    |box    |8.13.5   |file delivery session group
|    |    |gitn|    |    |    |    |box    |8.13.6   |group id to name
|    |idat|    |    |    |    |    |box    |8.11.11  |item data
|    |iref|    |    |    |    |    |box    |8.11.12  |item reference
|meco|    |    |    |    |    |    |box    |8.11.7   |additional metadata container
|    |mere|    |    |    |    |    |box    |8.11.8   |metabox relation
|    |    |meta|    |    |    |    |box    |8.11.1   |metadata
|styp|    |    |    |    |    |    |box    |8.16.2   |segment type
|sidx|    |    |    |    |    |    |box    |8.16.3   |segment index
|ssix|    |    |    |    |    |    |box    |8.16.4   |subsegment index
|prft|    |    |    |    |    |    |box    |8.16.5   |producer reference time



## box结构

mp4格式是由多个box组合成的，box可以嵌套

box分为basic box和full box，每个box由header和data两部分组成。basic box和full box主要是header有点区别。

>header

| name | length | desc |
|:--   |:--     |:--   |
|size      |4byte   |整个box的长度，就是header length + data length，当size==1时，也就是4byte不足以描述长度时，会使用到largesize，当size==0时，表示该box是最后一个box，长度是到文件末尾。
|type      |4byte   |box 类型，见mp4结构表，已列出全部box类型
|version   |1byte   |(full box独有)
|flags     |3byte   |(full box独有)
|largesize |8byte   |(当size==1时，也就是4byte不足以描述长度时，才会有largesize这个header)。
|UUIDs     |16      |(当type==uuid时有这个header，uuid是自定义的box)


>data

header之后就是data，header中描述的size大小是指header+data的大小。

data也可以是嵌套的box。

## boxtype
>ftyp (File Type Box)

长度一般为32byte


| name | length | desc |
|:--   |:--     |:--   |
|header            |* | basic box 头信息
|major_brand       |32    |isom
|minor_version     |32    |isom的版本号
|compatible_brands |32 * n| 数组，和major_brand 类似，通常是针对 MP4 中包含的额外格式，比如，AVC，AAC 等相当于的音视频解码格式。


>moov (Movie Box)

是个container box，data包含多个box


>mvhd (Movie Header Box)

是个full box,描述文件的基本信息

| name | length|length(version=1)|desc|
|:--|:--|:--|:--|
|fullbox header    | * | * | fullbox 头信息|
|creation time     |32     |64     |创建时间（相对于UTC时间1904-01-01零点的秒数）|
|modification time |32     |64     |修改时间|
|time scale        |32     |32     |文件媒体在1秒时间内的刻度值，可以理解为1秒长度的时间单元数|
|duration          |32     |64     |该track的时间长度，用duration和time scale值可以计算track时长，比如audio track的time scale = 8000, duration = 560128，时长为70.016，video track的time scale = 600, duration = 42000，时长为70|
|rate              |32     |32     |推荐播放速率，高16位和低16位分别为小数点整数部分和小数部分，即[16.16] 格式，该值为1.0（0x00010000）表示正常前向播放|
|volume            |16     |16     |推荐播放音量，[8.8] 格式，1.0（0x0100）表示最大音量|
|reserved          |16     |16     |
|reserved          |32 * 2 |32 * 2 |数组，reserved总共占据10个字节|
|matrix            |32 * 9 |32 * 9 |视频变换矩阵|
|pre-defined       |32 * 6 |32 * 6 | |
|next track id     |32     |32     |下一个track使用的id号 |


mvhd长度一般为108byte，版本1长度为120byte

header一般是12byte，剩下是data

`duration / timescale = 可播放时长（s）`

`创建时间和修改时间是UTC时间的1904年1月1日0点至今的秒数`

>trak (Track Box)

moov中会有多个track box，track中的一系列子box描述了每个媒体轨道的具体信息，比较重要的两个内嵌box是`stco`和`co64`    
这两个是存放chunk的offset信息的，offset是相对于整个文件的offset，这是为了方便从整个文件中获取某个chunk的信息，也就是方便视频跳跃播放    
当视频文件的文件结构变化时，这两个box的信息也是要跟着变化的。

>tkhd (Track Header Box)

该Box用于描述Track的基本属性，一个track中有且只有一个Track Header Box。

媒体轨道的flag标志的默认值为7（即111：track_enabled，track_in_movie，track_in_preview）。如果在媒体数据中所有轨道都没有设置track_in_movie和track_in_preview，则应将所有轨道视为在所有轨道上都设置了两个标志。

服务器提示轨道（hint track）应将track_in_movie和track_in_preview设置为0，以便在本地回放和预览时忽略它们

| name | length|length(version=1)|desc|
|:--|:--|:--|:--|
|header            |*    |*     | fullbox 头信息|
|creation_time     |32    |64    |创建时间，从1904.01.01 00:00开始秒数
|modification_time |32    |64    |最近修改时间，从1904.01.01 00:00开始秒数
|track_ID          |32    |32    |track id
|reserved          |32    |32    |
|duration          |32    |64    |媒体持续时长（在指定的时间尺度上）,如果无法确定，值将被设置为1s，实际播放时间计算公式：duration/timescale秒，
|reserved          |32* 2 |32 * 2|数组
|layer             |16    |16    |指定视频track的前后顺序，该值通常为0。如果有-1和0两个track，那么-1所在track将在0所在track的前方显示。
|alternate_group   |16    |16    |是一个整数，指定一组或一组轨道，该值默认为0，表示没有和其它轨道关联。（不懂）
|volume            |16    |16    |播放音量，值为1.0时表示最高音量
|reserved          |16    |16    |
|matrix            |32 * 9|32 * 9|视频变换矩阵（不明白是干啥用的）
|width             |32    |32    |
|height            |32    |32    |


需要特别说明的字段是flag，这是一个占用24bit空间的整数，用于定义以下属性：

**Track_enabled:** 表示该track是否可用，Flag值为0x000001。一个不可用状态的track（Flag值为0x000000）数据会被当做不显示处理。
**Track_in_movie:** 表示该track被用于播放，Flag值为0x000002。
**Track_in_preview:**表示该track用于预览媒体文件。Flag值为0x000004。
**Track_size_is_aspect_ratio:** 表示track的宽高字段不是以像素为单位，且该值表示宽高比。Flag值为0x000008。
width&height：

对于文字或者字幕track，宽高取决于编码格式，用于描述推荐渲染区域的尺寸。对于这样的轨道，值0x0也可用于指示数据可以以任何大小呈现，并没有指定最优显示尺寸，它的实际大小可以通过外部上下文或通过重用另一个轨道的宽高来确定。对于这种轨道，也可以使用标志**track_size_is_aspect_ratio**。
对于可不见内容的track（例如音频文件）宽高都应该设置为0。
除此之外的其他track，width&height指定了可见track的宽高。

>media (Media Box)

>mdhd (Media Header Box)

| name | length|length(version=1)|desc|
|:--|:--|:--|:--|
|header            |*      |*     |fullbox header
|creation_time     |32     |64    |创建时间
|modification_time |32     |64    |修改时间
|timescale         |32     |32    |时间尺度，表示一秒钟的时间单位数
|duration          |32     |64    |媒体持续时长（在指定的时间尺度下）如果无法确定，值将被设置为1s。实际播放时间计算公式：duration/timescale秒。
|pad               |1      |1     |一个占位符
|language          |5 * 3  |5 * 3 |当前track的语言，该字段总长为16bit，和pad字段组成两个字节
|pre_defined       |16     |16    |默认值为0。

>hdlr(Handler Reference Box)

| name | length|desc|
|:--|:--|:--|
|pre_defined   |32     |默认值为 0
|handler_type  |32     |表示当前track的处理类型
|reserved      |32 * 3 |
|name          | *     |名称


**handler_type：**

当该Box位于Media Box中，表示当前track的处理类型。如：video、audio、hint track、meta track等。
当存在于Meta Box中时，包含指定Meta Box的内容格式。 可以在住Meta Box中使用值’null’来指示它
仅用于保存资源。

* 视频：vide 0x76, 0x69, 0x64, 0x65, 0x00, 0x00, 0x00, 0x00
* 音频：soun 0x73, 0x6F, 0x75, 0x6E, 0x00, 0x00, 0x00, 0x00

**name:**   
是一个以UTF-8字符结尾的以null结尾的字符串，它为轨道类型提供了一个人类可读的名称（用于调试和检查）

>moof

moofbox，这个box是视频分片的描述信息。并不是MP4文件必须的部分，但在我们常见的可在线播放的MP4格式文件中（例如Silverlight Smooth Streaming中的ismv文件）确是重中之重。

>mdat

实际媒体数据

>mfra

一般在文件末尾，媒体的索引文件，可通过查询直接定位所需时间点的媒体数据

>mvhd


















