using QQ.Framework.Utils;

namespace QQ.Framework.Packets.Receive.Message
{
    /// <summary>
    ///     好友消息
    /// </summary>
    public class Receive_0X00Ce : ReceivePacket
    {
        /// <summary>
        ///     好友消息
        /// </summary>
        public Receive_0X00Ce(byte[] byteBuffer, QQUser user)
            : base(byteBuffer, user, user.TXProtocol.SessionKey)
        {
        }

        /// <summary>
        ///     消息来源QQ
        /// </summary>
        public long FromQQ { get; set; }

        /// <summary>
        ///     消息类型
        /// </summary>
        public byte[] MessageType { get; set; }

        /// <summary>
        ///     消息时间
        /// </summary>
        public byte[] MessageDateTime { get; set; }

        /// <summary>
        ///     消息字体
        /// </summary>
        public byte[] FontStyle { get; set; }

        /// <summary>
        ///     消息内容
        /// </summary>
        public Richtext Message { get; set; } = "";

        protected override void ParseBody()
        {
            Decrypt(User.TXProtocol.SessionKey);
            FromQQ = (long) Util.GetQQNumRetUint(Util.ToHex(Reader.ReadBytes(4)));
            Reader.ReadBytes(4); //自己的QQ
            Reader.ReadBytes(10);
            MessageType = Reader.ReadBytes(2);
            Reader.BeReadChar();
            Reader.ReadBytes(Reader.BeReadChar()); //未知
            Reader.BeReadChar(); //消息来源QQ的版本号
            Reader.ReadBytes(4); //FromQQ
            Reader.ReadBytes(4); //自己的QQ
            Reader.ReadBytes(20);
            MessageDateTime = Reader.ReadBytes(4);
            Reader.BeReadChar(); //00
            Reader.ReadBytes(4); //MessageDateTime
            Reader.ReadBytes(5); //00
            Reader.ReadBytes(3);
            Reader.ReadBytes(5); //00
            Reader.ReadBytes(4); //MessageDateTime
            Reader.ReadBytes(4);
            Reader.ReadBytes(8);
            FontStyle = Reader.ReadBytes(Reader.BeReadChar());
            Reader.ReadBytes(6);
            Message = Reader.ReadRichtext();
            Reader.ReadBytes(22);
        }
    }
}