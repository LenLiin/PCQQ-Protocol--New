using QQ.Framework;
using QQ.Framework.Utils;
using System;
using System.IO;

namespace Struggle.Framework.PCQQ.PCLogin.PCPacket.PCTLV
{
    internal class TLV_000D : BaseTLV
    {
        public TLV_000D()
        {
            this.cmd = 0x000D;
            this.Name = "TLV_000D";
        }

        public void parser_tlv_0D(QQClient m_PCClient, BinaryReader buf)
        {
            this.wSubVer = buf.BEReadUInt16(); //wSubVer
            if (this.wSubVer == 0x0001)
            {
                buf.BEReadInt32(); //UNKNOW
            }
            else
            {
                throw new Exception(string.Format("{0} 无法识别的版本号 {1}", this.Name, this.wSubVer));
            }
        }
    }
}
