using System.Drawing;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.Send.Message
{
    /// <summary>
    ///     图片处理
    /// </summary>
    public class Send_0X0388 : SendPacket
    {
        public Send_0X0388(QQUser user, string fileName, long group)
            : base(user)
        {
            Sequence = GetNextSeq();
            SecretKey = user.TXProtocol.SessionKey;
            Command = QQCommand.Message0X0388;
            FileName = fileName;
            Group = group;
        }

        public string FileName { get; set; }
        public long Group { get; set; }

        protected override void PutHeader()
        {
            base.PutHeader();
            //writer.Write(user.QQ_PACKET_FIXVER);
            Writer.Write(new byte[]
            {
                0x04, 0x00, 0x00, 0x00, 0x01, 0x01, 0x01, 0x00, 0x00, 0x68,
                0x20, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
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
            //var Md5 = Util.GetMD5ToGuidHashFromFile(fileName);
            var picBytes = ImageHelper.ImageToBytes(pic);
            var md5 = QQTea.MD5(picBytes);
            BodyWriter.Write(new byte[]
            {
                0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0x00, 0x5E, 0x08,
                0x01, 0x12, 0x03, 0x98, 0x01, 0x01, 0x10, 0x01, 0x1A
            });
            BodyWriter.Write((byte) 0x5A);
            BodyWriter.Write((byte) 0x08);
            BodyWriter.Write(Util.HexStringToByteArray(Util.PB_toLength(Group)));
            BodyWriter.Write((byte) 0x10);
            BodyWriter.Write(Util.HexStringToByteArray(Util.PB_toLength(User.QQ)));
            BodyWriter.BeWrite((ushort) 0x1800);
            BodyWriter.Write((byte) 0x22);
            BodyWriter.Write((byte) 0x10);
            BodyWriter.Write(md5);
            BodyWriter.Write((byte) 0x28);
            BodyWriter.Write(Util.HexStringToByteArray(Util.PB_toLength(picBytes.Length))); //TODO:
            BodyWriter.Write((byte) 0x32);
            BodyWriter.Write((byte) 0x1A);
            BodyWriter.Write(new byte[]
            {
                0x37, 0x00, 0x4D, 0x00, 0x32, 0x00, 0x25, 0x00, 0x4C,
                0x00, 0x31, 0x00, 0x56, 0x00, 0x32, 0x00, 0x7B, 0x00,
                0x39, 0x00, 0x30, 0x00, 0x29, 0x00, 0x52, 0x00
            });
            BodyWriter.BeWrite((ushort) 0x3801);
            BodyWriter.BeWrite((ushort) 0x4801);
            BodyWriter.Write((byte) 0x50);
            BodyWriter.Write(Util.HexStringToByteArray(Util.PB_toLength(width))); //TODO:
            BodyWriter.Write((byte) 0x58);
            BodyWriter.Write(Util.HexStringToByteArray(Util.PB_toLength(height))); //TODO:
            BodyWriter.BeWrite((ushort) 0x6004);
            BodyWriter.BeWrite((ushort) 0x6A05);
            BodyWriter.Write(new byte[]
            {
                0x32, 0x36, 0x36, 0x35, 0x36
            });
            BodyWriter.BeWrite((ushort) 0x7000);
            BodyWriter.BeWrite((ushort) 0x7803);
            BodyWriter.BeWrite((ushort) 0x8001);
            BodyWriter.Write((byte) 0x00);
        }
    }
}