using QQ.Framework;
using QQ.Framework.Utils;
using System;
using System.IO;
namespace Struggle.Framework.PCQQ.PCLogin.PCPacket.PCTLV
{
    internal class TLV_0103 : BaseTLV
    {
        public TLV_0103()
        {
            this.cmd = 0x0103;
            this.Name = "SSO2::TLV_SID_0x103";
            this.wSubVer = 0x0001;
        }

        public byte[] get_tlv_0103(QQClient m_PCClient)
        {
            if (QQGlobal.bufSID == null || QQGlobal.bufSID.Length == 0)
            {
                return null;
            }
              var data = new BinaryWriter(new MemoryStream());
            data.BEWrite(0x0001);
            data.Write(QQGlobal.bufSID);
            fill_head(this.cmd);
            fill_body(data.BaseStream.ToBytesArray(), data.BaseStream.Length);
            set_length();
            return get_buf();
        }

        public void parser_tlv_0103(QQClient m_PCClient, BinaryReader buf)
        {
            this.wSubVer = buf.BEReadUInt16(); //wSubVer
            if (this.wSubVer == 0x0001)
            {
                var len = buf.BEReadUInt16();
                QQGlobal.bufSID = buf.ReadBytes(len);
            }
            else
            {
                throw new Exception(string.Format("{0} 无法识别的版本号 {1}", this.Name, this.wSubVer));
            }
        }
    }
}
