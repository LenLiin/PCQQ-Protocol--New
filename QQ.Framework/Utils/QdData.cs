using System.IO;

namespace QQ.Framework.Utils
{
    public static class QdData
    {
        public static byte[] GetQdData(QQUser user)
        {
            try
            {
                var data = new BinaryWriter(new MemoryStream());
                data.Write(user.TXProtocol.DwServerIP);

                var qddata = new BinaryWriter(new MemoryStream());
                qddata.Write(user.TXProtocol.DwQdVerion);
                qddata.Write(user.TXProtocol.DwPubNo);
                qddata.BeWrite(user.QQ);
                qddata.BeWrite((ushort) data.BaseStream.Length);

                data = new BinaryWriter(new MemoryStream());
                data.Write(user.TXProtocol.QdPreFix);
                data.BeWrite(user.TXProtocol.CQdProtocolVer);
                data.BeWrite(user.TXProtocol.DwQdVerion);
                data.Write((byte) 0);
                data.BeWrite(user.TXProtocol.WQdCsCmdNo);
                data.Write(user.TXProtocol.CQdCcSubNo);
                data.Write(new byte[] {0x0E, 0x88}); //xrand(0xFFFF) + 1
                data.BeWrite(0); //四个0
                data.Write(user.TXProtocol.BufComputerIdEx);
                data.Write(user.TXProtocol.COsType);
                data.Write(user.TXProtocol.BIsWow64);
                data.Write(user.TXProtocol.DwPubNo);
                data.BeWrite((ushort) user.TXProtocol.DwClientVer);
                data.BeWrite(user.TXProtocol.DwDrvVersionInfo / 0x10000);
                data.BeWrite(user.TXProtocol.DwDrvVersionInfo % 0x10000);
                data.Write(user.TXProtocol.BufVersionTsSafeEditDat);
                data.Write(user.TXProtocol.BufVersionQScanEngineDll);
                data.Write((byte) 0);

                data.Write(new TeaCrypter().Encrypt(qddata.BaseStream.ToBytesArray(), user.TXProtocol.BufQdKey));

                data.Write(user.TXProtocol.QdSufFix);

                var size = data.BaseStream.Length + 3;
                qddata = new BinaryWriter(new MemoryStream());
                qddata.Write(user.TXProtocol.QdPreFix);
                qddata.Write(size);
                qddata.Write(data.BaseStream.Length);

                var result = data.BaseStream.ToBytesArray();
                user.TXProtocol.QdData = result;
                return result;
            }
            catch
            {
                return new byte[] { };
            }
        }
    }
}