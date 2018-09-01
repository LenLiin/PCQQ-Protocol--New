using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQ.Framework.Packets.Send.Message
{
    public class Send_0x01C0 : SendPacket
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="byteBuffer"></param>
        /// <param name="User"></param>
        /// <param name="ToQQ"></param>
        public Send_0x01C0(QQUser User, byte[] ToQQ)
            : base(User)
        {
            Sequence = GetNextSeq();
            _secretKey = user.QQ_SessionKey;
            Command = QQCommand.Message0x01C0;
            _toQQ = ToQQ;
        }

        protected override void PutHeader()
        {
            base.PutHeader();
            writer.Write(user.QQ_PACKET_FIXVER);
        }

        /// <summary>
        /// 好友QQ
        /// </summary>
        byte[] _toQQ;

        /// <summary>
        /// 初始化包体
        /// </summary>
        /// <param name="buf">The buf.</param>
        protected override void PutBody()
        {
            bodyWriter.Write(0x01);
            bodyWriter.Write(_toQQ);
        }
    }
}