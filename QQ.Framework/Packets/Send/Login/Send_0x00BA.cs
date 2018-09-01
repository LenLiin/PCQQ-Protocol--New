using QQ.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQ.Framework.Packets.Send.Login
{
    /// <summary>
    /// 验证码
    /// </summary>
    public class Send_0x00BA : SendPacket
    {
        string VerifyCode { get; set; }

        /// <summary>
        /// 验证码提交
        /// </summary>
        public Send_0x00BA(QQUser User, string VerifyCode)
            : base(User)
        {
            Sequence = GetNextSeq();
            _secretKey = user.QQ_PACKET_00BA_Key;
            Command = QQCommand.Login0x00BA;
            this.VerifyCode = VerifyCode;
        }

        protected override void PutHeader()
        {
            base.PutHeader();
            writer.Write(user.QQ_PACKET_FIXVER);
            writer.Write(_secretKey);
        }

        /// <summary>
        /// 初始化包体
        /// </summary>
        /// <param name="buf">The buf.</param>
        protected override void PutBody()
        {
            bodyWriter.Write(new byte[] {0x00, 0x02, 0x00, 0x00, 0x08, 0x04, 0x01, 0xE0});
            bodyWriter.Write(user.QQ_PACKET_0825DATA2);
            bodyWriter.Write((byte) 0x00);
            bodyWriter.BEWrite((ushort) user.QQ_0825Token.Length);
            bodyWriter.Write(user.QQ_0825Token);
            bodyWriter.Write(new byte[] {0x01, 0x02});
            bodyWriter.BEWrite((ushort) user.QQ_PUBLIC_KEY.Length);
            bodyWriter.Write(user.QQ_PUBLIC_KEY);
            if (string.IsNullOrEmpty(VerifyCode))
            {
                bodyWriter.Write(new byte[] {0x13, 0x00, 0x05, 0x00, 0x00, 0x00, 0x00});
                bodyWriter.Write(user.QQ_PACKET_00BASequence);
                bodyWriter.BEWrite((ushort) user.QQ_PACKET_00BAToken.Length);
                if (user.QQ_PACKET_00BAToken.Length == 0)
                {
                    bodyWriter.Write((byte) 0x00);
                }
                else
                {
                    bodyWriter.Write(user.QQ_PACKET_00BAToken);
                }
            }
            else
            {
                var VerifyCodeBytes = Util.HexStringToByteArray(Util.ConvertStringToHex(VerifyCode));
                bodyWriter.Write(new byte[] {0x14, 0x00, 0x05, 0x00, 0x00, 0x00, 0x00});
                bodyWriter.BEWrite((ushort) VerifyCodeBytes.Length);
                bodyWriter.Write(VerifyCodeBytes);
                bodyWriter.BEWrite((ushort) user.QQ_PACKET_00BAVerifyToken.Length);
                bodyWriter.Write(user.QQ_PACKET_00BAVerifyToken);
                //输入验证码后清空图片流
                user.QQ_PACKET_00BAVerifyCode = new byte[] { };
            }

            bodyWriter.BEWrite((ushort) user.QQ_PACKET_00BA_FixKey.Length);
            bodyWriter.Write(user.QQ_PACKET_00BA_FixKey);
        }

        protected override void PutTail()
        {
            base.PutTail();
            user.QQ_PACKET_00BASequence++;
        }
    }
}