using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ValidationDemo.ViewModel
{
    public class ValidationProto
    {
        [Display(Name ="Select Folder:")]
        [Required(ErrorMessage = "Please select Folder.")]
        public int FolderID { get; set; }

        [Display(Name = "Layout Name:")]
        [Required(ErrorMessage = "Please provide Layout Name.")]
        [MinLength(5,ErrorMessage ="Layout Name should of minimum 5 character.")]
        public string LayoutName { get; set; }


        [Required(ErrorMessage = "TemplateID cannot be empty.")]
        [Display(Name = "TemplateID:")]
        public int TemplateID { get; set; }

        [Display(Name = "ContentID:")]
        [Required(ErrorMessage ="Content should be selected.")]
        public int ContentID { get; set; }

        public List<Model.ECNError> ErrorList { get; set; }
        public List<SelectListItem> FolderList { get; set; }

        public List<SelectListItem> ContentList { get; set; }
        
        public ValidationProto()
        {
            this.FolderID = 0;
            this.LayoutName = "";
            this.ContentID = 0;
            this.TemplateID = 0;
            FolderList = new List<SelectListItem>();
            ContentList = new List<SelectListItem>();
            ErrorList = new List<Model.ECNError>();
        }
}

}

namespace ValidationDemo.Model
{
    public class ECNException : Exception
    {
        public List<ECNError> ErrorList { get; private set; }
        public ECNException(List<ECNError> errorList)
        {
            ErrorList = errorList;
        }
    }

    [Serializable]
    public class ECNError
    {
        public string ErrorMessage { get; set; }

        public ECNError()
        {
        }

        public ECNError(string error)
        {
            ErrorMessage = error;
        }
    }

    [Serializable]
    public class Layout
    {
        public Layout()
        {
            LayoutID = 0;
            FolderID = 0;
            ContentID = 0;
            TemplateID = 0;
            LayoutName = string.Empty;
        }
        public int LayoutID { get; set; }
        public int FolderID { get; set; }
        public int ContentID { get; set; }
        public int TemplateID { get; set; }
        public string LayoutName { get; set; }
    }

    [Serializable]
    public class Folder
    {
        public Folder()
        {
            FolderID = 0;
            FolderName = string.Empty;
        }
        public int FolderID { get; set; }
        public string FolderName { get; set; }
    }

    [Serializable]
    public class Content
    {
        public Content()
        {
            ContentID = 0;
            ContentName = string.Empty;
        }
        public int ContentID { get; set; }
        public string ContentName { get; set; }
    }

    [Serializable]
    public class Template
    {
        public Template()
        {
            TemplateID = 0;
            TemplateName = string.Empty;
        }
        public int TemplateID { get; set; }
        public string TemplateName { get; set; }
    }
}

namespace ValidationDemo.Business
{
    [Serializable]
    public class Layout
    {
        public static void Save(Model.Layout layout)
        {
            Validate(layout);
            Data.Layout.Save(layout);
        }
        public static void Validate(Model.Layout layout)
        {
            List<Model.ECNError> errorList = new List<Model.ECNError>();
            if (layout.FolderID == 0 || (!Folder.Exists(layout.FolderID)))
                errorList.Add(new Model.ECNError("FolderID is invalid"));
            if (layout.ContentID == 0 || (!Content.Exists(layout.ContentID)))
                errorList.Add(new Model.ECNError("ContentID is invalid"));
            if (layout.TemplateID == 0 || (!Template.Exists(layout.TemplateID)))
                errorList.Add(new Model.ECNError("TemplateID is invalid"));
            if (layout.LayoutName.Trim().Length <= 0 || (Layout.Exists(layout.LayoutName)))
                errorList.Add(new Model.ECNError("LayoutName is duplicate"));

            if (errorList.Count > 0)
            {
                throw new Model.ECNException(errorList);
            }
        }

        public static bool Exists(string layoutName)
        {
            return Data.Layout.Exists(layoutName);
        }
    }

    [Serializable]
    public class Folder
    {
        public static bool Exists(int folderID)
        {
            return Data.Folder.Exists(folderID);
        }
    }

    [Serializable]
    public class Content
    {
        public static bool Exists(int contentID)
        {
            return Data.Content.Exists(contentID);
        }
    }

    [Serializable]
    public class Template
    {
        public static bool Exists(int templateID)
        {
            return Data.Template.Exists(templateID);
        }
    }

}

namespace ValidationDemo.Data
{
    [Serializable]
    public class Layout
    {
        public static void Save(Model.Layout layout)
        {
            //save to db
        }

        public static bool Exists(string layoutName)
        {
            bool exists = false;
            if (LoadData().Exists(x => x.LayoutName == layoutName))
                exists = true;
            return exists;
        }

        public static List<Model.Layout> LoadData()
        {
            List<Model.Layout> layoutList = new List<Model.Layout>();
            ValidationDemo.Model.Layout layout = new Model.Layout();
            layout.LayoutID = 1;
            layout.FolderID = 1;
            layout.ContentID = 1;
            layout.TemplateID = 1;
            layout.LayoutName = "Layout One";
            layoutList.Add(layout);
            layout = new Model.Layout();
            layout.LayoutID = 2;
            layout.FolderID = 2;
            layout.ContentID = 2;
            layout.TemplateID = 2;
            layout.LayoutName = "Layout Two";
            layoutList.Add(layout);

            return layoutList;
        }
    }

    [Serializable]
    public class Folder
    {
        public static bool Exists(int folderID)
        {
            bool exists = false;
            if (LoadData().Exists(x => x.FolderID == folderID))
                exists = true;
            return exists;
        }
        public static List<Model.Folder> LoadData()
        {
            List<Model.Folder> folderList = new List<Model.Folder>();
            ValidationDemo.Model.Folder folder = new Model.Folder();
            folder.FolderID = 1;
            folder.FolderName = "Folder One";
            folderList.Add(folder);
            folder = new Model.Folder();
            folder.FolderID = 2;
            folder.FolderName = "Folder Two";
            folderList.Add(folder);

            return folderList;
        }
    }

    [Serializable]
    public class Content
    {
        public static bool Exists(int contentID)
        {
            bool exists = false;
            if (LoadData().Exists(x => x.ContentID == contentID))
                exists = true;
            return exists;
        }
        public static List<Model.Content> LoadData()
        {
            List<Model.Content> contentList = new List<Model.Content>();
            ValidationDemo.Model.Content content = new Model.Content();
            content.ContentID = 1;
            content.ContentName = "Content One";
            contentList.Add(content);
            content = new Model.Content();
            content.ContentID = 2;
            content.ContentName = "Content Two";
            contentList.Add(content);

            return contentList;
        }
    }

    [Serializable]
    public class Template
    {
        public static bool Exists(int templateID)
        {
            bool exists = false;
            if (LoadData().Exists(x => x.TemplateID == templateID))
                exists = true;
            return exists;
        }
        public static List<Model.Template> LoadData()
        {
            List<Model.Template> templateList = new List<Model.Template>();
            ValidationDemo.Model.Template template = new Model.Template();
            template.TemplateID = 1;
            template.TemplateName = "Template One";
            templateList.Add(template);
            template = new Model.Template();
            template.TemplateID = 2;
            template.TemplateName = "Template Two";
            templateList.Add(template);

            return templateList;
        }
    }
}
