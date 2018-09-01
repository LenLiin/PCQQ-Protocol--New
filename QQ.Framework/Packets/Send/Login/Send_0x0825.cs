using QQ.Framework.Utils;

namespace QQ.Framework.Packets.Send.Login
{
    public class Send_0x0825 : SendPacket
    {
        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="byteBuffer"></param>
        /// <param name="User"></param>
        /// <param name="Key">数据包密钥</param>
        /// <param name="Redirect">是否是重定向包</param>
        public Send_0x0825(QQUser User, bool Redirect)
            : base(User)
        {
            if (Redirect)
            {
                Sequence = (char) 0x3102;
            }
            else
            {
                Sequence = (char) 0x3101;
            }

            redirect = Redirect;
            if (!Redirect)
            {
                _secretKey = user.QQ_PACKET_0825KEY;
            }
            else
            {
                _secretKey = user.QQ_PACKET_REDIRECTIONKEY;
            }

            Command = QQCommand.Login0x0825;
        }

        /// <summary>
        ///     重定向标识
        /// </summary>
        private bool redirect { get; }

        public override string GetPacketName()
        {
            return "登录包0x0825（Ping）";
        }

        protected override void PutHeader()
        {
            base.PutHeader();
            writer.Write(user.QQ_PACKET_FIXVER);
            writer.Write(_secretKey);
        }

        /// <summary>
        ///     初始化包体
        /// </summary>
        /// <param name="buf">The buf.</param>
        protected override void PutBody()
        {
            bodyWriter.Write(user.QQ_PACKET_0825DATA0);
            bodyWriter.Write(user.QQ_PACKET_0825DATA2);
            bodyWriter.BEWrite(user.QQ);
            if (!redirect)
            {
                bodyWriter.Write(new byte[] {0x00, 0x00, 0x00, 0x00, 0x03, 0x09, 0x00, 0x08, 0x00, 0x01});
                bodyWriter.Write(user.ServerIp);
                bodyWriter.Write(new byte[]
                {
                    0x00, 0x02, 0x00, 0x36, 0x00, 0x12, 0x00, 0x02, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x14, 0x00, 0x1D, 0x01, 0x02, 0x00, 0x19
                });
            }
            else
            {
                bodyWriter.Write(new byte[] {0x00, 0x01, 0x00, 0x00, 0x03, 0x09, 0x00, 0x0C, 0x00, 0x01});
                bodyWriter.Write(user.ServerIp);
                bodyWriter.Write(new byte[]
                {
                    0x01, 0x6F, 0xA1, 0x58, 0x22, 0x01, 0x00, 0x36, 0x00, 0x12, 0x00, 0x02, 0x00, 0x01, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x14, 0x00, 0x1D,
                    0x01, 0x03, 0x00, 0x19
                });
            }

            bodyWriter.Write(user.QQ_PUBLIC_KEY);
        }
    }
}