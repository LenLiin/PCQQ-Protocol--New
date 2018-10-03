using System.Drawing;
using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.Send.Message
{
    /// <summary>
    ///     私聊图片获取Ukey
    /// </summary>
    public class Send_0X0352 : SendPacket
    {
        public Send_0X0352(QQUser user, string fileName, long toQQ)
            : base(user)
        {
            Sequence = GetNextSeq();
            SecretKey = user.TXProtocol.SessionKey;
            Command = QQCommand.Message0X0352;
            FileName = fileName;
            ToQQ = toQQ;
        }

        public string FileName { get; set; }
        public long ToQQ { get; set; }

        protected override void PutHeader()
        {
            base.PutHeader();
            //writer.Write(user.QQ_PACKET_FIXVER);
            Writer.Write(new byte[]
            {
                0x04, 0x00, 0x00, 0x00, 0x01, 0x01, 0x01, 0x00, 0x00, 0x68,
                0x1C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            });
        }

        /// <summary>
        ///     初始化包体
        /// </summary>
        protected override void PutBody()
        {
            var pic = new Bitmap(FileName);
            var width = pic.Size.Width; // 图片的宽度
            var height = pic.Size.Height; // 图片的高度
            var picBytes = ImageHelper.ImageToBytes(pic);
            var md5 = QQTea.MD5(picBytes);

            var data = new BinaryWriter(new MemoryStream());
            data.Write(new byte[]
            {
                0x01, 0x08, 0x01, 0x12
            });
            data.Write((byte) 0x5A);
            data.Write((byte) 0x08);
            data.Write(Util.HexStringToByteArray(Util.PB_toLength(User.QQ)));
            data.Write((byte) 0x10);
            data.Write(Util.HexStringToByteArray(Util.PB_toLength(ToQQ)));
            data.BeWrite((ushort) 0x1800);
            data.Write((byte) 0x22);
            data.Write((byte) 0x10);
            data.Write(md5);
            data.Write((byte) 0x28);
            data.Write(Util.HexStringToByteArray(Util.PB_toLength(picBytes.Length)));
            data.Write((byte) 0x32);
            data.Write((byte) 0x1A);
            data.Write(new byte[]
            {
                0x57, 0x00, 0x53, 0x00, 0x4E, 0x00, 0x53, 0x00, 0x4C, 0x00, 0x54, 0x00,
                0x31, 0x00, 0x36, 0x00, 0x47, 0x00, 0x45, 0x00, 0x4F, 0x00, 0x5B, 0x00,
                0x5F, 0x00
            });
            data.BeWrite((ushort) 0x3801);
            data.BeWrite((ushort) 0x4801);
            data.Write((byte) 0x70);
            data.Write(Util.HexStringToByteArray(Util.PB_toLength(width)));
            data.Write((byte) 0x78);
            data.Write(Util.HexStringToByteArray(Util.PB_toLength(height)));

            BodyWriter.Write(new byte[]
            {
                0x00, 0x00, 0x00, 0x07
            });
            //数据长度
            BodyWriter.BeWrite(data.BaseStream.Length);
            BodyWriter.Write(new byte[]
            {
                0x08, 0x01, 0x12, 0x03, 0x98, 0x01
            });
            //数据
            BodyWriter.Write(data.BaseStream.ToBytesArray());
        }
    }
}