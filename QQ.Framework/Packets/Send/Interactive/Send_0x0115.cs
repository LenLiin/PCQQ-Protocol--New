using QQ.Framework.Utils;

namespace QQ.Framework.Packets.Send.Interactive
{
    public class Send_0X0115 : SendPacket
    {
        /// <summary>
        /// </summary>
        /// <param name="user"></param>
        /// <param name="addQQ"></param>
        public Send_0X0115(QQUser user, long addQQ)
            : base(user)
        {
            Sequence = GetNextSeq();
            SecretKey = user.TXProtocol.SessionKey;
            Command = QQCommand.Interactive0X00Ae;
            AddQQ = addQQ;
        }

        public long AddQQ { get; set; }

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
            BodyWriter.Write(new byte[]
            {
                0x03
            });
            BodyWriter.BeWrite(AddQQ);
        }
    }
}