namespace ecn.webservice.classes
{
    public interface IResponseManager
    {
        string GetResponse(string webMethod, SendResponse.ResponseCode status, int id, string output);
        string GetResponse(string webMethod, string status, int id, string output);
    }
}