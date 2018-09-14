using System;
using System.IO;
using QQ.Framework;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    internal class TLV_010E : BaseTLV
    {
        public TLV_010E()
        {
            cmd = 0x010E;
            Name = "TLV_010E";
        }

        public void parser_tlv_010E(QQClient m_PCClient, BinaryReader buf)
        {
            wSubVer = buf.BEReadUInt16(); //wSubVer
            if (wSubVer == 0x0001)
            {
                int len = buf.BEReadUInt16();
                var buffer = buf.ReadBytes(len);
                var sig = new BinaryReader(new MemoryStream(buffer));
                var dwUinLevel = sig.BEReadInt32();
                var dwUinLevelEx = sig.BEReadInt32();

                len = sig.BEReadUInt16();
                buffer = sig.ReadBytes(len);
                var buf24byteSignature = buffer;

                len = sig.BEReadUInt16();
                buffer = sig.ReadBytes(len);
                var buf32byteValueAddedSignature = buffer;

                len = sig.BEReadUInt16();
                buffer = sig.ReadBytes(len);
                var buf12byteUserBitmap = buffer;

                m_PCClient.QQUser.TXProtocol.ClientKey = buf32byteValueAddedSignature;
                //client.QQUser.ClientKeyString = Util.ToHex(buf32byteValueAddedSignature).Replace(" ", "");
                //client.GetCookie();
            }
            else
            {
                throw new Exception($"{Name} 无法识别的版本号 {wSubVer}");
            }
        }
    }
}