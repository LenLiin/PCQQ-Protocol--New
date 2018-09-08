using QQ.Framework.Packets.Receive.Message;

namespace QQ.Framework.Domains.Commands.ReceiveCommands.Message
{
    /// <summary>
    ///     KeepAlive（心跳）
    /// </summary>
    [ReceivePacketCommand(QQCommand.Message0x0058)]
    public class KeepAliveCommand : ReceiveCommand<Receive_0x0058>
    {
        /// <summary>
        ///     KeepAlive（心跳）
        /// </summary>
        public KeepAliveCommand(byte[] data, QQClient client) : base(data, client)
        {
            _packet = new Receive_0x0058(data, client.QQUser);
            _event_args = new QQEventArgs<Receive_0x0058>(client, _packet);
        }

        public override void Process()
        {
            Response();
        }
    }
}