using QQ.Framework.Packets.Receive.Login;

namespace QQ.Framework.Domains.Commands.ReceiveCommands.Login
{
    [ReceivePacketCommand(QQCommand.Login0x0828)]
    public class PreLoginCommand : ReceiveCommand<Receive_0x0828>
    {
        public PreLoginCommand(byte[] data, QQClient client) : base(data, client)
        {
            _packet = new Receive_0x0828(data, client.QQUser);
            _event_args = new QQEventArgs<Receive_0x0828>(client, _packet);
        }

        public override void Process()
        {
            Response();
        }
    }
}