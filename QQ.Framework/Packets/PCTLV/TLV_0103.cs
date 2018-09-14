using System;
using System.IO;
using QQ.Framework;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    internal class TLV_0103 : BaseTLV
    {
        public TLV_0103()
        {
            cmd = 0x0103;
            Name = "SSO2::TLV_SID_0x103";
            wSubVer = 0x0001;
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
            fill_head(cmd);
            fill_body(data.BaseStream.ToBytesArray(), data.BaseStream.Length);
            set_length();
            return get_buf();
        }

        public void parser_tlv_0103(QQClient m_PCClient, BinaryReader buf)
        {
            wSubVer = buf.BEReadUInt16(); //wSubVer
            if (wSubVer == 0x0001)
            {
                var len = buf.BEReadUInt16();
                QQGlobal.bufSID = buf.ReadBytes(len);
            }
            else
            {
                throw new Exception($"{Name} 无法识别的版本号 {wSubVer}");
            }
        }
    }
}