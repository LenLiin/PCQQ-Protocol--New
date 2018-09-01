using QQ.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQ.Framework.Packets.Receive.Message
{
    /// <summary>
    /// 点赞回复包
    /// </summary>
    public class Receive_0x03E3 : ReceivePacket
    {
        /// <summary>
        /// 点赞回复包
        /// </summary>
        public Receive_0x03E3(byte[] byteBuffer, QQUser User)
            : base(byteBuffer, User, User.QQ_SessionKey)
        {
        }

        protected override void ParseBody()
        {
            Decrypt(user.QQ_SessionKey);
        }
    }
}