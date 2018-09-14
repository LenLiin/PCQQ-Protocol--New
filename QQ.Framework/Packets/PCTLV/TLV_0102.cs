using System;
using QQ.Framework;

namespace QQ.Framework.Packets.PCTLV
{
    internal class TLV_0102 : BaseTLV
    {
        public TLV_0102()
        {
            cmd = 0x0102;
            Name = "SSO2::TLV_Official_0x102";
            wSubVer = 0x0001;
        }

        public byte[] get_tlv_0102(QQClient m_PCClient)
        {
            //TODO:Official
            throw new Exception("Official 获取失败！");
            //  var data = new BinaryWriter(new MemoryStream());
            //if (this.wSubVer == 0x0001)
            //{
            //    data.BEWrite(this.wSubVer);
            //    var bufKey = Util.RandomKey();
            //    data.Write(bufKey);
            //    var bufSigPic = m_PCClient.QQUser.TXProtocol.bufSigPic;
            //    if (bufSigPic == null)
            //    {
            //        bufSigPic = Util.RandomKey(56);
            //    }
            //    data.BEWrite((ushort)bufSigPic.Length);
            //    data.Write(bufSigPic);
            //    data.BEWrite(0x0014);
            //    byte[] Data = null;
            //    if (m_PCClient.m_LisHelper.GetOfficial(bufKey, bufSigPic, m_PCClient.QQUser.TXProtocol.bufTGTGT,out Data))
            //    {
            //        data.Write(Data);
            //        data.BEWrite(CRC32cs.CRC32(Data));
            //    }
            //    else
            //    {
            //        throw new Exception("Official 获取失败！");
            //    }
            //}
            //else
            //{
            //    throw new Exception(string.Format("{0} 无法识别的版本号 {1}", this.Name, this.wSubVer));
            //}
            //fill_head(this.cmd);
            //fill_body(data.BaseStream.ToBytesArray(), data.BaseStream.Length);
            //set_length();
            //return get_buf();
        }
    }
}