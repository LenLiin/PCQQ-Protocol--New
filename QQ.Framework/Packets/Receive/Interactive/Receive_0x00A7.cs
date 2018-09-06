namespace QQ.Framework.Packets.Receive.Interactive
{
    public class Receive_0x00A7 : ReceivePacket
    {
        /// <summary>
        ///     
        /// </summary>
        public Receive_0x00A7(byte[] byteBuffer, QQUser User)
            : base(byteBuffer, User, User.QQ_SessionKey)
        {
        }
        public byte resultCode { get; set; }
        protected override void ParseBody()
        {
            Decrypt(user.QQ_SessionKey);
            //响应吗（00：无需验证信息，01：需要验证信息，99：对方已经是你的好友，03或04：添加失败）
            resultCode = (byte)reader.Read();
        }
    }
}