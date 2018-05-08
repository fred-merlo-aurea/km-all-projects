using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace KMPS.MD.Helpers
{
    public static class FileHelper
    {
        private const string LogosPath = "../Images/logo/";
        private const string FileSuffixDateTimeFormat = "MM-dd-yyyy hh-mm-ss";
        private static readonly string[] LogoFileExtenions = {".gif", ".jpg", ".png"};

        public static SaveFileResult SaveLogo(HttpPostedFile postedFile, int customerId, HttpServerUtility serverUtility)
        {
            if (postedFile == null)
            {
                throw new ArgumentNullException(nameof(postedFile));
            }
            if (serverUtility == null)
            {
                throw new ArgumentNullException(nameof(serverUtility));
            }
            var file = postedFile;
            var server = serverUtility;

            var fileName = string.Format("{0}{1}{2}",
                Path.GetFileNameWithoutExtension(file.FileName),
                DateTime.Now.ToString(FileSuffixDateTimeFormat),
                Path.GetExtension(file.FileName));

            if (string.IsNullOrWhiteSpace(fileName))
            {
                return SaveFileResult.Fail("Please select logo to upload.");
            }

            if (!LogoFileExtenions.Any(ext => fileName.EndsWith(ext, StringComparison.OrdinalIgnoreCase)))
            {
                return SaveFileResult.Fail($"ERROR - Cannot Upload File: <br><br>Only files with an extension of {string.Join(", ", LogoFileExtenions)} are supported.");
            }

            var path = $"{LogosPath}{customerId}/";

            try
            {
                if (!Directory.Exists(server.MapPath(path)))
                {
                    Directory.CreateDirectory(server.MapPath(path));
                }

                if (File.Exists(server.MapPath(path) + fileName))
                {
                    return SaveFileResult.Fail("Image with same name already exists.");
                }

                file.SaveAs(server.MapPath(path) + fileName);
            }
            catch (Exception ex)
            {
                return SaveFileResult.Fail($"An error has occured uploading your file.<br /><br />{ex.Message}");
            }

            return SaveFileResult.Succeed($"{path}{fileName}", fileName);
        }
    }
}