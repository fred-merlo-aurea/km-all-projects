namespace KMPS.MD.Helpers
{
    public class SaveFileResult
    {
        public bool Succeeded { get; set; }
        public string Url { get; set; }
        public string FileName { get; set; }
        public string ErrorMessage { get; set; }

        public static SaveFileResult Succeed(string url, string fileName)
        {
            return new SaveFileResult
            {
                Succeeded = true,
                Url = url,
                FileName = fileName
            };
        }

        public static SaveFileResult Fail(string errorMessage)
        {
            return new SaveFileResult
            {
                Succeeded = false,
                ErrorMessage = errorMessage
            };
        }
    }
}
