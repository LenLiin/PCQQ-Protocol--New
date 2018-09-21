using QQ.Framework.Utils;

namespace QQ.Framework.Packets.Send.Message
{
    /// <summary>
    ///     心跳
    /// </summary>
    public class Send_0x0058 : SendPacket
    {
        public Send_0x0058(QQUser User)
            : base(User)
        {
            Sequence = GetNextSeq();
            _secretKey = User.TXProtocol.SessionKey;
            Command = QQCommand.Message0x0058;
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
            bodyWriter.BEWrite(user.QQ);
        }
    }
}