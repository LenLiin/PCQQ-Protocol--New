using System;
using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.Send.Message
{
    public class Send_0X0319 : SendPacket
    {
        private readonly long _recvQQ;

        /// <summary>
        /// </summary>
        /// <param name="user"></param>
        public Send_0X0319(QQUser user, long recvQQ, byte[] messageTime)
            : base(user)
        {
            Sequence = GetNextSeq();
            SecretKey = user.TXProtocol.SessionKey;
            Command = QQCommand.Message0X0319;
            _recvQQ = recvQQ;
            MessageTime = messageTime;
        }

        private byte[] MessageTime { get; }

        protected override void PutHeader()
        {
            base.PutHeader();
            Writer.Write(new byte[]
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
            BodyWriter.Write(new byte[] {0x00, 0x00, 0x00, 0x07});
            var data = new BinaryWriter(new MemoryStream());
            data.Write(new byte[] {0x0A, 0x0C, 0x08});
            data.Write(Util.HexStringToByteArray(Util.PB_toLength(_recvQQ)));
            data.Write((byte) 0x10);
            data.Write(
                Util.HexStringToByteArray(
                    Util.PB_toLength(Convert.ToInt64(Util.ToHex(MessageTime).Replace(" ", ""), 16))));
            data.Write(new byte[] {0x20, 0x00});
            //数据长度
            BodyWriter.BeWrite(data.BaseStream.Length);
            BodyWriter.Write(new byte[] {0x08, 0x01, 0x12, 0x03, 0x98, 0x01, 0x00});
            //数据
            BodyWriter.Write(data.BaseStream.ToBytesArray());
        }
    }
}