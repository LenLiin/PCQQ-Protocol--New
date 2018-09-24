using System;
using System.Linq;
using System.Reflection;
using QQ.Framework.Domains;
using QQ.Framework.Domains.Commands;
using QQ.Framework.Domains.Commands.ResponseCommands;
using QQ.Framework.Events;
using QQ.Framework.Packets;

namespace QQ.Framework.Utils
{
    public class ResponsePacketProcessor<PacketType> : PacketProcessor<PacketCommand>
        where PacketType : ReceivePacket
    {
        private readonly QQEventArgs<PacketType> _args;
        private readonly Type _receivePacketType;

        protected ResponsePacketProcessor(QQEventArgs<PacketType> args, Type receivePacketType)
        {
            _args = args;
            _receivePacketType = receivePacketType;
        }

        /// <summary>
        ///     根据接收包的Command,自动寻找对应的回复包。
        /// </summary>
        /// <returns></returns>
        public PacketCommand Process()
        {
            var receivePackageCommandAttributes = _receivePacketType.GetCustomAttributes<ReceivePacketCommand>();
            if (receivePackageCommandAttributes.Any())
            {
                var packetCommand = receivePackageCommandAttributes.First().Command;
                var types = Assembly.GetExecutingAssembly().GetTypes();
                foreach (var type in types)
                {
                    var attributes = type.GetCustomAttributes<ResponsePacketCommand>();
                    if (!attributes.Any())
                    {
                        continue;
                    }

                    var responseCommand = attributes.First().Command;
                    if (responseCommand == packetCommand)
                    {
                        return Activator.CreateInstance(type, _args) as PacketCommand;
                    }
                }
            }

            return new DefaultResponseCommand(new QQEventArgs<ReceivePacket>(_args.Service, _args.User,
                _args.ReceivePacket));
        }

        public static ResponsePacketProcessor<PacketType> of(QQEventArgs<PacketType> args, Type receivePacketType)
        {
            return new ResponsePacketProcessor<PacketType>(args, receivePacketType);
        }
    }
}