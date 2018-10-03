namespace QQ.Framework.Packets.Send.Data
{
    public class Send_0X01C5 : SendPacket
    {
        /// <summary>
        /// </summary>
        /// <param name="user"></param>
        public Send_0X01C5(QQUser user)
            : base(user)
        {
            Sequence = GetNextSeq();
            SecretKey = user.TXProtocol.SessionKey;
            Command = QQCommand.Data0X01C5;
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
            BodyWriter.Write(new byte[] {0x00, 0x01, 0x00});
        }
    }
}