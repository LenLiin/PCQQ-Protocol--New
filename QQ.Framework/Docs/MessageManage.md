# 接收/回复包

#### 添加一个接收包处理

1. 首先在`Domains/Commands`下对应目录新建Command, 继承`ReceiveCommand<要处理的包类型>`。
2. 在新建的Commad上添加`[ReceivePackageCommand(QQCommand.具体你的包名)]`, 这个属性就是包分发的依据，你填写的是什么Command,那么当接收到这个包的时候就会分发给这个类来处理。
3. 构造函数中实例化对应`Packet`和`QQQEventArgs`。
4. 在`Process`方法中写你的具体逻辑即可。

Tips: 可以参考`Domains/Commands/ReceiveCommands/LoginPingCommand`的写法, 这个是对于Login0x0825包的处理。

#### 添加一个回复包

1. 首先在`Domains/Commands`下对应目录新建Command, 继承`ResponseCommand<要处理的包类型>`。
2. 在新建的Commad上添加`[ResponsePacketCommand(QQCommand.具体你的包名)]`, 这个属性就是包分发的依据，你填写的是什么Command,那么当接收到这个包的时候就会分发给这个类来处理。
3. 构造函数中会传入`QQQEventArgs`。
4. 在`Process`方法中写你的具体逻辑即可。
