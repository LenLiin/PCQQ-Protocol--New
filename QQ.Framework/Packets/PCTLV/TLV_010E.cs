using QQ.Framework;
using QQ.Framework.Utils;
using System;
using System.IO;
namespace Struggle.Framework.PCQQ.PCLogin.PCPacket.PCTLV
{
    internal class TLV_010E : BaseTLV
    {
        public TLV_010E()
        {
            this.cmd = 0x010E;
            this.Name = "TLV_010E";
        }

        public void parser_tlv_010E(QQClient m_PCClient, BinaryReader buf)
        {
            int len;
            byte[] buffer;
            this.wSubVer = buf.BEReadUInt16(); //wSubVer
            if (this.wSubVer == 0x0001)
            {
                len = buf.BEReadUInt16();
                buffer = buf.ReadBytes(len);
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
                throw new Exception(string.Format("{0} 无法识别的版本号 {1}", this.Name, this.wSubVer));
            }
        }
    }
}
