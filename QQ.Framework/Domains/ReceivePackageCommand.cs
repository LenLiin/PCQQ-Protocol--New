using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQ.Framework.Domains
{
    public class ReceivePackageCommand : Attribute
    {
        public QQCommand Command { get; }

        public ReceivePackageCommand(QQCommand command)
        {
            Command = command;
        }
    }
}
