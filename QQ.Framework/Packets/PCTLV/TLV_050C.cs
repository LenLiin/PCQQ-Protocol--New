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
            var buf = new BinaryWriter(new MemoryStream());
            var dataTime = DateTime.Now;
            buf.BeWrite(0);
            buf.BeWrite(user.QQ);
            buf.Write(new byte[] { 0x76, 0x71, 0x01, 0x9d });
            buf.BeWrite(Util.GetTimeMillis(dataTime));
            buf.BeWrite(user.TXProtocol.DwServiceId);
            buf.Write(new byte[]
                { 0x77, 0x69, 0x6e, 0x64, 0x6f, 0x77, 0x73, 0x00, 0x04, 0x5f, 0x80, 0x33, 0x01, 0x01 });
            buf.BeWrite(user.TXProtocol.DwClientVer);
            buf.Write(new byte[]
                { 0x66, 0x35, 0x4d, 0xf1, 0xab, 0xdc, 0x98, 0xf0, 0x70, 0x69, 0xfc, 0x2a, 0x2b, 0x86, 0x06, 0x1b });
            buf.BeWrite(user.TXProtocol.SubVer);

            var data = new BinaryWriter(new MemoryStream());
            data.BeWrite(0);
            data.BeWrite(user.QQ);
            data.Write(new byte[] { 0x76, 0x71, 0x01, 0x9d });
            data.BeWrite(Util.GetTimeMillis(dataTime));
            data.Write(user.TXProtocol.DwPubNo);

            buf.Write((byte) data.BaseStream.Length * 3);
            buf.Write(data.BaseStream.ToBytesArray());
            buf.Write(data.BaseStream.ToBytesArray());
            buf.Write(data.BaseStream.ToBytesArray());


            FillHead(Command);
            FillBody(buf.BaseStream.ToBytesArray(), buf.BaseStream.Length);
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