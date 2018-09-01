using System;
using System.IO;
using QQ.Framework;
using QQ.Framework.Utils;

namespace Struggle.Framework.PCQQ.PCLogin.PCPacket.PCTLV
{
    internal class TLV_002F : BaseTLV
    {
        public TLV_002F()
        {
            cmd = 0x002F;
            Name = "TLV_Control";
        }

        public void parser_tlv_002F(QQClient m_PCClient, BinaryReader buf)
        {
            wSubVer = buf.BEReadUInt16(); //wSubVer
            if (wSubVer == 0x0001)
            {
                var bufControl = buf.ReadBytes((int) (buf.BaseStream.Length - buf.BaseStream.Position));
            }
            else
            {
                throw new Exception(string.Format("{0} 无法识别的版本号 {1}", Name, wSubVer));
            }
        }
    }
}