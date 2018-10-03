namespace QQ.Framework.Packets.Send.Data
{
    public class Send_0X00D8 : SendPacket
    {
        /// <summary>
        ///     问问个人中心地址
        /// </summary>
        /// <param name="user"></param>
        public Send_0X00D8(QQUser user)
            : base(user)
        {
            Sequence = GetNextSeq();
            SecretKey = user.TXProtocol.SessionKey;
            Command = QQCommand.Data0X00D8;
        }

        protected override void PutHeader()
        {
            base.PutHeader();
            SendPACKET_FIX();
        }

        /// <summary>
        ///     初始化包体
        /// </summary>
        protected override void PutBody()
        {
            BodyWriter.Write((byte) 0x07);
        }
    }
}