using QQ.Framework.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQ.Framework.Packets.Receive.Login
{
    public class Receive_0x0825 : ReceivePacket
    {
        public Receive_0x0825(byte[] byteBuffer, QQUser User)
            : base(byteBuffer, User, User.QQ_PACKET_0825KEY)
        {
        }

        public byte DataHead { get; set; }

        protected override void ParseBody()
        {
            Decrypt(!user.IsLoginRedirect ? user.QQ_PACKET_0825KEY : user.QQ_PACKET_REDIRECTIONKEY);

            DataHead = reader.ReadByte();
            reader.BEReadChar(); //0112
            reader.BEReadChar(); //0038
            user.QQ_0825Token = reader.ReadBytes(0x38);
            if (DataHead == 0xFE)
            {
                reader.ReadBytes(6);
                user.LoginTime = reader.ReadBytes(4);
                reader.ReadBytes(2);
                reader.ReadBytes(4);
                reader.ReadBytes(18);
                user.ServerIp = reader.ReadBytes(4);
                reader.ReadBytes(6);
            }
            else
            {
                reader.ReadBytes(6);
                user.LoginTime = reader.ReadBytes(4);
                reader.ReadBytes(2);
                reader.ReadBytes(4);
                reader.ReadBytes(6);
                user.ServerIp = reader.ReadBytes(4);
            }

            //从原始数据包提取加密包
            reader.ReadBytes(buffer.Length - 1);
        }
    }
}