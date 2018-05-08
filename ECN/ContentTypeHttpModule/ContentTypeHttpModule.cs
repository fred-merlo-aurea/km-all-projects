using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace ContentTypeHttpModule
{
    public class ContentTypeHttpModule : IHttpModule
    {
        private const string JSON_CONTENT_TYPE = "application/json; charset=utf-8";

        #region IHttpModule Members
        public void Dispose()
        {
        }

        public void Init(HttpApplication app)
        {
            app.BeginRequest += OnBeginRequest;
            app.ReleaseRequestState += OnReleaseRequestState;
        }
        #endregion

        public void OnBeginRequest(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;
            HttpRequest resquest = app.Request;
            if (!resquest.Url.AbsolutePath.Contains("DomainTracker.asmx")) return;

            if (string.IsNullOrEmpty(app.Context.Request.ContentType))
            {
                app.Context.Request.ContentType = JSON_CONTENT_TYPE;
            }
        }

        public void OnReleaseRequestState(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;
            HttpResponse response = app.Response;
            if (app.Context.Request.ContentType != JSON_CONTENT_TYPE) return;

            response.Filter = new JsonResponseFilter(response.Filter);
        }
    }

    public class JsonResponseFilter : Stream
    {
        private readonly Stream _responseStream;
        private long _position;

        public JsonResponseFilter(Stream responseStream)
        {
            _responseStream = responseStream;
        }

        public override bool CanRead { get { return true; } }

        public override bool CanSeek { get { return true; } }

        public override bool CanWrite { get { return true; } }

        public override long Length { get { return 0; } }

        public override long Position { get { return _position; } set { _position = value; } }

        public override void Write(byte[] buffer, int offset, int count)
        {
            string strBuffer = Encoding.UTF8.GetString(buffer, offset, count);
            strBuffer = AppendJsonpCallback(strBuffer, HttpContext.Current.Request);
            byte[] data = Encoding.UTF8.GetBytes(strBuffer);
            _responseStream.Write(data, 0, data.Length);
        }

        private string AppendJsonpCallback(string strBuffer, HttpRequest request)
        {
            return request.Params["callback"] + "(" + strBuffer + ");";
        }

        public override void Close()
        {
            _responseStream.Close();
        }

        public override void Flush()
        {
            _responseStream.Flush();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _responseStream.Seek(offset, origin);
        }

        public override void SetLength(long length)
        {
            _responseStream.SetLength(length);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _responseStream.Read(buffer, offset, count);
        }
    }
}