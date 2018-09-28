using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags._0x050C)]
    internal class TLV050C : BaseTLV
    {
        public TLV050C()
        {
            Command = 0x050C;
            Name = "SSO2::TLV_050C";
        }

        public byte[] Get_Tlv(QQUser user)
        {
            var data = new BinaryWriter(new MemoryStream());
            /*
            05 0C //TagIndex:1,length:117
            00 75 
            00 00 00 00 
            3F 33 83 CF 
            76 71 01 9D 
            5B AD EF 37 
            00 00 00 01 
            77 69 6E 64 6F 77 73 00 04 5F 80 33 01 01 
            00 00 15 D9 
            66 35 4D F1 AB DC 98 F0 70 69 FC 2A 2B 86
            06 1B 
            00 01 
            3C 
            00 00 00 00 
            18 DC 39 73 
            76 71 01 9D 
            5B AD 7D EF 
            00 00 68 D9 
            00 00 00 00 
            3F 33 83 CF 
            76 71 01 9D 
            5B AD EB B2 
            00 00 68 D9  
            00 00 00 00 
            3F 33 83 CF 
            76 71 01 9D
            5B AD EF 37
            00 00 68 D9 
             */


            FillHead(Command);
            FillBody(data.BaseStream.ToBytesArray(), data.BaseStream.Length);
            SetLength();
            return GetBuffer();
        }

        public void Parser_Tlv(QQUser user, BinaryReader buf)
        {
            var type = buf.BeReadUInt16(); //type
            var length = buf.BeReadUInt16(); //length
            buf.ReadBytes(length);
        }
    }
}