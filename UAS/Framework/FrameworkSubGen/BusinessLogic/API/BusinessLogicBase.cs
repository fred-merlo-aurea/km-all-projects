using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using static FrameworkSubGen.Entity.Enums;

namespace FrameworkSubGen.BusinessLogic.API
{
    public class BusinessLogicBase
    {
        protected string Delete(Client client, string uriExtension, NameValueCollection requestParameter)
        {
            try
            {
                var authentication = new Authentication();
                using (var webClient = authentication.GetClient(client))
                {
                    var responseBytes = webClient.UploadValues(
                        $"{authentication.BaseUri}{uriExtension}",
                        HttpMethod.DELETE.ToString(),
                        requestParameter);

                    return Encoding.UTF8.GetString(responseBytes);
                }
            }
            catch (Exception exception) when (exception is WebException
                                              || exception is ArgumentNullException
                                              || exception is DecoderFallbackException
                                              || exception is ArgumentException)
            {
                Authentication.SaveApiLog(exception, GetType().ToString(), GetType().Name);
            }

            return string.Empty;
        }
    }
}
