using QQ.Framework.Utils;

namespace QQ.Framework.Packets.Receive.Message
{
    public class Receive_0X0017 : ReceivePacket
    {
        /// <summary>
        ///     群消息
        /// </summary>
        public Receive_0X0017(byte[] byteBuffer, QQUser user)
            : base(byteBuffer, user, user.TXProtocol.SessionKey)
        {
        }

        /// <summary>
        ///     消息来源QQ
        /// </summary>
        public long FromQQ { get; set; }

        /// <summary>
        ///     群号
        /// </summary>
        public long Group { get; set; }

        /// <summary>
        ///     消息类型
        /// </summary>
        public char MessageType { get; set; }

        /// <summary>
        ///     消息
        /// </summary>
        public Richtext Message { get; set; } = "";

        /// <summary>
        ///     字体
        /// </summary>
        public byte[] Font { get; set; }

        /// <summary>
        ///     接收时间
        /// </summary>
        public byte[] ReceiveTime { get; set; }

        /// <summary>
        ///     发送时间
        /// </summary>
        public byte[] SendTime { get; set; }

        protected override void ParseBody()
        {
            Decrypt(User.TXProtocol.SessionKey);
            Reader.ReadBytes(4);
            Reader.ReadBytes(4); //自己的QQ
            Reader.ReadBytes(10);
            MessageType = Reader.BeReadChar(); //消息类型
            if (MessageType == (char) 0x0052)
            {
                Reader.ReadBytes(2);
                Reader.ReadBytes(Reader.BeReadChar());
                Group = (long) Util.GetQQNumRetUint(Util.ToHex(Reader.ReadBytes(4))); //群号
                Reader.ReadByte();
                FromQQ = (long) Util.GetQQNumRetUint(Util.ToHex(Reader.ReadBytes(4))); //发消息人的QQ
                Reader.ReadBytes(4);
                ReceiveTime = Reader.ReadBytes(4); //接收时间  
                Reader.ReadBytes(24);
                SendTime = Reader.ReadBytes(4); //发送时间  
                Reader.ReadBytes(12);
                Font = Reader.ReadBytes(Reader.BeReadChar()); //字体
                Reader.ReadBytes(6);
                Message = Reader.ReadRichtext(); //消息
                Reader.ReadBytes(58);
                Reader.ReadBytes(Reader.BeReadChar()); //消息
                Reader.ReadBytes(11);
            }
            else if (MessageType == (char) 0x0058)
            {
            }
        }
    }
}