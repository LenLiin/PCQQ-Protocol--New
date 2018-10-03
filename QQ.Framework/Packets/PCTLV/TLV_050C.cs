using System;
using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags._0x050C)]
    internal class TLV050C : BaseTLV
    {
        public TLV050C()
        {
            Command = 0x050C;
            Name = "SSO2::TLV_050C";
        }

        public byte[] Get_Tlv(QQUser user)
        {
            var Buf = new BinaryWriter(new MemoryStream());
            var _dataTime = DateTime.Now;
            Buf.BeWrite(0);
            Buf.BeWrite(user.QQ);
            Buf.Write(new byte[] {0x76, 0x71, 0x01, 0x9d});
            Buf.BeWrite(Util.GetTimeMillis(_dataTime));
            Buf.BeWrite(user.TXProtocol.DwServiceId);
            Buf.Write(new byte[] {0x77, 0x69, 0x6e, 0x64, 0x6f, 0x77, 0x73, 0x00, 0x04, 0x5f, 0x80, 0x33, 0x01, 0x01});
            Buf.BeWrite(user.TXProtocol.DwClientVer);
            Buf.Write(new byte[]
                {0x66, 0x35, 0x4d, 0xf1, 0xab, 0xdc, 0x98, 0xf0, 0x70, 0x69, 0xfc, 0x2a, 0x2b, 0x86, 0x06, 0x1b});
            Buf.BeWrite(user.TXProtocol.SubVer);

            var Data = new BinaryWriter(new MemoryStream());
            Data.BeWrite(0);
            Data.BeWrite(user.QQ);
            Data.Write(new byte[] {0x76, 0x71, 0x01, 0x9d});
            Data.BeWrite(Util.GetTimeMillis(_dataTime));
            Data.Write(user.TXProtocol.DwPubNo);

            Buf.Write((byte) Data.BaseStream.Length * 3);
            Buf.Write(Data.BaseStream.ToBytesArray());
            Buf.Write(Data.BaseStream.ToBytesArray());
            Buf.Write(Data.BaseStream.ToBytesArray());


            FillHead(Command);
            FillBody(Buf.BaseStream.ToBytesArray(), Buf.BaseStream.Length);
            SetLength();
            return GetBuffer();
        }

        public void Parser_Tlv(QQUser user, BinaryReader buf)
        {
            var type = buf.BeReadUInt16(); //type
            var length = buf.BeReadUInt16(); //length
            buf.ReadBytes(length);
        }
    }
}