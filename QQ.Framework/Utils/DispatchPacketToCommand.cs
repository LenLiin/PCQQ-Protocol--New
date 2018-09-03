using System;
using System.Linq;
using System.Reflection;
using QQ.Framework.Domains;

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

        public ReceiveCommand dispatch_receive_packet(QQCommand command)
        {
            var types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var type in types)
            {
                var attributes = type.GetCustomAttributes();
                if (!attributes.Any(attr => attr is ReceivePackageCommand)) continue;

                var attribute = attributes.First(attr => attr is ReceivePackageCommand) as ReceivePackageCommand;
                if (attribute.Command == command)
                {
                    var receive_packet = Activator.CreateInstance(type, new object[] { _data, _client }) as ReceiveCommand;
                    return receive_packet;
                }
            }
            return new DefaultReceiveCommand(_data, _client);
        }
    }
}
