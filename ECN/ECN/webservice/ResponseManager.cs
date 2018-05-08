namespace ecn.webservice.classes
{
    public class ResponseManager : IResponseManager
    {
        public string GetResponse(string webMethod, SendResponse.ResponseCode status, int id, string output)
        {
            return SendResponse.response(webMethod, status, id, output);
        }

        public string GetResponse(string webMethod, string status, int id, string output)
        {
            return SendResponse.response(webMethod, status, id, output);
        }
    }
}
