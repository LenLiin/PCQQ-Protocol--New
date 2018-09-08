using System;
using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.Send.Message
{
    public class Send_0x0360 : SendPacket
    {
        private readonly long recvGroupQQ;

        /// <summary>
        /// </summary>
        /// <param name="User"></param>
        public Send_0x0360(QQUser User, long RecvGroupQQ, byte[] messageTime)
            : base(User)
        {
            Sequence = GetNextSeq();
            _secretKey = user.QQ_SessionKey;
            Command = QQCommand.Message0x0360;
            _messageTime = messageTime;
            recvGroupQQ = RecvGroupQQ;
        }

        private byte[] _messageTime { get; }

        protected override void PutHeader()
        {
            base.PutHeader();
            writer.Write(new byte[]
            {
                0x04, 0x00, 0x00, 0x00, 0x01, 0x01, 0x01, 0x00, 0x00, 0x68, 0x1C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00
            });
        }

        /// <summary>
        ///     初始化包体
        /// </summary>
        protected override void PutBody()
        {
            bodyWriter.Write(new byte[] {0x00, 0x00, 0x00, 0x07});
            var _Data = new BinaryWriter(new MemoryStream());
            _Data.Write(new byte[] {0x03, 0x08, 0x03, 0x22, 0x46, 0x08});
            _Data.Write(Util.HexStringToByteArray(Util.PB_toLength(recvGroupQQ)));
            _Data.Write((byte) 0x10);
            _Data.Write(
                Util.HexStringToByteArray(
                    Util.PB_toLength(Convert.ToInt64(Util.ToHex(_messageTime).Replace(" ", ""), 16))));
            _Data.Write(new byte[] {0x20, 0x00});
            //数据长度
            bodyWriter.BEWrite(_Data.BaseStream.Length);
            bodyWriter.Write(new byte[] {0x08, 0x01, 0x12, 0x03, 0x98, 0x01, 0x00});
            //数据
            bodyWriter.Write(_Data.BaseStream.ToBytesArray());
        }
    }
}