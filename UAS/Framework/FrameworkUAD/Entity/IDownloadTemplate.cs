using System;

namespace FrameworkUAD.Entity
{
    public interface IDownloadTemplate
    {
        int DownloadTemplateID { get; set; }
        string DownloadTemplateName { get; set; }
        int BrandID { get; set; }
        int PubID { get; set; }
        int? CreatedUserID { get; set; }
        DateTime CreatedDate { get; set; }
        int? UpdatedUserID { get; set; }
        DateTime UpdatedDate { get; set; }
        bool IsDeleted { get; set; }
    }
}
