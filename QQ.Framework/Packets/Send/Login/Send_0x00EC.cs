namespace QQ.Framework.Packets.Send.Login
{
    /// <summary>
    ///     改变在线状态
    /// </summary>
    public class Send_0X00Ec : SendPacket
    {
        private readonly byte _loginStatus;

        /// <summary>
        ///     改变在线状态
        /// </summary>
        public Send_0X00Ec(QQUser user, byte loginStatus = LoginStatus.我在线上)
            : base(user)
        {
            Sequence = GetNextSeq();
            SecretKey = user.TXProtocol.SessionKey;
            Command = QQCommand.Login0X00Ec;
            _loginStatus = loginStatus;
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
            BodyWriter.Write(new byte[] {0x01, 0x00});
            BodyWriter.Write(_loginStatus);
            BodyWriter.Write(new byte[] {0x00, 0x01, 0x00, 0x01, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00});
        }
    }
}