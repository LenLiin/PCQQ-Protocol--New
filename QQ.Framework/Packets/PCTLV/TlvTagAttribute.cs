using System;
using System.Collections.Generic;
using System.Text;

namespace QQ.Framework.Packets.PCTLV
{
    public class TlvTagAttribute : Attribute
    {
        public TlvTagAttribute(TlvTags tag)
        {
            Tag = tag;
        }

        public TlvTags Tag { get; }
    }
}
