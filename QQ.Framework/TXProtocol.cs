using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQ.Framework
{
    public class TXProtocol
    {
        public byte[] bufTGTGTKey { get; set; }
        public byte[] bufTGTGT { get; set; }
        public byte[] bufTGT { get; set; }
        public byte[] bufComputerIDEx { get; set; }
        public byte[] bufComputerID { get; set; }
        public byte[] bufDeviceID { get;  set; }
        public byte[] bufSigPic { get;  set; }
        public byte[] PngToken { get;  set; }
        public byte[] PngKey { get;  set; }
        public byte[] bufTGT_GTKey { get;  set; }
        public byte[] buf16bytesGTKey_ST { get;  set; }
        public byte[] bufServiceTicket { get;  set; }
        public byte[] buf16bytesGTKey_STHttp { get;  set; }
        public byte[] bufServiceTicketHttp { get;  set; }
        public byte[] bufGTKey_TGTPwd { get;  set; }
        public byte[] bufSessionKey { get;  set; }
        public byte[] bufSigSession { get;  set; }
        public byte[] bufPwdForConn { get;  set; }
        public byte[] SessionKey { get;  set; }
        public byte[] ClientKey { get;  set; }
        public byte[] bufSigClientAddr { get;  set; }
        public byte[] bufDHPublicKey { get;  set; }
        public byte[] bufMachineInfoGuid { get;  set; }
        public byte[] bufMacGuid { get;  set; }
    }
}
