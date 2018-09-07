using QQ.Framework.Packets.Receive.Data;

namespace QQ.Framework.Domains.Commands.ReceiveCommands.Data
{
    /// <summary>
    /// Key查询
    /// </summary>
    [ReceivePacketCommand(QQCommand.Data0x001D)]
    public class KeyQueryCommand : ReceiveCommand<Receive_0x001D>
    {
        /// <summary>
        /// Key查询
        /// </summary>
        public KeyQueryCommand(byte[] data, QQClient client) : base(data, client)
        {
            _packet = new Receive_0x001D(data, client.QQUser);
            _event_args = new QQEventArgs<Receive_0x001D>(client, _packet);
        }

        public override void Process()
        {
            _client.OnReceive_0x001D(_event_args);
            
        }
    }
}
