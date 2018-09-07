using System;
using System.IO;
using QQ.Framework;
using QQ.Framework.Utils;

namespace Struggle.Framework.PCQQ.PCLogin.PCPacket.PCTLV
{
    internal class TLV_010D : BaseTLV
    {
        public TLV_010D()
        {
            cmd = 0x010D;
            Name = "TLV_SigLastLoginInfo";
        }

        public void parser_tlv_010D(QQClient m_PCClient, BinaryReader buf)
        {
            wSubVer = buf.BEReadUInt16(); //wSubVer
            if (wSubVer == 0x0001)
            {
                var bufSigLastLoginInfo = buf.ReadBytes((int) (buf.BaseStream.Length - buf.BaseStream.Position));
            }
            else
            {
                throw new Exception($"{Name} 无法识别的版本号 {wSubVer}");
            }
        }
    }
}