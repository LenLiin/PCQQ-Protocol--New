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
        private readonly SocketService _service;
        private readonly ServerMessageSubject _transponder;
        private readonly QQUser _user;

        protected DispatchPacketToCommand(byte[] data, SocketService service, ServerMessageSubject transponder,
            QQUser user)
        {
            _data = data;
            _service = service;
            _transponder = transponder;
            _user = user;
        }

        public static DispatchPacketToCommand Of(byte[] data, SocketService service, ServerMessageSubject transponder,
            QQUser user)
        {
            return new DispatchPacketToCommand(data, service, transponder, user);
        }

        public PacketCommand dispatch_receive_packet(QQCommand command)
        {
            var types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var type in types)
            {
                var attributes = type.GetCustomAttributes();
                if (!attributes.Any(attr => attr is ReceivePacketCommand))
                {
                    continue;
                }

                var attribute = attributes.First(attr => attr is ReceivePacketCommand) as ReceivePacketCommand;
                if (attribute.Command == command)
                {
                    var receive_packet =
                        Activator.CreateInstance(type, _data, _service, _transponder, _user) as PacketCommand;
                    return receive_packet;
                }
            }

            return new DefaultReceiveCommand(_data, _service, _transponder, _user);
        }
    }
}