namespace QQ.Framework.Domains
{
    public interface PacketCommand
    {
        /// <summary>
        /// 包的处理逻辑在此处完成
        /// </summary>
        void Process();
    }
}