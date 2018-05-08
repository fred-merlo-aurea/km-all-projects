using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using UAD.API;

namespace UAD.API.Exceptions
{
    public class ImageException : Exception
    {
        public ImageException(string message) : base(message)
        {
            this.Data[Strings.Errors.ImageExceptionDataKey] = message;
        }

        public static string GetImageExceptionMessage(ImageException imageException)
        {
            return imageException.Data[Strings.Errors.ImageExceptionDataKey].ToString();
        }
    }
}