using System;
using System.Text;
using System.Threading.Tasks;

namespace QQ.Framework.Packets.Receive.Data
{
    public class Receive_0X001D : ReceivePacket
    {
        /// <summary>
        ///     改变在线状态
        /// </summary>
        public Receive_0X001D(byte[] byteBuffer, QQUser user)
            : base(byteBuffer, user, user.TXProtocol.SessionKey)
        {
        }

        protected override void ParseBody()
        {
            Decrypt(User.TXProtocol.SessionKey);
            Reader.ReadBytes(4);
            User.QQSkey = Encoding.UTF8.GetString(Reader.ReadBytes(10));
            if (string.IsNullOrEmpty(User.QQSkey))
            {
                throw new Exception("skey获取失败");
            }

            new Task(() =>
            {
                User.GetQunCookies();
                User.Friends = User.Get_Friend_List();
                User.Groups = User.Get_Group_List();
            }).Start();
            //User.QQCookies = "uin=o" + User.QQ + ";skey=" + User.QQSkey + ";";
            //User.QQGtk = Util.GET_GTK(User.QQSkey);
        }
    }
}