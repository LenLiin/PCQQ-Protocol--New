using System;
using System.IO;
#if NETCOREAPP2_0||NETCOREAPP2_1
using System.Text;
#endif
using QQ.Framework;
using QQ.Framework.Domains;
using QQ.Framework.Packets.Receive.Login;
using QQ.Framework.Packets.Receive.Message;
using QQ.Framework.Packets.Send.Login;
using QQ.Framework.Sockets;
using QQ.Framework.Utils;
#if NETCOREAPP2_0 || NETCOREAPP2_1
#else 
using QQ.FrameworkTest.Robots;
#endif

namespace QQLoginTest
{
    public partial class Program
    {
        private static void Main(string[] args)
        {
            #if NETCOREAPP2_0 || NETCOREAPP2_1
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            #endif
            var user = new QQUser(MyQQ, MyPassWord);
            var socketServer = new SocketServiceImpl(user);
            var transponder = new Transponder();
            var sendService = new SendMessageServiceImpl(socketServer);

            var manage = new MessageManage(socketServer, user, transponder);

            #if NETCOREAPP2_0 || NETCOREAPP2_1
            
            #else

            var robot = new TestRoBot(sendService, transponder, user);

            #endif

            manage.Init();
            Console.ReadKey();
        }
    }
}