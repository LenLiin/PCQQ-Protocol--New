namespace QQ.Framework.Packets.Receive.Interactive
{
    public class Receive_0X00A8 : ReceivePacket
    {
        /// <summary>
        /// </summary>
        public Receive_0X00A8(byte[] byteBuffer, QQUser user)
            : base(byteBuffer, user, user.TXProtocol.SessionKey)
        {
        }

        public byte ResultCode { get; set; }

        protected override void ParseBody()
        {
            Decrypt(User.TXProtocol.SessionKey);
            if (GetPacketLength() == 159)
            {
                User.MessageLog("抱歉，由于你操作过于频繁或账户存在不安全因素，添加好友功能暂被停止使用，请稍后再试");
            }
        }
    }
}