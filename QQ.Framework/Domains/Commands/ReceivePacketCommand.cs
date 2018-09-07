using System;

namespace QQ.Framework.Domains.Commands
{
    public class ReceivePacketCommand : Attribute
    {
        public QQCommand Command { get; }

        public ReceivePacketCommand(QQCommand command)
        {
            Command = command;
        }
    }
}