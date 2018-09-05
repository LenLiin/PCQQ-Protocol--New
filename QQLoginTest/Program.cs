using System;
using System.IO;
#if NETCOREAPP2_0||NETCOREAPP2_1
using System.Text;
#endif
using QQ.Framework;
using QQ.Framework.Packets.Receive.Login;
using QQ.Framework.Packets.Receive.Message;
using QQ.Framework.Packets.Send.Login;
using QQ.Framework.Utils;

namespace QQLoginTest
{
    public partial class Program
    {
        private static QQClient client;

        private static void Main(string[] args)
        {
            #if NETCOREAPP2_0||NETCOREAPP2_1
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            #endif
            var user = new QQUser(MyQQ, MyPassWord);
            client = new QQClient
            {
                QQUser = user
            };
            client.EventReceive_0x0017 += Client_EventReceive_0x0017;
            client.EventReceive_0x00CE += Client_EventReceive_0x00CE;

            client.EventReceive_0x00BA += Client_EventReceive_0x00BA;

            client.Login();

            Console.ReadKey();
        }

        private static void Client_EventReceive_0x00BA(object sender, QQEventArgs<Receive_0x00BA> e)
        {
            if (e.ReceivePacket.VerifyCommand == 0x02)
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "yanzhengma");
                var img = ImageHelper.CreateImageFromBytes(path, e.QQClient.QQUser.QQ_PACKET_00BAVerifyCode);
                var VerifyCode = Console.ReadLine();
                if (!string.IsNullOrEmpty(VerifyCode))
                {
                    e.QQClient.Send(new Send_0x00BA(e.ReceivePacket.user, VerifyCode).WriteData());
                }
            }
        }

        /// <summary>
        ///     好友消息
        /// </summary>
        private static void Client_EventReceive_0x00CE(object sender, QQEventArgs<Receive_0x00CE> e)
        {
        }

        /// <summary>
        ///     群消息
        /// </summary>
        private static void Client_EventReceive_0x0017(object sender, QQEventArgs<Receive_0x0017> e)
        {
            //if (e.ReceivePacket.FromQQ == Someone)
            //{
            //    e.QQClient.SendLongGroupMessage(e.ReceivePacket.Message, e.ReceivePacket.Group,FriendMessageType.Message);
            //}
        }
    }
}