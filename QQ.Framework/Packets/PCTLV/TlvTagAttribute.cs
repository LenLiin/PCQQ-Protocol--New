using System;

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