using QQ.Framework.Packets.PCTLV;
using QQ.Framework.TlvLib;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.Receive.Login
{
    public class Receive_0x0825 : ReceivePacket
    {
        public Receive_0x0825(byte[] byteBuffer, QQUser User)
            : base(byteBuffer, User, User.QQ_PACKET_0825KEY)
        {
        }
        /// <summary>
        /// ×´Ì¬Âë
        /// </summary>
        public byte Result { get; set; }

        protected override void ParseBody()
        {
            Decrypt(!user.IsLoginRedirect ? user.QQ_PACKET_0825KEY : user.QQ_PACKET_REDIRECTIONKEY);
            Result = reader.ReadByte();
            var tlvs = TlvLib.Tlv.ParseTlv(reader.ReadBytes((int)(reader.BaseStream.Length - 1)));
            reader.BaseStream.Position = 1;
            TlvExecutionProcessing(tlvs);
        }
    }
}