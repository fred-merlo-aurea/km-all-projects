namespace KM.Integration.Marketo
{
	public class AddLeadsToListResponse
	{
		public string RequestId { get; set; }
		public bool Success { get; set; }
		public Result[] Result { get; set; }
        public Error[] Errors { get; set; }
	}
}
