using System;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.Send.Data
{
    public class Send_0X0134 : SendPacket
    {
        /// <summary>
        ///     获取好友和群列表
        /// </summary>
        /// <param name="user"></param>
        public Send_0X0134(QQUser user)
            : base(user)
        {
            Sequence = GetNextSeq();
            SecretKey = user.TXProtocol.SessionKey;
            Command = QQCommand.Data0X0134;
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
            BodyWriter.BeWrite(0x0000000C);
            BodyWriter.BeWrite(Util.GetTimeSeconds(DateTime.Now));
            BodyWriter.BeWrite(0x000003E8);
            BodyWriter.WriteKey(new byte[]
                {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00});
        }
    }
}