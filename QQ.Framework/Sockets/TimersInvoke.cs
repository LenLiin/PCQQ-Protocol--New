using System.Timers;
using QQ.Framework.Packets.Send.Message;

namespace QQ.Framework.Sockets
{
    /// <summary>
    ///     定时发送心跳包
    /// </summary>
    public class TimersInvoke
    {
        private readonly QQClient _client;
        private readonly Timer timer = new Timer();

        /// <summary>
        ///     定时发送心跳包
        /// </summary>
        /// <param name="client"></param>
        public TimersInvoke(QQClient client)
        {
            _client = client;
        }

        /// <summary>
        ///     定时发送心跳包
        /// </summary>
        public void StartTimer()
        {
            timer.Elapsed += InvokeFailMsg;
            timer.Enabled = true; //是否触发Elapsed事件
            timer.AutoReset = true; //每到指定时间Elapsed事件是触发一次（false），还是一直触发（true）
            timer.Interval = 20000; // 设置时间间隔为20秒
        }

        /// <summary>
        ///     定时发送心跳包
        /// </summary>
        public void InvokeFailMsg(object sender, ElapsedEventArgs e)
        {
            _client.Send(new Send_0x0058(_client.QQUser).WriteData());
        }
    }
}