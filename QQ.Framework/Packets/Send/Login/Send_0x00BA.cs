using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQ.Framework.Packets.Send.Login
{
    /// <summary>
    /// 验证码
    /// </summary>
    public class Send_0x00BA : SendPacket
    {

        /// <summary>
        /// 初始化包体
        /// </summary>
        /// <param name="buf">The buf.</param>
        protected override void PutBody(ByteBuffer buf)
        {
            throw new NotImplementedException();
        }
    }
}
