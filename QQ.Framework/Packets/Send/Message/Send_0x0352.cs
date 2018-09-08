using QQ.Framework.Utils;
using System;
using System.Drawing;
using System.IO;

namespace QQ.Framework.Packets.Send.Message
{
    /// <summary>
    /// 私聊图片获取Ukey
    /// </summary>
    public class Send_0x0352 : SendPacket
    {
        public Send_0x0352(QQUser User,string FileName,long ToQQ)
            : base(User)
        {
            Sequence = GetNextSeq();
            _secretKey = user.QQ_SessionKey;
            Command = QQCommand.Message0x0352;
            fileName = FileName;
            _toQQ = ToQQ;
        }

        protected override void PutHeader()
        {
            base.PutHeader();
            //writer.Write(user.QQ_PACKET_FIXVER);
            writer.Write(new byte[] {
                0x04,0x00,0x00,0x00,0x01,0x01,0x01,0x00,0x00,0x68,
                0x1C,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
            });
        }
        public string fileName { get; set; }
        public long _toQQ { get; set; }
        /// <summary>
        ///     初始化包体
        /// </summary>
        protected override void PutBody()
        {
            Bitmap pic = new Bitmap(fileName);
            int Width = pic.Size.Width; // 图片的宽度
            int Height = pic.Size.Height; // 图片的高度
            var picBytes = ImageHelper.ImageToBytes(pic);
            var Md5 = QQTea.MD5(picBytes);

            BinaryWriter _Data = new BinaryWriter(new MemoryStream());
            _Data.Write(new byte[]{
                 0x01, 0x08, 0x01, 0x12
            });
            _Data.Write((byte)0x5A);
            _Data.Write((byte)0x08);
            _Data.Write(Util.HexStringToByteArray(Util.PB_toLength(user.QQ)));
            _Data.Write((byte)0x10);
            _Data.Write(Util.HexStringToByteArray(Util.PB_toLength(_toQQ)));
            _Data.BEWrite((ushort)0x1800);
            _Data.Write((byte)0x22);
            _Data.Write((byte)0x10);
            _Data.Write(Md5);
            _Data.Write((byte)0x28);
            _Data.Write(Util.HexStringToByteArray(Util.PB_toLength(picBytes.Length)));
            _Data.Write((byte)0x32);
            _Data.Write((byte)0x1A);
            _Data.Write(new byte[]{
                0x57, 0x00, 0x53, 0x00, 0x4E, 0x00, 0x53, 0x00, 0x4C, 0x00, 0x54, 0x00,
                0x31, 0x00, 0x36, 0x00, 0x47, 0x00, 0x45, 0x00, 0x4F, 0x00, 0x5B, 0x00,
                0x5F, 0x00 });
            _Data.BEWrite((ushort)0x3801);
            _Data.BEWrite((ushort)0x4801);
            _Data.Write((byte)0x70);
            _Data.Write(Util.HexStringToByteArray(Util.PB_toLength(Width)));
            _Data.Write((byte)0x78);
            _Data.Write(Util.HexStringToByteArray(Util.PB_toLength(Height)));

            bodyWriter.Write(new byte[]{
                0x00, 0x00, 0x00, 0x07
            });
            //数据长度
            bodyWriter.BEWrite(_Data.BaseStream.Length);
            bodyWriter.Write(new byte[]{
                 0x08, 0x01, 0x12, 0x03, 0x98, 0x01
            });
            //数据
            bodyWriter.Write(_Data.BaseStream.ToBytesArray());
        }
    }
}