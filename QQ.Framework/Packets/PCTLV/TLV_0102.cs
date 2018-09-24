using System;
using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.Official)]
    internal class TLV_0102 : BaseTLV
    {
        public TLV_0102()
        {
            cmd = 0x0102;
            Name = "SSO2::TLV_Official_0x102";
            wSubVer = 0x0001;
        }

        public byte[] Get_Tlv(QQUser User)
        {
            var data = new BinaryWriter(new MemoryStream());
            if (wSubVer == 0x0001)
            {
                data.BEWrite(wSubVer);
                //OfficialKey
                data.Write(new byte[]
                    {0x9e, 0x9b, 0x03, 0x23, 0x6d, 0x7f, 0xa8, 0x81, 0xa8, 0x10, 0x72, 0xec, 0x50, 0x97, 0x96, 0x8e});
                var bufSigPic = User.TXProtocol.bufSigPic ?? Util.RandomKey(56);
                data.WriteKey(bufSigPic);
                //Official
                data.WriteKey(new byte[]
                {
                    0x60, 0x6f, 0x27, 0xd7, 0xdc, 0x40, 0x46, 0x33, 0xa6, 0xc4, 0xb9, 0x05, 0x7e, 0x60, 0xfb, 0x64,
                    0x1e, 0x75, 0x65, 0x6
                });
            }
            else
            {
                throw new Exception($"{Name} 无法识别的版本号 {wSubVer}");
            }

            fill_head(cmd);
            fill_body(data.BaseStream.ToBytesArray(), data.BaseStream.Length);
            set_length();
            return get_buf();
        }
    }
}