using System.Drawing;
using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.Send.Message
{
    /// <summary>
    ///     图片处理
    /// </summary>
    public class Send_0X0388 : SendPacket
    {
        public Send_0X0388(QQUser user, TextSnippet fileName, long @group)
            : base(user)
        {
            Sequence = GetNextSeq();
            SecretKey = user.TXProtocol.SessionKey;
            Command = QQCommand.Message0X0388;
            FileName = fileName;
            Group = @group;
        }

        public TextSnippet FileName { get; set; }
        public long Group { get; set; }
        private byte[] _md5 { get; set; }

        public string Md5
        {
            get
            {
                return Util.ToHex(_md5).Replace(" ", "");
            }
        }

        protected override void PutHeader()
        {
            base.PutHeader();
            Writer.Write(new byte[] { 0x04, 0x00, 0x00 });
            Writer.Write(User.TXProtocol.DwClientType);
            Writer.Write(User.TXProtocol.DwPubNo);
            Writer.Write(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
        }

        /// <summary>
        ///     初始化包体
        /// </summary>
        protected override void PutBody()
        {
            var pic = new Bitmap(FileName.Content);
            var width = pic.Size.Width; // 图片的宽度
            var height = pic.Size.Height; // 图片的高度
            var picBytes = ImageHelper.ImageToBytes(pic);
            _md5 = QQTea.MD5(picBytes);

            FileName.Set("Md5", Md5);

            BodyWriter.Write(new byte[] { 0x00, 0x00, 0x00, 0x07 });
            var data = new BinaryWriter(new MemoryStream());
            data.Write(new byte[] { 0x10, 0x01, 0x1A, 0x5A, 0x08 });
            data.Write(Util.HexStringToByteArray(Util.PB_toLength(Group)));
            data.Write((byte)0x10);
            data.Write(Util.HexStringToByteArray(Util.PB_toLength(User.QQ)));
            data.Write(new byte[] { 0x18, 0x00, 0x22, 0x10 });
            data.Write(_md5);
            data.Write((byte)0x28);
            data.Write(Util.HexStringToByteArray(Util.PB_toLength(picBytes.Length)));
            data.Write(new byte[] { 0x32, 0x1a });
            data.Write(new byte[] { 0x50, 0x00, 0x42, 0x00, 0x41, 0x00, 0x42, 0x00, 0x52, 0x00, 0x24, 0x00, 0x4d, 0x00, 0x36, 0x00, 0x5f, 0x00, 0x5a, 0x00, 0x32, 0x00, 0x25, 0x00, 0x39, 0x00 });
            data.Write(new byte[] { 0x38, 0x01, 0x48, 0x01 });
            data.Write((byte)0x50);
            data.Write(Util.HexStringToByteArray(Util.PB_toLength(width)));
            data.Write((byte)0x58);
            data.Write(Util.HexStringToByteArray(Util.PB_toLength(height)));
            data.Write(new byte[] { 0x60, 0x04, 0x6a, 0x05 });
            data.Write(new byte[] { 0x32, 0x36, 0x36, 0x35, 0x32 });
            data.Write(new byte[] { 0x70, 0x00, 0x78, 0x03, 0x80, 0x01, 0x00 });

            //数据长度
            BodyWriter.BeWrite(data.BaseStream.Length);
            BodyWriter.Write(new byte[] { 0x08, 0x01, 0x12, 0x03, 0x98, 0x01, 0x01 });
            //数据
            BodyWriter.Write(data.BaseStream.ToBytesArray());

            //使用之后释放文件
            pic.Dispose();
        }
    }
}