namespace QQ.Framework.Domains
{
    public interface PacketCommand
    {
        /// <summary>
        /// 接收包的处理逻辑在此处完成
        /// </summary>
        void Receive();
    }
}