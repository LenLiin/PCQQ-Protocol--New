using QQ.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQ.Framework.Packets.Send.Message
{
    /// <summary>
    /// 点赞
    /// </summary>
    public class Send_0x03E3 : SendPacket
    {
        /// <summary>
        /// 点赞
        /// </summary>
        /// <param name="User"></param>
        /// <param name="ToQQ">要点赞的QQ</param>
        public Send_0x03E3(QQUser User, long ToQQ)
            : base(User)
        {
            Sequence = GetNextSeq();
            _secretKey = user.QQ_SessionKey;
            Command = QQCommand.Interactive0x03E3;
            _toQQ = ToQQ;
        }
        /// <summary>
        /// 好友QQ
        /// </summary>
        long _toQQ;
        protected override void PutHeader(ByteBuffer buf)
        {
            base.PutHeader(buf);
            buf.Put(new byte[] { 0x04, 0x00, 0x00, 0x00, 0x01, 0x01, 0x01, 0x00, 0x00, 0x68, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
        }
        protected override void PutBody(ByteBuffer buf)
        {
            var myid = Util.HexStringToByteArray(Util.PB_toLength(user.QQ));
            var id = Util.HexStringToByteArray(Util.PB_toLength(_toQQ));
            buf.Put(new byte[] { 0x00, 0x00, 0x00 });
            buf.PutLong(myid.Length + 11);
            buf.Put(new byte[] { 0x00, 0x00, 0x00 });
            buf.PutLong(myid.Length + id.Length + 8);
            buf.Put(new byte[] { 0x08, 0x01 });
            buf.Put(new byte[] { 0x12 });
            buf.PutLong(myid.Length + 7);
            buf.Put(new byte[] { 0x08 });
            buf.Put(myid);
            buf.Put(new byte[] { 0x10, 0xE3, 0x07, 0x98, 0x01, 0x00 });
            buf.Put(new byte[] { 0x08, 0xE5, 0x0F });
            buf.Put(new byte[] { 0x10, 0x01 });
            buf.Put(new byte[] { 0x22 });
            buf.PutLong(id.Length + 6);
            buf.Put(new byte[] { 0x58 });
            buf.Put(id);
            buf.Put(new byte[] { 0x60, 0x92, 0x4E });
            buf.Put(new byte[] { 0x68, 0x01 });
        }
    }
}
