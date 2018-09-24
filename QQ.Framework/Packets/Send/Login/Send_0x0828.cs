using QQ.Framework.Packets.PCTLV;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.Send.Login
{
    public class Send_0x0828 : SendPacket
    {
        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="User"></param>
        public Send_0x0828(QQUser User)
            : base(User)
        {
            Sequence = GetNextSeq();
            _secretKey = user.TXProtocol.bufSessionKey;
            Command = QQCommand.Login0x0828;
        }

        protected override void PutHeader()
        {
            base.PutHeader();
            writer.Write(new byte[] { 0x02, 0x00, 0x00 });
            writer.Write(user.TXProtocol.dwClientType);
            writer.Write(user.TXProtocol.dwPubNo);
            writer.Write(new byte[] { 0x00, 0x30, 0x00, 0x3a});
            writer.WriteKey(user.TXProtocol.bufSigSession);
        }

        /// <summary>
        ///     初始化包体
        /// </summary>
        protected override void PutBody()
        {
            bodyWriter.Write(new TLV_0007().Get_Tlv(user));
            bodyWriter.Write(new TLV_000C().Get_Tlv(user));
            bodyWriter.Write(new TLV_0015().Get_Tlv(user));
            bodyWriter.Write(new TLV_0036().Get_Tlv(user));
            bodyWriter.Write(new TLV_0018().Get_Tlv(user));
            bodyWriter.Write(new TLV_001F().Get_Tlv(user));
            bodyWriter.Write(new TLV_0105().Get_Tlv(user));
            bodyWriter.Write(new TLV_010B().Get_Tlv(user));
            bodyWriter.Write(new TLV_002D().Get_Tlv(user));
        }
    }
}