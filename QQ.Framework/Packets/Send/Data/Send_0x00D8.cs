namespace QQ.Framework.Packets.Send.Data
{
    public class Send_0X00D8 : SendPacket
    {
        /// <summary>
        /// 问问个人中心地址
        /// </summary>
        /// <param name="User"></param>
        public Send_0X00D8(QQUser User)
            : base(User)
        {
            Sequence = GetNextSeq();
            SecretKey = User.TXProtocol.SessionKey;
            Command = QQCommand.Data0X00D8;
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
            BodyWriter.Write((byte)0x07);
        }
    }
}