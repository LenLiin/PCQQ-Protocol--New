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
        private readonly ISocketService _service;
        private readonly IServerMessageSubject _transponder;
        private readonly QQUser _user;

        protected DispatchPacketToCommand(byte[] data, ISocketService service, IServerMessageSubject transponder,
            QQUser user)
        {
            _data = data;
            _service = service;
            _transponder = transponder;
            _user = user;
        }

        public static DispatchPacketToCommand Of(byte[] data, ISocketService service, IServerMessageSubject transponder,
            QQUser user)
        {
            return new DispatchPacketToCommand(data, service, transponder, user);
        }

        public IPacketCommand dispatch_receive_packet(QQCommand command)
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
                    var receivePacket =
                        Activator.CreateInstance(type, _data, _service, _transponder, _user) as IPacketCommand;
                    return receivePacket;
                }
            }

            return new DefaultReceiveCommand(_data, _service, _transponder, _user);
        }
    }
}