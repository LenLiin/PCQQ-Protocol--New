namespace QQ.Framework.Packets.Receive.Interactive
{
    public class Receive_0X0115 : ReceivePacket
    {
        /// <summary>
        ///     发送添加好友消息回执
        /// </summary>
        public Receive_0X0115(byte[] byteBuffer, QQUser user)
            : base(byteBuffer, user, user.TXProtocol.SessionKey)
        {
        }

        public AddFriendType AddFriendType { get; set; }

        protected override void ParseBody()
        {
            Decrypt(User.TXProtocol.SessionKey);
            Reader.ReadBytes(28);
            User.AddFriend0018Value = Reader.ReadBytes(24);
        }
    }
}