using System;
using System.Text;
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
            var user = new QQUser(0, "");
            var socketServer = new SocketServiceImpl(user);
            var transponder = new Transponder();
            var sendService = new SendMessageServiceImpl(socketServer, user);

            var manage = new MessageManage(socketServer, user, transponder);

            var robot = new TestRobot(sendService, transponder, user);

            manage.Init();
            Console.ReadKey();
        }
    }
}