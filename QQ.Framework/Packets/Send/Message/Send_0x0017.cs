using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQ.Framework.Packets.Send.Message
{
    public class Send_0x0017 : SendPacket
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="byteBuffer"></param>
        /// <param name="User"></param>
        /// <param name="Data">要发送的数据内容</param>
        /// <param name="_sequence">序号</param>
        public Send_0x0017(QQUser User,byte[] Data,char _sequence)
            : base(User)
        {
            Sequence = _sequence;
            _secretKey = user.QQ_SessionKey;
            Command = QQCommand.Message0x0017;
            _data = Data;
        }
        private byte[] _data { get; set; }
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
            buf.Put(_data);
        }

    }
}
