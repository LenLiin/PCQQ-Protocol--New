using System.IO;
using System.Linq;
using QQ.Framework.Packets.PCTLV;
using QQ.Framework.TlvLib;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.Receive.Login
{
    public class Receive_0x0836 : ReceivePacket
    {
        public Receive_0x0836(byte[] byteBuffer, QQUser User)
            : base(byteBuffer, User, User.TXProtocol.bufTGTGTKey)
        {
        }

        public string ErrorMsg { get; set; }

        /// <summary>
        ///     状态码
        /// </summary>
        public byte Result { get; set; }

        protected override void ParseBody()
        {
            Decrypt(user.TXProtocol.bufDHShareKey);
            Result = reader.ReadByte();
            //返回错误
            if (Result == (byte) ResultCode.DoMain || Result == (byte) ResultCode.其它错误
                || Result == (byte) ResultCode.密码错误 || Result == (byte) ResultCode.帐号被回收
                || Result == (byte) ResultCode.要求切换TCP || Result == (byte) ResultCode.过载保护
                || Result == (byte) ResultCode.需要验证密保 || Result == (byte) ResultCode.需要验证码)
            {
                var tlvs = Tlv.ParseTlv(reader.ReadBytes((int) (reader.BaseStream.Length - 1)));
                //重置指针（因为tlv解包后指针已经移动到末尾）
                reader.BaseStream.Position = 1;
                TlvExecutionProcessing(tlvs);
                if (tlvs.Any(c => c.Tag == 0x0100))
                {
                    var errorData = tlvs.FirstOrDefault(c => c.Tag == 0x0100);
                    var ErrReader = new BinaryReader(new MemoryStream(errorData.Value));
                    var tlv = new TLV_0100();
                    tlv.Parser_Tlv2(user, ErrReader, errorData.Length);
                    ErrorMsg = tlv.ErrorMsg;
                }
            }
            else
            {
                bodyDecrypted = QQTea.Decrypt(bodyDecrypted, user.TXProtocol.bufTGTGTKey);
                reader = new BinaryReader(new MemoryStream(bodyDecrypted));
                Result = reader.ReadByte();
                var tlvs = Tlv.ParseTlv(reader.ReadBytes((int) (reader.BaseStream.Length - 1)));
                //重置指针（因为tlv解包后指针已经移动到末尾）
                reader.BaseStream.Position = 1;
                TlvExecutionProcessing(tlvs);
            }
        }
    }
}