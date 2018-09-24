namespace QQ.Framework.Packets.Receive.Interactive
{
    public class Receive_0X00A7 : ReceivePacket
    {
        /// <summary>
        /// </summary>
        public Receive_0X00A7(byte[] byteBuffer, QQUser user)
            : base(byteBuffer, user, user.TXProtocol.SessionKey)
        {
        }

        public byte ResultCode { get; set; }

        protected override void ParseBody()
        {
            Decrypt(User.TXProtocol.SessionKey);
            //响应吗（00：无需验证信息，01：需要验证信息，99：对方已经是你的好友，03或04：添加失败）
            ResultCode = (byte) Reader.Read();
        }
    }
}