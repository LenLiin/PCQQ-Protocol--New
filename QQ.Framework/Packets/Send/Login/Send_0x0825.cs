using QQ.Framework.Packets.PCTLV;

namespace QQ.Framework.Packets.Send.Login
{
    public class Send_0X0825 : SendPacket
    {
        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="user"></param>
        /// <param name="redirect">是否是重定向包</param>
        public Send_0X0825(QQUser user, bool redirect)
            : base(user)
        {
            Sequence = GetNextSeq();
            Redirect = redirect;
            SecretKey = !redirect ? User.QQPacket0825Key : User.QQPacketRedirectionkey;
            Command = QQCommand.Login0X0825;
        }

        /// <summary>
        ///     重定向标识
        /// </summary>
        private bool Redirect { get; }

        public override string GetPacketName()
        {
            return "登录包0x0825（Ping）";
        }

        protected override void PutHeader()
        {
            base.PutHeader();
            SendPACKET_FIX();
            Writer.Write(SecretKey);
        }

        /// <summary>
        ///     初始化包体
        /// </summary>
        protected override void PutBody()
        {
            BodyWriter.Write(new TLV0018().Get_Tlv(User));
            BodyWriter.Write(new TLV0309().Get_Tlv(User));
            BodyWriter.Write(new TLV0036().Get_Tlv(User));
            BodyWriter.Write(new TLV0114().Get_Tlv(User));
        }
    }
}