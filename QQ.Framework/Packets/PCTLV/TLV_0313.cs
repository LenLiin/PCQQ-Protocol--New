using System;
using System.Collections.Generic;
using System.IO;
using QQ.Framework;
using QQ.Framework.Utils;

namespace Struggle.Framework.PCQQ.PCLogin.PCPacket.PCTLV
{
    internal class TLV_0313 : BaseTLV
    {
        public TLV_0313()
        {
            cmd = 0x0313;
            Name = "SSO2::TLV_GUID_Ex_0x313";
            wSubVer = 0x01;
        }

        public byte[] get_tlv_0313(QQClient m_PCClient)
        {
            var data = new BinaryWriter(new MemoryStream());
            if (wSubVer == 0x01)
            {
                data.Write(1); // wSubVer
                var GuidName = new Dictionary<string, byte[]>(6);
                GuidName.Add("UNKNOW1", null);
                GuidName.Add("MacGuid", m_PCClient.QQUser.TXProtocol.bufMacGuid);
                GuidName.Add("UNKNOW2", null);
                GuidName.Add("ComputerIDEx", m_PCClient.QQUser.TXProtocol.bufComputerIDEx);
                GuidName.Add("UNKNOW3", null);
                GuidName.Add("MachineInfoGuid", m_PCClient.QQUser.TXProtocol.bufMachineInfoGuid);
                byte k = 0;
                byte c = 0;
                var guid = new BinaryWriter(new MemoryStream());
                foreach (var kv in GuidName)
                {
                    k++;
                    if (kv.Value != null)
                    {
                        c++;
                        guid.Write(k);
                        guid.Write(kv.Value);
                        guid.BEWrite(0);
                    }
                }

                data.Write(c);
                data.Write(guid.BaseStream.ToBytesArray());
            }
            else
            {
                throw new Exception(string.Format("{0} 无法识别的版本号 {1}", Name, wSubVer));
            }

            fill_head(cmd);
            fill_body(data.BaseStream.ToBytesArray(), data.BaseStream.Length);
            set_length();
            return get_buf();
        }
    }
}