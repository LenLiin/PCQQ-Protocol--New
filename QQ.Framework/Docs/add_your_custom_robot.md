# 自定义机器人(非完整版)

1. 新建你的机器人类`YourRoBotClass`, 实现接口`ServerMessageObserver`。
2. 构造函数带`ServerMessageSubject`参数, 并在构造函数中添加一行代码`ServerMessageSubject.AddCustomRoBot(this)`
3. 在对应事件实现你想要的功能。

    demo待补充, 此处暂未重构完