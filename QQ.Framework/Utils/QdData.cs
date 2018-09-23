using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace QQ.Framework.Utils
{
    public static class QdData
    {
        public static byte[] GetQdData(QQUser User)
        {
            byte[] result = null;
            try
            {
                var data = new BinaryWriter(new MemoryStream());
                data.Write(User.TXProtocol.dwServerIP);

                var qddata = new BinaryWriter(new MemoryStream());
                qddata.Write(User.TXProtocol.dwQdVerion);
                qddata.Write(User.TXProtocol.dwPubNo);
                qddata.BEWrite(User.QQ);
                qddata.BEWrite((ushort)data.BaseStream.Length);

                data = new BinaryWriter(new MemoryStream());
                data.Write(User.TXProtocol.QdPreFix);
                data.BEWrite(User.TXProtocol.cQdProtocolVer);
                data.BEWrite(User.TXProtocol.dwQdVerion);
                data.Write((byte)0);
                data.BEWrite(User.TXProtocol.wQdCsCmdNo);
                data.Write(User.TXProtocol.cQdCcSubNo);
                data.Write(new byte[] { 0x0E, 0x88 });//xrand(0xFFFF) + 1
                data.BEWrite(0);//四个0
                data.Write(User.TXProtocol.bufComputerIDEx);
                data.Write(User.TXProtocol.cOsType);
                data.Write(User.TXProtocol.bIsWOW64);
                data.Write(User.TXProtocol.dwPubNo);
                data.BEWrite((ushort)User.TXProtocol.dwClientVer);
                data.BEWrite(User.TXProtocol.dwDrvVersionInfo / 0x10000);
                data.BEWrite(User.TXProtocol.dwDrvVersionInfo % 0x10000);
                data.Write(User.TXProtocol.bufVersion_TSSafeEdit_dat);
                data.Write(User.TXProtocol.bufVersion_QScanEngine_dll);
                data.Write((byte)0);

                data.Write(new TeaCrypter().Encrypt(qddata.BaseStream.ToBytesArray(), User.TXProtocol.bufQdKey));

                data.Write(User.TXProtocol.QdSufFix);

                var size = data.BaseStream.Length + 3;
                qddata = new BinaryWriter(new MemoryStream());
                qddata.Write(User.TXProtocol.QdPreFix);
                qddata.Write(size);
                qddata.Write(data.BaseStream.Length);

                result = data.BaseStream.ToBytesArray();
                User.TXProtocol.QdData = result;
                return result;
            }
            catch
            {
                return new byte[] { };
            }
        }
    }
}
