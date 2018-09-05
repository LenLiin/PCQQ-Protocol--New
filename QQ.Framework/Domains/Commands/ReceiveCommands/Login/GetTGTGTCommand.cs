    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
    using QQ.Framework.Packets.Receive.Login;

namespace QQ.Framework.Domains.Commands.ReceiveCommands.Login
{
    [ReceivePacketCommand(QQCommand.Login0x0836)]
    public class GetTGTGTCommand : ReceiveCommand<Receive_0x0836>
    {
        public GetTGTGTCommand(byte[] data, QQClient client) : base(data, client)
        {
            _packet = new Receive_0x0836(data, client.QQUser);
            _event_args = new QQEventArgs<Receive_0x0836>(client, _packet);
        }

        public override void Process()
        {
            if (_packet.GetPacketLength() == 319 || _packet.GetPacketLength() == 351)
            {
                _client.MessageLog("你输入的帐号名或密码不正确，原因可能是：输错帐号；记错密码；未区分字母大小写；未开启小键盘。");
            }
            else if (_packet.GetPacketLength() == 271 || _packet.GetPacketLength() == 207)
            {
                _client.OnReceive_0x0836_686(_event_args);
            }
            else if (_packet.GetPacketLength() == 135)
            {
                _client.MessageLog("抱歉，请重新输入密码");
            }
            else if (_packet.GetPacketLength() == 279)
            {
                _client.MessageLog("你的帐号存在被盗风险，已进入保护模式");
            }
            else if (_packet.GetPacketLength() == 263)
            {
                _client.MessageLog("你输入的帐号不存在");
            }
            else if (_packet.GetPacketLength() == 551 || _packet.GetPacketLength() == 487)
            {
                _client.MessageLog("你的帐号开启了设备锁，请关闭设备锁后再进行操作");
            }
            else if (_packet.GetPacketLength() == 359)
            {
                _client.MessageLog("你的帐号长期未登录已被回收");
            }
            else if (_packet.GetPacketLength() == 871)
            {
                _client.MessageLog("需要验证码登录");
                _client.OnReceive_0x0836_871(_event_args);
            }
            else if (_packet.GetPacketLength() > 700)
            {
                _client.OnReceive_0x0836_622(_event_args);
            }
        }
    }
}
