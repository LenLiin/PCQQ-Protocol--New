using System;
using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.DHParams)]
    internal class TLV0114 : BaseTLV
    {
        public TLV0114()
        {
            Command = 0x0114;
            Name = "SSO2::TLV_DHParams_0x114";
            WSubVer = 0x0102;
        }

        /// <summary>
        ///     DHParams
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public byte[] Get_Tlv(QQUser user)
        {
            var data = new BinaryWriter(new MemoryStream());
            if (WSubVer == 0x0102)
            {
                data.BeWrite(WSubVer); //wDHVer
                data.BeWrite((ushort) user.TXProtocol.BufDhPublicKey.Length); //bufDHPublicKey长度
                data.Write(user.TXProtocol.BufDhPublicKey);
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