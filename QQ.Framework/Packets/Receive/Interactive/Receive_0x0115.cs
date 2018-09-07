namespace QQ.Framework.Packets.Receive.Interactive
{
    public class Receive_0x0115 : ReceivePacket
    {
        /// <summary>
        ///     发送添加好友消息回执
        /// </summary>
        public Receive_0x0115(byte[] byteBuffer, QQUser User)
            : base(byteBuffer, User, User.QQ_SessionKey)
        {
        }

        public AddFriendType addFriendType { get; set; }

        protected override void ParseBody()
        {
            Decrypt(user.QQ_SessionKey);
            reader.ReadBytes(28);
            user.AddFriend_0018Value = reader.ReadBytes(24);
        }
    }
}