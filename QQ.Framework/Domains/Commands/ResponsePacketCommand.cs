using System;

namespace QQ.Framework.Domains.Commands
{
    public class ResponsePacketCommand : Attribute
    {
        public ResponsePacketCommand(QQCommand command)
        {
            Command = command;
        }

        public QQCommand Command { get; }
    }
}