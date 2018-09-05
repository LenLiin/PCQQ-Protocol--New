using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQ.Framework.Domains
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