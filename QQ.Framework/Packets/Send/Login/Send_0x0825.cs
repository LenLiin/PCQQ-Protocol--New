using QQ.Framework.Packets.PCTLV;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.Send.Login
{
    public class Send_0x0825 : SendPacket
    {
        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="User"></param>
        /// <param name="Redirect">是否是重定向包</param>
        public Send_0x0825(QQUser User, bool Redirect)
            : base(User)
        {
            Sequence = GetNextSeq();
            redirect = Redirect;
            _secretKey = !Redirect ? user.QQ_PACKET_0825KEY : user.QQ_PACKET_REDIRECTIONKEY;
            Command = QQCommand.Login0x0825;
        }

        /// <summary>
        ///     重定向标识
        /// </summary>
        private bool redirect { get; }

        public override string GetPacketName()
        {
            return "登录包0x0825（Ping）";
        }

        protected override void PutHeader()
        {
            base.PutHeader();
            SendPACKET_FIX();
            writer.Write(_secretKey);
        }

        /// <summary>
        ///     初始化包体
        /// </summary>
        protected override void PutBody()
        {
            bodyWriter.Write(new TLV_0018().Get_Tlv(user));
            bodyWriter.Write(new TLV_0309().Get_Tlv(user));
            bodyWriter.Write(new TLV_0036().Get_Tlv(user));
            bodyWriter.Write(new TLV_0114().Get_Tlv(user));
        }
    }
}