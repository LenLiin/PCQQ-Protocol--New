using QQ.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQ.Framework.Packets.Send.Message
{
    /// <summary>
    /// 心跳
    /// </summary>
    public class Send_0x0058 : SendPacket
    {
        public Send_0x0058(QQUser User)
            : base(User)
        {
            Sequence = GetNextSeq();
            _secretKey = user.QQ_SessionKey;
            Command = QQCommand.Message0x0058;
        }

        protected override void PutHeader(ByteBuffer buf)
        {
            base.PutHeader(buf);
            buf.Put(user.QQ_PACKET_FIXVER);
        }
        /// <summary>
        /// 初始化包体
        /// </summary>
        protected override void PutBody(ByteBuffer buf)
        {
            buf.PutLong(user.QQ);
        }
    }
}
