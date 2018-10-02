using QQ.Framework.Utils;

namespace QQ.Framework.Packets.Send.Data
{
    public class Send_0X005C : SendPacket
    {
        public Send_0X005C(QQUser user)
            : base(user)
        {
            Sequence = GetNextSeq();
            SecretKey = user.TXProtocol.SessionKey;
            Command = QQCommand.Data0X005C;
        }

        protected override void PutHeader()
        {
            base.PutHeader();
            Sequence = GetNextSeq();
            SendPACKET_FIX();
        }

        /// <summary>
        ///     初始化包体
        /// </summary>
        protected override void PutBody()
        {
            BodyWriter.Write((byte) 0x88);
            BodyWriter.BeWrite(User.QQ);
            BodyWriter.Write((byte) 0x00);
        }
    }
}