namespace QQ.Framework.Packets.Send.Login
{
    /// <summary>
    ///     改变在线状态
    /// </summary>
    public class Send_0x00EC : SendPacket
    {
        private readonly byte _loginStatus;

        /// <summary>
        ///     改变在线状态
        /// </summary>
        public Send_0x00EC(QQUser User, byte loginStatus = LoginStatus.我在线上)
            : base(User)
        {
            Sequence = GetNextSeq();
            _secretKey = User.TXProtocol.SessionKey;
            Command = QQCommand.Login0x00EC;
            _loginStatus = loginStatus;
        }

        protected override void PutHeader()
        {
            base.PutHeader();
            writer.Write(user.QQ_PACKET_FIXVER);
        }

        /// <summary>
        ///     初始化包体
        /// </summary>
        protected override void PutBody()
        {
            bodyWriter.Write(new byte[] {0x01, 0x00});
            bodyWriter.Write(_loginStatus);
            bodyWriter.Write(new byte[] {0x00, 0x01, 0x00, 0x01, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00});
        }
    }
}