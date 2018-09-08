using QQ.Framework.Packets.Receive.Message;

namespace QQ.Framework.Domains.Commands.ResponseCommands.Message
{
    [ResponsePacketCommand(QQCommand.Message0x0058)]
    public class ResponseKeepliveCommand : ResponseCommand<Receive_0x0058>
    {
        public ResponseKeepliveCommand(QQEventArgs<Receive_0x0058> args) : base(args)
        {
        }

        public override void Process()
        {
            var client = _args.QQClient;
            var user = client.QQUser;
            var packet = _args.ReceivePacket;
        }
    }
}