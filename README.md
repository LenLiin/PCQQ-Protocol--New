# PCQQ-Protocol
PCQQ协议
简单实现某版本QQ登录与收发消息的功能，可以用来制作使用QQPC协议的机器人

如果对你有用，希望你能留下Star

[![Build Status](https://travis-ci.org/luojinfang/PCQQ-Protocol.svg?branch=master)](https://travis-ci.org/luojinfang/PCQQ-Protocol)

源码仅供参考，欢迎吐槽
------------------------------------------
#### 2018-9-26
1. 群分组信息和好友信息查询
2. 新增`.net standard`分支
3. 主分支修改为`.net framework`

#### 2018-9-25
1. 添加群分组信息报文

#### 2018-9-21
1. TLV格式登录组包
2. 升级协议为QQ8.8

#### 2018-9-9
1. 完成QQClient的重构。
2. 完成自定义机器人的重构, 示例请查看`TestRoBot.cs`

#### 2018-9-8
1. 消息接收后查看确认，发送确认包后不再重复接收消息
2. 添加消息及自定义机器人相关类图。[查看](./QQ.Framework/Docs/UMLs/RoBot.png)

#### 2018-9-7
1. 重构完回复包。
2. 重构消息处理相关，抽象可响应事件列表、转发器、自定义机器人基类。

Tips:文档待补充。

#### 2018-9-6
1. 添加文档
2. 如何添加`Command`。[查看](./QQ.Framework/Docs/MessageManage.md)
3. 如何自定义自己的机器人。[查看](./QQ.Framework/Docs/add_your_custom_robot.md)

#### 2018-9-5
1.`Command`提炼为接口, 移除具体`Command`中的重复定义。<br>
2.`Command`处理方法更名为`Process`。<br>
3.新增回复包的`Command`，及对应的`Prossor`。

#### 2018-9-4 
1.将QQ.Framework目标框架改为netstandard<br>
2.完善现有有效解析的反射重构<br>
3.添加travis-ci持续集成

#### 2018-9-3 
重构MessageManage,将对包的处理逻辑移至Command中, 对应项目里Domains/Commands文件夹下的内容。
如何添加一个Command: 

1. 首先在`Domains/Commands`下对应目录新建Command, 继承`ReceiveCommand`/`SendCommand`。
2. 在新建的Commad上添加`[ReceivePackageCommand(QQCommand.具体你的包名)]`, 这个属性就是包分发的依据，你填写的是什么Command,那么当接收到这个包的时候就会分发给这个类来处理。
3. 构造函数中实例化对应`Packet`和`QQQEventArgs`。
4. 在`Receive`方法中写你的具体逻辑即可。

通过以上四步, 就可以对一个包的解析啦，不用去修改MessageManage，避免在多人修改时容易产生冲突导致, 也更方便的查看处理包的具体逻辑。

Tips: 可以参考`Domains/Commands/ReceiveCommands/LoginPingCommand`的写法, 这个是对于Login0x0825包的处理。

#### 2018-9-1
添加XML消息发送  
移除ByteBuffer，使用BinaryReader和BinaryWriter进行读写。  
发送数据包的方式变更
```C#
//原方式
var buf = new ByteBuffer();
new Send_0x0000().Fill(buf);
Send(buf);
//新方式
Send(new Send_0x0000().WriteData());
//使用以下方法替换原有的ByteBuffer.Put和Get
//若类型为byte或byte[]
BinaryReader.ReadByte()
BinaryReader.ReadBytes(int count)<br/>
BinaryWriter.Write(byte) //注意：需要进行类型转换，如Write((byte)1);
BinaryWriter.Write(byte[])
//对于其他类型(char, ushort, int, long)
BinaryReader.BEReadChar()
BinaryReader.BEReadUInt16()
BinaryReader.BEReadInt32()
BinaryWriter.BEWrite(ushort)
//这些扩展方法定义于Utils.Util.cs内。
```
#### 2018-8-31
合并Core和Framework到同一个文件夹（如果编译出错删除bin、obj和.vs重新编译）  
添加发送系统表情(格式为“[face\{num}.gif]”)，num为系统表情对应索引

#### 2018-8-30
新增长文本分段发送  
修改为项目为.Net Core  
以一种我也不知道会不会出问题的方式兼容了Core和Framework了

#### 2018-8-29
新增验证码处理

联系方式
------------------------------------------
我的TOXID：536F06809AAE9F29B8440B308E310AF8A26B9F93ADB60CAE086EEB26AB8F0D167486F38F39ED
<br>
<!--img src="/tox_save.png?raw=true" style="width:275px;" alt="联系方式"-->
<br>
