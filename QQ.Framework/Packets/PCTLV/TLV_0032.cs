using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.QdData)]
    internal class TLV0032 : BaseTLV
    {
        public TLV0032()
        {
            Command = 0x0032;
            Name = "TLV_QdData";
            WSubVer = 0x0002;
        }

        public byte[] Get_Tlv(QQUser user)
        {
            var qddata = QdData.GetQdData(user);
            FillHead(Command);
            FillBody(qddata, qddata.Length);
            SetLength();
            return GetBuffer();
        }
    }
}