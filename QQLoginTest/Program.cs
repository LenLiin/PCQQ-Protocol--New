using System;
#if NETCOREAPP2_0||NETCOREAPP2_1
using System.Text;
#endif
using QQ.Framework;
using QQ.Framework.Domains;
using QQ.Framework.Sockets;
using QQLoginTest.Robots;

namespace QQLoginTest
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            #if NETCOREAPP2_0 || NETCOREAPP2_1
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            #endif
            var user = new QQUser(0, "");
            var socketServer = new SocketServiceImpl(user);
            var transponder = new Transponder();
            var sendService = new SendMessageServiceImpl(socketServer, user);

            var manage = new MessageManage(socketServer, user, transponder);

            var robot = new TestRoBot(sendService, transponder, user);

            manage.Init();
            Console.ReadKey();
        }
    }
}