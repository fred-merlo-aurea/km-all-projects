using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;

public class ReCaptchaClass
{

    [JsonProperty("success")]
    public bool Success { get; set; }

    [JsonProperty("error-codes")]
    public List<string> ErrorCodes { get; set; }


    public static bool Validate(string EncodedResponse)
    {
        var client = new System.Net.WebClient();

        string PrivateKey = ConfigurationManager.AppSettings["Google.ReCaptcha.Secret"].ToString();

        var GoogleReply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", PrivateKey, EncodedResponse));

        var captchaResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<ReCaptchaClass>(GoogleReply);

        return captchaResponse.Success;
    }
}