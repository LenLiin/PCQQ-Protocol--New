using System;

namespace QQ.Framework.Domains.Commands
{
    public class ResponsePacketCommand : Attribute
    {
        public QQCommand Command { get; }

        public ResponsePacketCommand(QQCommand command)
        {
            Command = command;
        }
    }
}