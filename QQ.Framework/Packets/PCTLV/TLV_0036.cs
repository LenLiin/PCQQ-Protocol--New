using System;
using System.IO;
using QQ.Framework.Utils;

namespace QQ.Framework.Packets.PCTLV
{
    [TlvTag(TlvTags.LoginReason)]
    internal class TLV0036 : BaseTLV
    {
        public TLV0036()
        {
            Command = 0x0036;
            Name = "SSO2::TLV_LoginReason_0x36";
            WSubVer = 0x0002;
        }

        /// <summary>
        ///     LoginReason
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public byte[] Get_Tlv(QQUser user)
        {
            var data = new BinaryWriter(new MemoryStream());
            if (WSubVer == 0x0002)
            {
                data.BeWrite(WSubVer); //wSubVer
                data.BeUshortWrite(1);
                data.BeUshortWrite(0);
                data.BeUshortWrite(1);
                data.BeUshortWrite(0);
                data.BeUshortWrite(0);
                data.BeUshortWrite(0);
                data.BeUshortWrite(0);
                data.BeUshortWrite(0);
            }
            else if (WSubVer == 0x0001)
            {
                data.BeWrite(WSubVer); //wSubVer
                data.BeUshortWrite(1);
                data.BeUshortWrite(0);
                data.BeUshortWrite(0);
            }
            else
            {
                throw new Exception($"{Name} 无法识别的版本号 {WSubVer}");
            }

            FillHead(Command);
            FillBody(data.BaseStream.ToBytesArray(), data.BaseStream.Length);
            SetLength();
            return GetBuffer();
        }
    }
}