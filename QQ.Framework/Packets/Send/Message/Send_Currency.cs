using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQ.Framework.Packets.Send.Message
{
    public class Send_Currency : SendPacket
    {
        /// <summary>
        /// 通用响应包
        /// </summary>
        /// <param name="byteBuffer"></param>
        /// <param name="User"></param>
        /// <param name="Data">要发送的数据内容</param>
        /// <param name="_sequence">序号</param>
        public Send_Currency(QQUser User, byte[] Data, char _sequence, char _Command)
            : base(User)
        {
            Sequence = _sequence;
            _secretKey = user.QQ_SessionKey;
            Command = (QQCommand) _Command;
            _data = Data;
        }

        private byte[] _data { get; set; }

        protected override void PutHeader()
        {
            base.PutHeader();
            writer.Write(user.QQ_PACKET_FIXVER);
        }

        /// <summary>
        /// 初始化包体
        /// </summary>
        /// <param name="buf">The buf.</param>
        protected override void PutBody()
        {
            bodyWriter.Write(_data);
        }
    }
}