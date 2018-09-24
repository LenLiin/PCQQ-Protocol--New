using QQ.Framework.TlvLib;

namespace QQ.Framework.Packets.Receive.Login
{
    public class Receive_0X0828 : ReceivePacket
    {
        public Receive_0X0828(byte[] byteBuffer, QQUser user)
            : base(byteBuffer, user, user.TXProtocol.BufTgtGtKey)
        {
        }

        /// <summary>
        ///     状态码
        /// </summary>
        public byte Result { get; set; }

        protected override void ParseBody()
        {
            Decrypt(SecretKey);
            Result = Reader.ReadByte();
            var tlvs = Tlv.ParseTlv(Reader.ReadBytes((int) (Reader.BaseStream.Length - 1)));
            //重置指针（因为tlv解包后指针已经移动到末尾）
            Reader.BaseStream.Position = 1;
            TlvExecutionProcessing(tlvs);
        }
    }
}