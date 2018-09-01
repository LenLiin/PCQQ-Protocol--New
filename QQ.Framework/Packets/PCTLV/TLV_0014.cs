using System;
using System.IO;
using QQ.Framework;
using QQ.Framework.Utils;

namespace Struggle.Framework.PCQQ.PCLogin.PCPacket.PCTLV
{
    internal class TLV_0014 : BaseTLV
    {
        public TLV_0014()
        {
            cmd = 0x0014;
            Name = "TLV_0014";
        }

        public void parser_tlv_14(QQClient m_PCClient, BinaryReader buf)
        {
            wSubVer = buf.BEReadUInt16(); //wSubVer
            if (wSubVer == 0x0001)
            {
                buf.BEReadUInt16(); //UNKNOW
            }
            else
            {
                throw new Exception(string.Format("{0} 无法识别的版本号 {1}", Name, wSubVer));
            }
        }
    }
}