using QQ.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQ.Framework.Packets.Send.Message
{
    public class Send_0x0360 : SendPacket
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="byteBuffer"></param>
        /// <param name="User"></param>
        public Send_0x0360(QQUser User, byte[] timeLast)
            : base(User)
        {
            Sequence = GetNextSeq();
            _secretKey = user.QQ_SessionKey;
            Command = QQCommand.Message0x0360;
            _timeLast = timeLast;
        }
        byte[] _timeLast;
        protected override void PutHeader(ByteBuffer buf)
        {
            base.PutHeader(buf);
            buf.Put(user.QQ_PACKET_FIXVER);
        }
        /// <summary>
        /// 初始化包体
        /// </summary>
        /// <param name="buf">The buf.</param>
        protected override void PutBody(ByteBuffer buf)
        {
            buf.Put(new byte[] { 0x00, 0x00, 0x00, 0x07, 0x00, 0x00 });

            //数据长度
            buf.Put(new byte[] { 0x00, 0x10 });


            buf.Put(new byte[] { 0x08, 0x01, 0x12, 0x03, 0x98, 0x01, 0x00 });

            //数据
            buf.Put(new byte[] { 0x08, 0x04, 0x2A, 0x0E, 0x08, 0xE7, 0xF6, 0xEC, 0x9C, 0x0E, 0x12, 0x06, 0x08, 0xED, 0xCB, 0x8B, 0x8D, 0x04 });
        }

    }
}
