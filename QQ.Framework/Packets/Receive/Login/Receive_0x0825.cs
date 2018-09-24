using QQ.Framework.TlvLib;

namespace QQ.Framework.Packets.Receive.Login
{
    public class Receive_0X0825 : ReceivePacket
    {
        public Receive_0X0825(byte[] byteBuffer, QQUser user)
            : base(byteBuffer, user, user.QQPacket0825Key)
        {
        }

        /// <summary>
        ///     ״̬��
        /// </summary>
        public byte Result { get; set; }

        protected override void ParseBody()
        {
            Decrypt(!User.IsLoginRedirect ? User.QQPacket0825Key : User.QQPacketRedirectionkey);
            Result = Reader.ReadByte();
            var tlvs = Tlv.ParseTlv(Reader.ReadBytes((int) (Reader.BaseStream.Length - 1)));
            Reader.BaseStream.Position = 1;
            TlvExecutionProcessing(tlvs);
        }
    }
}