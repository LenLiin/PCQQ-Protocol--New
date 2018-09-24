using System;
using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.GUID_Ex)]
    internal class TLV0313 : BaseTLV
    {
        public TLV0313()
        {
            Command = 0x0313;
            Name = "SSO2::TLV_GUID_Ex_0x313";
            WSubVer = 0x01;
        }

        public byte[] Get_Tlv(QQUser user)
        {
            var data = new BinaryWriter(new MemoryStream());
            if (WSubVer == 0x01)
            {
                data.Write((byte) 1);
                data.Write((byte) 1);
                data.Write((byte) 2);
                data.WriteKey(user.TXProtocol.BufMacGuid);
                data.BeWrite(2);
            }
            else
            {
                throw new Exception($"{Name} 无法识别的版本号 {WSubVer}");
            }

            FillHead(Command);
            FillBody(data.BaseStream.ToBytesArray(), data.BaseStream.Length);
            SetLength();
            return GetBuffer();
        }
    }
}