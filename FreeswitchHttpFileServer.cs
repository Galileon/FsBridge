using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace FsConnect
{
    /// <summary>
    /// File server for freeswitch streaming purposes
    /// </summary>
    public class FreeswitchHttpFileServer
    {
    
        #region Properties

        HttpListener _httpListener;

        FreeswitchHttpServerSettings _settings;

        #endregion

        public void Start(FreeswitchHttpServerSettings settings)
        {
            this._settings = settings;
            _httpListener = new HttpListener();
            _httpListener.IgnoreWriteExceptions = true;
            _httpListener.Prefixes.Add(settings.Url);
            _httpListener.Start();
            StartListener();
        }
        
        private void StartListener()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    HttpListenerContext listenerContext = _httpListener.GetContext();
                    ProcessRequest(listenerContext);
                }
            });
        }

        private void ProcessRequest(HttpListenerContext context)
        {
            var body = new StreamReader(context.Request.InputStream).ReadToEndAsync();
            HttpListenerRequest request = context.Request;

            if (request.HttpMethod == "GET") SendFileToHttp(context, request.RawUrl);
            if (request.HttpMethod == "PUT") ReceiveRecordingFile(context, request.RawUrl);
        }

        private void ReceiveRecordingFile(HttpListenerContext context, string filePath)
        {

        }

        private void SendFileToHttp(HttpListenerContext context, string filePath)
        {
            var response = context.Response;
            // decode ..
            filePath = HttpUtility.UrlDecode(filePath);

            filePath = Path.Combine(this._settings.ReadBasePath, filePath.TrimStart('/'));

            if (!File.Exists(filePath))
            {
                response.StatusCode = (int)HttpStatusCode.NotFound;
                response.StatusDescription = "FILE NOT FOUND";
                response.OutputStream.Close();
                return;
            }

            response.StatusDescription = "OK";
            response.StatusCode = (int)HttpStatusCode.OK;
            response.SendChunked = false;

            using (FileStream fs = File.OpenRead(filePath))
            {
                string filename = Path.GetFileName(filePath);
                response.ContentLength64 = fs.Length;
                response.AddHeader("Accept-Ranges", "bytes");
                response.AddHeader("Content-Length", fs.Length.ToString());
                response.AddHeader("Content-Range", $"bytes 0-{fs.Length - 1}/{fs.Length}");
                response.AddHeader("Content-Type", "audio/x-wav");
                response.AddHeader("Last-Modified", File.GetLastWriteTime(filePath).ToString());

                byte[] buffer = new byte[1024 * 10];
                int read;

                while ((read = fs.Read(buffer, 0, buffer.Length)) > 0)
                {
                    response.OutputStream.Write(buffer, 0, read);
                }
            }

            response.OutputStream.Flush();
            response.OutputStream.Close();
        }


    }


}
