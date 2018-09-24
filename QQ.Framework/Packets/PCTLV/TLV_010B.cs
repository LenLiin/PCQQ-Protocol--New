using System;
using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.QDLoginFlag)]
    internal class TLV010B : BaseTLV
    {
        public TLV010B()
        {
            Command = 0x010B;
            Name = "TLV_QDLoginFlag";
            WSubVer = 0x0002;
        }

        public byte[] Get_Tlv(QQUser user)
        {
            var data = new BinaryWriter(new MemoryStream());
            if (WSubVer == 0x0002)
            {
                data.BeWrite(WSubVer); //wSubVer
                var newbyte = user.TXProtocol.BufTgt;
                var flag = EncodeLoginFlag(newbyte, QQGlobal.QqexeMD5);
                data.Write(user.MD51);
                data.Write(flag);
                data.Write((byte) 0x10);
                data.BeWrite(0);
                data.BeWrite(2);
                var qddata = QdData.GetQdData(user);
                data.WriteKey(qddata);
                data.BeWrite(0);
            }
            else
            {
                throw new Exception($"{Name} 无法识别的版本号 {WSubVer}");
            }

            FillHead(Command);
            FillBody(data.BaseStream.ToBytesArray(), data.BaseStream.Length);
            SetLength();
            return GetBuffer();
        }

        private byte EncodeLoginFlag(byte[] bufTgt /*bufTGT*/, byte[] qqexeMD5 /*QQEXE_MD5*/,
            byte flag = 0x01 /*固定 0x01*/)
        {
            var rc = flag;
            foreach (var t in bufTgt)
            {
                rc ^= t;
            }

            for (var i = 0; i < 4; i++)
            {
                var rcc = qqexeMD5[i * 4];
                rcc ^= qqexeMD5[i * 4 + 1];
                rcc ^= qqexeMD5[i * 4 + 3];
                rcc ^= qqexeMD5[i * 4 + 2];
                rc ^= rcc;
            }

            return rc;
        }
    }
}