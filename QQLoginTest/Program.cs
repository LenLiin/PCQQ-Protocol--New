using System;
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
            if (args.Length != 2)
            {
                throw new Exception("参数数量错误：账号 密码。");
            }
            var user = new QQUser(long.Parse(args[0]), args[1]); // 虽然已经太迟了，但是还是把密码去掉了... 起码总比在这里好啊！（塞口球
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