using System;
using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.QDLoginFlag)]
    internal class TLV_010B : BaseTLV
    {
        public TLV_010B()
        {
            cmd = 0x010B;
            Name = "TLV_QDLoginFlag";
            wSubVer = 0x0002;
        }

        public byte[] Get_Tlv(QQUser User)
        {
            var data = new BinaryWriter(new MemoryStream());
            if (wSubVer == 0x0002)
            {
                data.BEWrite(wSubVer); //wSubVer
                var newbyte = User.TXProtocol.bufTGT;
                var flag = EncodeLoginFlag(newbyte, QQGlobal.QQEXE_MD5);
                data.Write(User.MD51);
                data.Write(flag);
                data.Write((byte) 0x10);
                data.BEWrite(0);
                data.BEWrite(2);
                var qddata = QdData.GetQdData(User);
                data.WriteKey(qddata);
                data.BEWrite(0);
            }
            else
            {
                throw new Exception($"{Name} 无法识别的版本号 {wSubVer}");
            }

            fill_head(cmd);
            fill_body(data.BaseStream.ToBytesArray(), data.BaseStream.Length);
            set_length();
            return get_buf();
        }

        private byte EncodeLoginFlag(byte[] bufTGT /*bufTGT*/, byte[] QQEXE_MD5 /*QQEXE_MD5*/,
            byte flag = 0x01 /*固定 0x01*/)
        {
            var RC = flag;
            foreach (var t in bufTGT)
            {
                RC ^= t;
            }

            for (var i = 0; i < 4; i++)
            {
                var RCC = QQEXE_MD5[i * 4];
                RCC ^= QQEXE_MD5[i * 4 + 1];
                RCC ^= QQEXE_MD5[i * 4 + 3];
                RCC ^= QQEXE_MD5[i * 4 + 2];
                RC ^= RCC;
            }

            return RC;
        }
    }
}