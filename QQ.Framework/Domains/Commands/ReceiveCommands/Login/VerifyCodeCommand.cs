using QQ.Framework.Packets.Receive.Login;

namespace QQ.Framework.Domains.Commands.ReceiveCommands.Login
{
    [ReceivePacketCommand(QQCommand.Login0x00BA)]
    public class VerifyCodeCommand : ReceiveCommand<Receive_0x00BA>
    {
        public VerifyCodeCommand(byte[] data, QQClient client) : base(data, client)
        {
            _packet = new Receive_0x00BA(data, client.QQUser);
            _event_args = new QQEventArgs<Receive_0x00BA>(client, _packet);
        }

        public override void Process()
        {
            Response();
        }
    }
}
