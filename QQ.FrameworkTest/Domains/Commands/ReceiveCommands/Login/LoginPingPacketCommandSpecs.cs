using System;
using Machine.Specifications;
using QQ.Framework;
using QQ.Framework.Domains;
using QQ.Framework.Domains.Commands.ReceiveCommands.Login;
using QQ.Framework.Utils;

namespace QQ.FrameworkTest.Domains.Commands.ReceiveCommands.Login
{
    public class LoginPingPacketCommandSpecs
    {
        private Establish that =
            () =>
            {
                var user = new QQUser(Int32.MaxValue, "");
                subject = DispatchPacketToCommand.of(new byte[] { }, new QQClient() { QQUser = user });
                command = QQCommand.Login0x0825;
            };

        private Because of =
            () =>
            {
                receive_command = subject.dispatch_receive_packet(command);
            };

        private It receive_command_should_a_LoginPingCommand =
            () =>
            {
                (receive_command is LoginPingCommand).ShouldBeTrue();
            };

        private static DispatchPacketToCommand subject;
        private static QQCommand command;
        private static ReceiveCommand receive_command;
    }
}
