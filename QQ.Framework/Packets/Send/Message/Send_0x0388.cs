using QQ.Framework.Utils;
using System;
using System.Drawing;
using System.IO;

namespace QQ.Framework.Packets.Send.Message
{
    /// <summary>
    /// 图片处理
    /// </summary>
    public class Send_0x0388 : SendPacket
    {
        public Send_0x0388(QQUser User,string FileName,long Group)
            : base(User)
        {
            Sequence = GetNextSeq();
            _secretKey = user.QQ_SessionKey;
            Command = QQCommand.Message0x0388;
            fileName = FileName;
            group = Group;
        }

        protected override void PutHeader()
        {
            base.PutHeader();
            //writer.Write(user.QQ_PACKET_FIXVER);
            writer.Write(new byte[] {
                0x04, 0x00, 0x00, 0x00, 0x01, 0x01, 0x01, 0x00, 0x00, 0x68,
                0x20, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            });
        }
        public string fileName { get; set; }
        public long group { get; set; }
        /// <summary>
        ///     初始化包体
        /// </summary>
        protected override void PutBody()
        {
            Bitmap pic = new Bitmap(fileName);
            int Width = pic.Size.Width; // 图片的宽度
            int Height = pic.Size.Height; // 图片的高度
            //var Md5 = Util.GetMD5ToGuidHashFromFile(fileName);
            var picBytes = ImageHelper.ImageToBytes(pic);
            var Md5 = QQTea.MD5(picBytes);
            bodyWriter.Write(new byte[]{
                0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0x00, 0x5E, 0x08,
                0x01, 0x12, 0x03, 0x98, 0x01, 0x01, 0x10, 0x01, 0x1A
            });
            bodyWriter.Write((byte)0x5A);
            bodyWriter.Write((byte)0x08);
            bodyWriter.Write(Util.HexStringToByteArray(Util.PB_toLength(group)));
            bodyWriter.Write((byte)0x10);
            bodyWriter.Write(Util.HexStringToByteArray(Util.PB_toLength(user.QQ)));
            bodyWriter.BEWrite((ushort)0x1800);
            bodyWriter.Write((byte)0x22);
            bodyWriter.Write((byte)0x10);
            bodyWriter.Write(Md5);
            bodyWriter.Write((byte)0x28);
            bodyWriter.Write(Util.HexStringToByteArray(Util.PB_toLength(picBytes.Length)));//TODO:
            bodyWriter.Write((byte)0x32);
            bodyWriter.Write((byte)0x1A);
            bodyWriter.Write(new byte[]{
                0x37, 0x00, 0x4D, 0x00, 0x32, 0x00, 0x25, 0x00, 0x4C,
                0x00, 0x31, 0x00, 0x56, 0x00, 0x32, 0x00, 0x7B, 0x00,
                0x39, 0x00, 0x30, 0x00, 0x29, 0x00, 0x52, 0x00
            });
            bodyWriter.BEWrite((ushort)0x3801);
            bodyWriter.BEWrite((ushort)0x4801);
            bodyWriter.Write((byte)0x50);
            bodyWriter.Write(Util.HexStringToByteArray(Util.PB_toLength(Width)));//TODO:
            bodyWriter.Write((byte)0x58);
            bodyWriter.Write(Util.HexStringToByteArray(Util.PB_toLength(Height)));//TODO:
            bodyWriter.BEWrite((ushort)0x6004);
            bodyWriter.BEWrite((ushort)0x6A05);
            bodyWriter.Write(new byte[]{
                0x32,0x36,0x36,0x35,0x36
            });
            bodyWriter.BEWrite((ushort)0x7000);
            bodyWriter.BEWrite((ushort)0x7803);
            bodyWriter.BEWrite((ushort)0x8001);
            bodyWriter.Write((byte)0x00);
        }
    }
}