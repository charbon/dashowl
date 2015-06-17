using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace DashOwl.WebAPI
{
    /// <summary>
    /// API control for media files upload
    /// </summary>
    public class UploadController : ApiController
    {
        /// <summary>
        /// Creates a new
        /// </summary>
        /// <returns></returns>
        public async Task<HttpResponseMessage> Post()
        {
            //Check whether the POST operation is Multipart.
            if(!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            //Prepare CustomMultipartFormDataSteamProvider in which  our multiparts from data will be loaded

            string fileSaveLocation = HttpContext.Current.Server.MapPath("~/MediaStorage");

            CustomMultipartFormDataStreamProvider provider = new CustomMultipartFormDataStreamProvider(fileSaveLocation);
            List<string> files = new List<string>();

            try
            {
                //Read all content of the multipart message into CustomMultipartFormDataStreamProvider
                await Request.Content.ReadAsMultipartAsync(provider);

                foreach (MultipartFileData file in provider.FileData)
                {
                    files.Add("/MediaStorage/" + Path.GetFileName(file.LocalFileName));
                }

                return Request.CreateResponse(HttpStatusCode.OK, files);
            }
            catch(System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
    }
}
