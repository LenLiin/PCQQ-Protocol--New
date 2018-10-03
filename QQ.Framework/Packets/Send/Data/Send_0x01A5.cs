namespace QQ.Framework.Packets.Send.Data
{
    public class Send_0X01A5 : SendPacket
    {
        /// <summary>
        ///     查询黑名单
        /// </summary>
        /// <param name="user"></param>
        public Send_0X01A5(QQUser user)
            : base(user)
        {
            Sequence = GetNextSeq();
            SecretKey = user.TXProtocol.SessionKey;
            Command = QQCommand.Data0X01A5;
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
            BodyWriter.Write(new byte[] {0x01, 0x00, 0x00, 0x00, 0x0c, 0x01, 0x00, 0x00});
        }
    }
}