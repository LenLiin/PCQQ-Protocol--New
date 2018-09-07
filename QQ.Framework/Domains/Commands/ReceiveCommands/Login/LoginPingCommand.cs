using QQ.Framework.Packets.Receive.Login;

namespace QQ.Framework.Domains.Commands.ReceiveCommands.Login
{
    [ReceivePacketCommand(QQCommand.Login0x0825)]
    public class LoginPingCommand : ReceiveCommand<Receive_0x0825>
    {
        public LoginPingCommand(byte[] data, QQClient client) : base(data, client)
        {
            _packet = new Receive_0x0825(data, client.QQUser);
            _event_args = new QQEventArgs<Receive_0x0825>(client, _packet);
        }

        public override void Process()
        {
            Response();
        }
    }
}
