using System.IO;
using FrameworkUAD.Object;
using FrameworkUAS.Entity;
using KMPlatformEntity = KMPlatform.Entity;

namespace Core.ADMS.Events
{
	public class FileBase
	{
		public FileInfo ImportFile { get; set; }
		public KMPlatformEntity.Client Client { get; set; }
		public bool IsKnownCustomerFileName { get; set; }
		public bool IsValidFileType { get; set; }
		public bool IsFileSchemaValid { get; set; }
		public SourceFile SourceFile { get; set; }
		public ValidationResult ValidationResult { get; set; }
		public AdmsLog AdmsLog { get; set; }
	}
}