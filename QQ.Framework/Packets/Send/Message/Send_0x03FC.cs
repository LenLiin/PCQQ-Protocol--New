using System;
using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.Send.Message
{
    public class Send_0X03FC : SendPacket
    {
        private readonly long _recvQQ;
        private readonly char _msgSequence;

        /// <summary>
        /// </summary>
        /// <param name="user"></param>
        public Send_0X03FC(QQUser user, long recvQQ, byte[] messageTime, char msgSequence, byte[] messageId)
            : base(user)
        {
            Sequence = GetNextSeq();
            SecretKey = user.TXProtocol.SessionKey;
            Command = QQCommand.Message0X03FC;
            _recvQQ = recvQQ;
            MessageTime = messageTime;
            _msgSequence = msgSequence;
            MessageId = messageId;
        }

        private byte[] MessageTime { get; }
        private byte[] MessageId { get; }

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
            BodyWriter.Write(new byte[] { 0x00, 0x00, 0x00, 0x07 });
            var data = new BinaryWriter(new MemoryStream());
            var data_0A = new BinaryWriter(new MemoryStream());
            data_0A.Write((byte) 0x08);
            data_0A.Write(Util.HexStringToByteArray(Util.PB_toLength(User.QQ)));
            data_0A.Write((byte) 0x10);
            data_0A.Write(Util.HexStringToByteArray(Util.PB_toLength(_recvQQ)));
            data_0A.Write((byte) 0x18);
            data_0A.Write(Util.HexStringToByteArray(Util.PB_toLength(_msgSequence))); //消息包序
            data_0A.Write((byte) 0x28);
            data_0A.Write(
                Util.HexStringToByteArray(
                    Util.PB_toLength(Convert.ToInt64(Util.ToHex(MessageTime).Replace(" ", ""), 16))));
            data_0A.Write(new byte[] { 0x30 });
            data_0A.Write(
                Util.HexStringToByteArray(
                    Util.PB_toLength(Convert.ToInt64(Util.ToHex(MessageId).Replace(" ", ""), 16)))); //消息Id
            data_0A.Write(new byte[] { 0x38, 0x01, 0x40, 0x00, 0x48, 0x00 });
            data.Write((byte) 0x0A);
            data.Write((byte) data_0A.BaseStream.Length); //length
            data.Write(data_0A.BaseStream.ToBytesArray());
            data.Write(new byte[] { 0x10, 0x01, 0x18, 0x01, 0x20, 0x00, 0x2a, 0x04, 0x08, 0x00, 0x10, 0x00 });
            //数据长度
            BodyWriter.BeWrite(data.BaseStream.Length);
            BodyWriter.Write(new byte[] { 0x08, 0x01, 0x12, 0x03, 0x98, 0x01, 0x00 });
            //数据
            BodyWriter.Write(data.BaseStream.ToBytesArray());
        }
    }
}