using System;
using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags._0x010C)]
    internal class TLV_010C : BaseTLV
    {
        public TLV_010C()
        {
            cmd = 0x010C;
            Name = "TLV_010C";
        }

        public void Parser_Tlv(QQUser User, BinaryReader buf)
        {
            var _type = buf.BEReadUInt16();//type
            var _length = buf.BEReadUInt16();//length
            wSubVer = buf.BEReadUInt16(); //wSubVer
            if (wSubVer == 0x0001)
            {
                User.TXProtocol.SessionKey = buf.ReadBytes(16);
                var dwUin = buf.BEReadInt32();
                var dwClientIP = Util.GetIpStringFromBytes(buf.ReadBytes(4)); //IP地址
                User.TXProtocol.wClientPort = buf.BEReadUInt16();
                var dwServerTime = Util.GetDateTimeFromMillis(buf.BEReadInt32());
                var UNKNOW = buf.BEReadInt32();
                var cPassSeqID = buf.ReadByte();
                var dwConnIP = buf.ReadBytes(4);
                var dwReLoginConnIP = buf.ReadBytes(4);
                var dwReLoginCtrlFlag = buf.BEReadInt32();

                int len = buf.BEReadUInt16();
                var bufComputerIDSig = buf.ReadBytes(len);

                len = buf.ReadByte();
                var UNKNOW2 = buf.ReadBytes(len);

                len = buf.BEReadUInt16();
                var UNKNOW3 = buf.ReadBytes(len);
                var a = new BinaryReader(new MemoryStream(UNKNOW3));
                a.ReadByte();
                var dwConnIP2 = a.ReadBytes(4);
            }
            else
            {
                throw new Exception($"{Name} 无法识别的版本号 {wSubVer}");
            }
        }
    }
}