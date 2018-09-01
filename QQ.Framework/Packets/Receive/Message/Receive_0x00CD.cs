using QQ.Framework.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQ.Framework.Packets.Receive.Message
{
    public class Receive_0x00CD : ReceivePacket
    {
        /// <summary>
        /// 发好友消息回复包
        /// </summary>
        public Receive_0x00CD(byte[] byteBuffer, QQUser User)
            : base(byteBuffer, User, User.QQ_SessionKey)
        {
        }

        protected override void ParseBody()
        {
            Decrypt(user.QQ_SessionKey);
        }
    }
}