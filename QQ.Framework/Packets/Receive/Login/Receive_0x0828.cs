using QQ.Framework.Utils;
using System;

namespace QQ.Framework.Packets.Receive.Login
{
    public class Receive_0x0828 : ReceivePacket
    {
        public Receive_0x0828(byte[] byteBuffer, QQUser User)
            : base(byteBuffer, User, User.TXProtocol.bufTGT_GTKey)
        {
        }

        /// <summary>
        /// 状态码
        /// </summary>
        public byte Result { get; set; }
        protected override void ParseBody()
        {
            Decrypt(_secretKey);
            Result = reader.ReadByte();
            var tlvs = TlvLib.Tlv.ParseTlv(reader.ReadBytes((int)(reader.BaseStream.Length - 1)));
            //重置指针（因为tlv解包后指针已经移动到末尾）
            reader.BaseStream.Position = 1;
            TlvExecutionProcessing(tlvs);
        }
    }
}