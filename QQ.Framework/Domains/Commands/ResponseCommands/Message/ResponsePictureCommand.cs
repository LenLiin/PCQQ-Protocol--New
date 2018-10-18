using QQ.Framework.Events;
using QQ.Framework.Packets.Receive.Message;
using QQ.Framework.Packets.Send.Message;
using QQ.Framework.Utils;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace QQ.Framework.Domains.Commands.ResponseCommands.Message
{
    [ResponsePacketCommand(QQCommand.Message0X0388)]
    public class ResponsePictureCommand : ResponseCommand<Receive_0X0388>
    {
        public ResponsePictureCommand(QQEventArgs<Receive_0X0388> args) : base(args)
        {
        }

        public override void Process()
        {
            if (_user.GroupSendMessages.Where(c => c.Sequence == _packet.Sequence && c.Command == QQCommand.Message0X0002).Any())
            {
                var send0X0002 = _user.GroupSendMessages.Where(c => c.Sequence == _packet.Sequence && c.Command == QQCommand.Message0X0002).FirstOrDefault();
                foreach(var textSnippet in send0X0002.Message.Snippets)
                {
                    var Md5 = textSnippet.Get<string>("Md5");
                    if (Md5 != null)
                    {
                        if (_packet.GetPacketLength() == 239)//未发送过的图片
                        {
                            HttpUpLoadGroupImg(send0X0002._group, _packet.Ukey, textSnippet.Content);
                        }
                        var _30Key = textSnippet.Get<string>("30Key");
                        if (string.IsNullOrEmpty(_30Key))
                        {
                            textSnippet.Set("30Key", _packet._30Key);
                        }
                        var _48Key = textSnippet.Get<string>("48Key");
                        if (string.IsNullOrEmpty(_48Key))
                        {
                            textSnippet.Set("48Key", _packet._48Key);
                        }
                    }
                }
                _service.Send(send0X0002);
            }
        }

        /// <summary>
        ///     上传图片
        /// </summary>
        /// <param name="groupNum"></param>
        /// <param name="ukey"></param>
        /// <param name="fileName"></param>
        public void HttpUpLoadGroupImg(long groupNum, string ukey, string fileName)
        {
            using (var webclient = new HttpWebClient())
            {
                var file = new Bitmap(fileName);
                var picBytes = ImageHelper.ImageToBytes(file);

                var apiUrl =
                    $"http://htdata2.qq.com/cgi-bin/httpconn?htcmd=0x6ff0071&ver={_user.TXProtocol.DwClientVer}&term=pc&ukey={ukey}&filesize={picBytes.Length}&range=0&uin{_user.QQ}&&groupcode={groupNum}";
                webclient.Headers["User-Agent"] = "QQClient";
                webclient.Headers.Add("Content-Type", "text/octet");
                var result = webclient.UploadData(apiUrl, "POST",
                    picBytes);

                file.Dispose();
            }
        }
    }
}