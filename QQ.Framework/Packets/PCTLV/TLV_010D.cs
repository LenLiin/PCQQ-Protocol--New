using QQ.Framework;
using QQ.Framework.Utils;
using System;
using System.IO;
namespace Struggle.Framework.PCQQ.PCLogin.PCPacket.PCTLV
{
    internal class TLV_010D : BaseTLV
    {
        public TLV_010D()
        {
            this.cmd = 0x010D;
            this.Name = "TLV_SigLastLoginInfo";
        }

        public void parser_tlv_010D(QQClient m_PCClient, BinaryReader buf)
        {
            this.wSubVer = buf.BEReadUInt16(); //wSubVer
            if (this.wSubVer == 0x0001)
            {
                var bufSigLastLoginInfo = buf.ReadBytes((int)(buf.BaseStream.Length- buf.BaseStream.Position));
            }
            else
            {
                throw new Exception(string.Format("{0} 无法识别的版本号 {1}", this.Name, this.wSubVer));
            }
        }
    }
}
