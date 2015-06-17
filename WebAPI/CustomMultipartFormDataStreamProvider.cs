using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace DashOwl.WebAPI
{
    public class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        public CustomMultipartFormDataStreamProvider(string path) : base(path) { }

        public override string GetLocalFileName(System.Net.Http.Headers.HttpContentHeaders headers)
        {
            return headers.ContentDisposition.FileName.Replace("\"", string.Empty);
        }
    }
}