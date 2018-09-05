using System;

namespace QQ.Framework.Domains
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