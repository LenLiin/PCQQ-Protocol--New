using System;
using System.Text;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.Receive.Data
{
    public class Receive_0x001D : ReceivePacket
    {
        /// <summary>
        ///     改变在线状态
        /// </summary>
        public Receive_0x001D(byte[] byteBuffer, QQUser User)
            : base(byteBuffer, User, User.TXProtocol.SessionKey)
        {
        }

        protected override void ParseBody()
        {
            Decrypt(user.TXProtocol.SessionKey);
            reader.ReadBytes(4);
            user.QQ_Skey = Encoding.UTF8.GetString(reader.ReadBytes(10));
            if (string.IsNullOrEmpty(user.QQ_Skey))
            {
                throw new Exception("skey获取失败");
            }

            user.QQ_Cookies = "uin=o" + user.QQ + ";skey=" + user.QQ_Skey + ";";
            user.QQ_Gtk = Util.GET_GTK(user.QQ_Skey);
        }
    }
}