using QQ.Framework;
using QQ.Framework.Packets.Send.Login;
using QQ.Framework.Packets.Send.Message;
using QQ.Framework.Sockets;
using QQ.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace QQLoginTest
{
    public class Program
    {
        static QQClient client;
        static void Main(string[] args)
        {
            long MyQQ = 0;
            string MyPassWord = "";
            QQUser user = new QQUser(MyQQ, MyPassWord);
            client = new QQClient()
            {
                QQUser = user
            };
            client.EventReceive_0x0017 += Client_EventReceive_0x0017;
            client.EventReceive_0x00CE += Client_EventReceive_0x00CE;

            client.Login();

            Console.ReadKey();
        }

        /// <summary>
        /// 好友消息
        /// </summary>
        private static void Client_EventReceive_0x00CE(object sender, QQEventArgs<QQ.Framework.Packets.Receive.Message.Receive_0x00CE> e)
        {
        }

        /// <summary>
        /// 群消息
        /// </summary>
        private static void Client_EventReceive_0x0017(object sender, QQEventArgs<QQ.Framework.Packets.Receive.Message.Receive_0x0017> e)
        {
        }

    }
}
