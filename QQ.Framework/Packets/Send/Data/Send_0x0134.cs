using QQ.Framework.Utils;
using System;
namespace QQ.Framework.Packets.Send.Data
{
    public class Send_0X0134 : SendPacket
    {
        /// <summary>
        /// 获取好友和群列表
        /// </summary>
        /// <param name="User"></param>
        public Send_0X0134(QQUser User)
            : base(User)
        {
            Sequence = GetNextSeq();
            SecretKey = User.TXProtocol.SessionKey;
            Command = QQCommand.Data0X0134;
        }

        protected override void PutHeader()
        {
            base.PutHeader();
            Writer.Write(User.QQPacketFixver);
        }

        /// <summary>
        ///     初始化包体
        /// </summary>
        protected override void PutBody()
        {
            BodyWriter.BeWrite(0x0000000C);
            BodyWriter.BeWrite(Util.GetTimeSeconds(DateTime.Now));
            BodyWriter.BeWrite(0x000003E8);
            BodyWriter.WriteKey(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
        }
    }
}