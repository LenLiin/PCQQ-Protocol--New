using System;
using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.DHParams)]
    internal class TLV_0114 : BaseTLV
    {
        public TLV_0114()
        {
            cmd = 0x0114;
            Name = "SSO2::TLV_DHParams_0x114";
            wSubVer = 0x0102;
        }
        /// <summary>
        /// DHParams
        /// </summary>
        /// <param name="User"></param>
        /// <returns></returns>
        public byte[] Get_Tlv(QQUser User)
        {
            var data = new BinaryWriter(new MemoryStream());
            if (wSubVer == 0x0102)
            {
                data.BEWrite(wSubVer); //wDHVer
                data.BEWrite((ushort) User.TXProtocol.bufDHPublicKey.Length); //bufDHPublicKey长度
                data.Write(User.TXProtocol.bufDHPublicKey);
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