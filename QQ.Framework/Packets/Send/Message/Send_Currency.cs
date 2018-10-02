namespace QQ.Framework.Packets.Send.Message
{
    public class SendCurrency : SendPacket
    {
        /// <summary>
        ///     通用响应包
        /// </summary>
        /// <param name="user"></param>
        /// <param name="data">要发送的数据内容</param>
        /// <param name="sequence">序号</param>
        public SendCurrency(QQUser user, byte[] data, char sequence, char command)
            : base(user)
        {
            Sequence = sequence;
            SecretKey = user.TXProtocol.SessionKey;
            Command = (QQCommand) command;
            Data = data;
        }

        private byte[] Data { get; }

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
            BodyWriter.Write(Data);
        }
    }
}