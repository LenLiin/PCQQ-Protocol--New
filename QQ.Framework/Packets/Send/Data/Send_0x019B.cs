namespace QQ.Framework.Packets.Send.Data
{
    public class Send_0X019B : SendPacket
    {
        /// <summary>
        /// 获取群分组信息
        /// </summary>
        /// <param name="user"></param>
        public Send_0X019B(QQUser user)
            : base(user)
        {
            Sequence = GetNextSeq();
            SecretKey = user.TXProtocol.SessionKey;
            Command = QQCommand.Data0X019B;
        }

        protected override void PutHeader()
        {
            base.PutHeader();
            Writer.Write(User.QQPacketFixver);
        }

        /// <summary>
        ///     初始化包体
        /// </summary>
        protected override void PutBody()
        {
            BodyWriter.Write(new byte[] { 0x01, 0x01, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00 });
        }
    }
}