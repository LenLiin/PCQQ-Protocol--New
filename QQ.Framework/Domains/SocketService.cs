using QQ.Framework.Packets;

namespace QQ.Framework.Domains
{
    /// <summary>
    ///     连接服务
    /// </summary>
    public interface SocketService
    {
        /// <summary>
        ///     接收来自服务器的消息
        /// </summary>
        /// <returns></returns>
        ReceiveData Receive();

        /// <summary>
        ///     发送消息
        /// </summary>
        void Send(SendPacket packet);

        /// <summary>
        ///     刷新服务器地址
        /// </summary>
        /// <param name="host"></param>
        void RefreshHost(string host);
    }
}