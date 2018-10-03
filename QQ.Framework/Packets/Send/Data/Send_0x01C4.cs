using System;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.Send.Data
{
    public class Send_0X01C4 : SendPacket
    {
        /// <summary>
        /// </summary>
        /// <param name="user"></param>
        public Send_0X01C4(QQUser user)
            : base(user)
        {
            Sequence = GetNextSeq();
            SecretKey = user.TXProtocol.SessionKey;
            Command = QQCommand.Data0X01C4;
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
            BodyWriter.Write(new byte[] {0x00, 0x01, 0x01, 0x00, 0x00, 0x00, 0x00});
            var data = "{\"t1\":" + Util.GetTimeSeconds(DateTime.Now) + "}";
            BodyWriter.WriteKey(Util.GetBytes(data));
        }
    }
}