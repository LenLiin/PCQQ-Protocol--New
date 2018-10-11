using System;
using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.Send.Message
{
    public class Send_0X03F7 : SendPacket
    {
        private readonly long _groupId;
        private readonly char _msgSequence;
        private readonly byte[] _messageId;
        private readonly byte[] _messageIndex;

        /// <summary>
        ///     撤回群消息
        /// </summary>
        /// <param name="user"></param>
        public Send_0X03F7(QQUser user, long groupId, byte[] messageId, byte[] messageIndex)
            : base(user)
        {
            Sequence = GetNextSeq();
            SecretKey = user.TXProtocol.SessionKey;
            Command = QQCommand.Message0X03F7;
            _groupId = groupId;
            _messageId = messageId;
            _messageIndex = messageIndex;
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
            var pbGroupId = Util.HexStringToByteArray(Util.PB_toLength(_groupId));
            var pbMessageId =
                Util.HexStringToByteArray(
                    Util.PB_toLength(Convert.ToInt64(Util.ToHex(_messageId).Replace(" ", ""), 16)));
            var pbMessageIndex =
                Util.HexStringToByteArray(
                    Util.PB_toLength(Convert.ToInt64(Util.ToHex(_messageIndex).Replace(" ", ""), 16)));
            BodyWriter.Write(new byte[] { 0x00, 0x00, 0x00, 0x07 });
            var data = new BinaryWriter(new MemoryStream());
            data.Write(new byte[] { 0x08, 0x01, 0x10, 0x00, 0x18 });
            data.Write(pbGroupId);
            data.Write(new byte[] { 0x22, 0x09, 0x08 });
            data.Write(pbMessageIndex); //消息索引
            data.Write((byte) 0x10);
            data.Write(pbMessageId); //消息Id
            var data_2A = new BinaryWriter(new MemoryStream());
            data_2A.Write(new byte[] { 0x08, 0x00 });
            var data12 = new BinaryWriter(new MemoryStream());
            data12.Write((byte) 0x08);
            data12.Write(pbMessageIndex); //消息索引
            data12.Write((byte) 0x10);
            data12.Write(new byte[] { 0x00, 0x18, 0x01, 0x20, 0x00 });
            data_2A.Write((byte) 0x12);
            data_2A.Write((byte) data12.BaseStream.Length);
            data_2A.Write(data12.BaseStream.ToBytesArray());
            data.Write((byte) 0x2a);
            data.Write((byte) data_2A.BaseStream.Length);
            data.Write(data_2A.BaseStream.ToBytesArray());
            //数据长度
            BodyWriter.BeWrite(data.BaseStream.Length);
            BodyWriter.Write(new byte[] { 0x08, 0x01, 0x12, 0x03, 0x98, 0x01, 0x00 });
            //数据
            BodyWriter.Write(data.BaseStream.ToBytesArray());
        }
    }
}