using System.IO;
using System.Linq;
using QQ.Framework.Packets.PCTLV;
using QQ.Framework.TlvLib;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.Receive.Login
{
    public class Receive_0X0836 : ReceivePacket
    {
        public Receive_0X0836(byte[] byteBuffer, QQUser user)
            : base(byteBuffer, user, user.TXProtocol.BufTgtgtKey)
        {
        }

        public string ErrorMsg { get; set; }

        /// <summary>
        ///     状态码
        /// </summary>
        public byte Result { get; set; }

        protected override void ParseBody()
        {
            Decrypt(User.TXProtocol.BufDhShareKey);
            Result = Reader.ReadByte();
            //返回错误
            if (Result == (byte) ResultCode.DoMain || Result == (byte) ResultCode.其它错误
                                                   || Result == (byte) ResultCode.密码错误 ||
                                                   Result == (byte) ResultCode.帐号被回收
                                                   || Result == (byte) ResultCode.要求切换TCP ||
                                                   Result == (byte) ResultCode.过载保护
                                                   || Result == (byte) ResultCode.需要验证密保 ||
                                                   Result == (byte) ResultCode.需要验证码)
            {
                var tlvs = Tlv.ParseTlv(Reader.ReadBytes((int) (Reader.BaseStream.Length - 1)));
                //重置指针（因为tlv解包后指针已经移动到末尾）
                Reader.BaseStream.Position = 1;
                TlvExecutionProcessing(tlvs);
                if (tlvs.Any(c => c.Tag == 0x0100))
                {
                    var errorData = tlvs.FirstOrDefault(c => c.Tag == 0x0100);
                    var errReader = new BinaryReader(new MemoryStream(errorData.Value));
                    var tlv = new TLV0100();
                    tlv.Parser_Tlv2(User, errReader, errorData.Length);
                    ErrorMsg = tlv.ErrorMsg;
                }
            }
            else
            {
                BodyDecrypted = QQTea.Decrypt(BodyDecrypted, User.TXProtocol.BufTgtgtKey);
                Reader = new BinaryReader(new MemoryStream(BodyDecrypted));
                Result = Reader.ReadByte();
                var tlvs = Tlv.ParseTlv(Reader.ReadBytes((int) (Reader.BaseStream.Length - 1)));
                //重置指针（因为tlv解包后指针已经移动到末尾）
                Reader.BaseStream.Position = 1;
                TlvExecutionProcessing(tlvs);
            }
        }
    }
}