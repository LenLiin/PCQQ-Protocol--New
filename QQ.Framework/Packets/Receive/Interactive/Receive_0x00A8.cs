namespace QQ.Framework.Packets.Receive.Interactive
{
    public class Receive_0x00A8 : ReceivePacket
    {
        /// <summary>
        ///     
        /// </summary>
        public Receive_0x00A8(byte[] byteBuffer, QQUser User)
            : base(byteBuffer, User, User.QQ_SessionKey)
        {
        }
        public byte resultCode { get; set; }
        protected override void ParseBody()
        {
            Decrypt(user.QQ_SessionKey);
            if (GetPacketLength() == 159)
            {
                user.MessageLog("抱歉，由于你操作过于频繁或账户存在不安全因素，添加好友功能暂被停止使用，请稍后再试");
            }
        }
    }
}