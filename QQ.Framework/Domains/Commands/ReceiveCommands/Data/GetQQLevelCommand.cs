using QQ.Framework.Packets.Receive.Data;

namespace QQ.Framework.Domains.Commands.ReceiveCommands.Data
{
    /// <summary>
    ///     获取QQ等级
    /// </summary>
    [ReceivePacketCommand(QQCommand.Data0x005C)]
    public class GetQQLevelCommand : ReceiveCommand<Receive_0x005C>
    {
        /// <summary>
        ///     获取QQ等级
        /// </summary>
        public GetQQLevelCommand(byte[] data, QQClient client) : base(data, client)
        {
            _packet = new Receive_0x005C(data, client.QQUser);
            _event_args = new QQEventArgs<Receive_0x005C>(client, _packet);
        }

        public override void Process()
        {
        }
    }
}