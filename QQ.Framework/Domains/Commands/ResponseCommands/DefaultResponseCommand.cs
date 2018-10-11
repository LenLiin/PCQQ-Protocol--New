using QQ.Framework.Events;
using QQ.Framework.Packets;

namespace QQ.Framework.Domains.Commands.ResponseCommands
{
    public class DefaultResponseCommand : ResponseCommand<ReceivePacket>
    {
        // 未完成
        public DefaultResponseCommand(QQEventArgs<ReceivePacket> args) : base(args)
        {
        }

        public override void Process()
        {
        }
    }
}