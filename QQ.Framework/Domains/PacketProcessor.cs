namespace QQ.Framework.Domains
{
    public interface PacketProcessor<T>
    {
        /// <summary>
        /// 处理的具体逻辑
        /// </summary>
        /// <returns></returns>
        T Process();
    }
}