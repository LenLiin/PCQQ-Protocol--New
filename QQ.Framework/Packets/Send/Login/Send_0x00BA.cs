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
        protected override void PutHeader(ByteBuffer buf)
        {
            base.PutHeader(buf);
            buf.Put(user.QQ_PACKET_FIXVER);
            buf.Put(_secretKey);
        }
        /// <summary>
        /// 初始化包体
        /// </summary>
        /// <param name="buf">The buf.</param>
        protected override void PutBody(ByteBuffer buf)
        {
            buf.Put(new byte[] { 0x00, 0x02, 0x00, 0x00, 0x08, 0x04, 0x01, 0xE0 });
            buf.Put(user.QQ_PACKET_0825DATA2);
            buf.Put(0x00);
            buf.PutUShort((ushort)user.QQ_0825Token.Length);
            buf.Put(user.QQ_0825Token);
            buf.Put(new byte[] { 0x01, 0x02 });
            buf.PutUShort((ushort)user.QQ_PUBLIC_KEY.Length);
            buf.Put(user.QQ_PUBLIC_KEY);
            if (string.IsNullOrEmpty(VerifyCode))
            {
                buf.Put(new byte[] { 0x13, 0x00, 0x05, 0x00, 0x00, 0x00, 0x00 });
                buf.Put(user.QQ_PACKET_00BASequence);
                buf.PutUShort((ushort)user.QQ_PACKET_00BAToken.Length);
                if (user.QQ_PACKET_00BAToken.Length == 0)
                {
                    buf.Put(0x00);
                }
                else
                {
                    buf.Put(user.QQ_PACKET_00BAToken);
                }
            }
            else
            {
                var VerifyCodeBytes = Util.HexStringToByteArray(Util.ConvertStringToHex(VerifyCode));
                buf.Put(new byte[] { 0x14, 0x00, 0x05, 0x00, 0x00, 0x00, 0x00 });
                buf.PutUShort((ushort)VerifyCodeBytes.Length);
                buf.Put(VerifyCodeBytes);
                buf.PutUShort((ushort)user.QQ_PACKET_00BAVerifyToken.Length);
                buf.Put(user.QQ_PACKET_00BAVerifyToken);
                //输入验证码后清空图片流
                user.QQ_PACKET_00BAVerifyCode = new byte[] { };
            }
            buf.PutUShort((ushort)user.QQ_PACKET_00BA_FixKey.Length);
            buf.Put(user.QQ_PACKET_00BA_FixKey);
        }

        protected override void PutTail(ByteBuffer buf)
        {
            base.PutTail(buf);
            user.QQ_PACKET_00BASequence++;
        }
    }
}
