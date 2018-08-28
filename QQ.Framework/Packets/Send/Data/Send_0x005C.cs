using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQ.Framework.Packets.Send.Data
{
    public class Send_0x005C : SendPacket
    {
        public Send_0x005C(QQUser User)
            : base(User)
        {
            Sequence = GetNextSeq();
            _secretKey = user.QQ_SessionKey;
            Command = QQCommand.Data0x005C;
        }
        protected override void PutHeader(ByteBuffer buf)
        {
            base.PutHeader(buf);
            Sequence = GetNextSeq();
            buf.Put(user.QQ_PACKET_FIXVER);
        }
        /// <summary>
        /// 初始化包体
        /// </summary>
        /// <param name="buf">The buf.</param>
        protected override void PutBody(ByteBuffer buf)
        {
            buf.Put(0x88);
            buf.PutLong(user.QQ);
            buf.Put(0x00);
        }

    }
}
