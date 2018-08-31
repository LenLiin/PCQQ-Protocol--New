using QQ.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQ.Framework.Packets.Send.Message
{
    public class Send_0x0319 : SendPacket
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="byteBuffer"></param>
        /// <param name="User"></param>
        public Send_0x0319(QQUser User, byte[] Data)
            : base(User)
        {
            Sequence = GetNextSeq();
            _secretKey = user.QQ_SessionKey;
            Command = QQCommand.Message0x0319;
            _Data = Data;
        }

        byte[] _Data;

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
            /*
            bodyWriter.Write(new byte[] { 0x00, 0x00, 0x00, 0x07, 0x00, 0x00 });

            //数据长度
            bodyWriter.Write(new byte[] { 0x00, 0x10 });


            bodyWriter.Write(new byte[] { 0x08, 0x01, 0x12, 0x03, 0x98, 0x01, 0x00 });

            //数据
            bodyWriter.Write(new byte[] { 0x0A, 0x0E, 0x08, 0xF3, 0xF2, 0xF0, 0xC6, 0x01, 0x10, 0x95, 0xA7, 0xFF, 0xDB, 0x05, 0x20, 0x00 });*/


            bodyWriter.Write(new byte[] {0x00, 0x00, 0x00, 0x07, 0x00, 0x00});
            //数据长度
            bodyWriter.BEWrite((ushort) _Data.Length);
            bodyWriter.Write(new byte[] {0x08, 0x01, 0x12, 0x03, 0x98, 0x01, 0x00, 0x0A, 0x0E, 0x08});
            //数据
            bodyWriter.Write(_Data);
            bodyWriter.Write(new byte[] {0xA7, 0xFF, 0xDB, 0x05, 0x20, 0x00});
        }
    }
}