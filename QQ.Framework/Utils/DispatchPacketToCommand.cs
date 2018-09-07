using System;
using System.Linq;
using System.Reflection;
using QQ.Framework.Domains;
using QQ.Framework.Domains.Commands;
using QQ.Framework.Domains.Commands.ReceiveCommands;

namespace QQ.Framework.Utils
{
    public class DispatchPacketToCommand
    {
        private readonly byte[] _data;
        private readonly QQClient _client;

        protected DispatchPacketToCommand(byte[] data, QQClient client)
        {
            _data = data;
            _client = client;
        }

        public static DispatchPacketToCommand of(byte[] data, QQClient client)
        {
            return new DispatchPacketToCommand(data, client);
        }

        public PacketCommand dispatch_receive_packet(QQCommand command)
        {
            var types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var type in types)
            {
                var attributes = type.GetCustomAttributes();
                if (!attributes.Any(attr => attr is ReceivePacketCommand)) continue;

                var attribute = attributes.First(attr => attr is ReceivePacketCommand) as ReceivePacketCommand;
                if (attribute.Command == command)
                {
                    var receive_packet = Activator.CreateInstance(type, new object[] { _data, _client }) as PacketCommand;
                    return receive_packet;
                }
            }
            return new DefaultReceiveCommand(_data, _client);
        }
    }
}
