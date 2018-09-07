using System;

namespace QQ.Framework.Domains.Commands
{
    public class ReceivePacketCommand : Attribute
    {
        public ReceivePacketCommand(QQCommand command)
        {
            Command = command;
        }

        public QQCommand Command { get; }
    }
}