using System;
using System.IO;
using QQ.Framework;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.QdData)]
    internal class TLV_0032 : BaseTLV
    {
        public TLV_0032()
        {
            cmd = 0x0032;
            Name = "TLV_QdData";
            wSubVer = 0x0002;
        }

        public byte[] Get_Tlv(QQUser User)
        {
            byte[] qddata = QdData.GetQdData(User);
            fill_head(cmd);
            fill_body(qddata, qddata.Length);
            set_length();
            return get_buf();
        }
    }
}