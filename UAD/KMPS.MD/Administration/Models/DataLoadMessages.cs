namespace KMPS.MD.Administration.Models
{
    public class DataLoadMessages
    {
        public string Step1Status { get; set; }
        public string Step2Status { get; set; }
        public string Step3Status { get; set; }
        public string Step4Status { get; set; }

        public void Reset()
        {
            Step1Status = string.Empty;
            Step2Status = string.Empty;
            Step3Status = string.Empty;
            Step4Status = string.Empty;
        }
    }
}