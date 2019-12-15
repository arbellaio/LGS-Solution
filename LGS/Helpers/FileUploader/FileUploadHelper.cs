using System;
using System.IO;
using System.Web;
using System.Web.Helpers;

namespace LGS.Helpers.FileUploader
{
    public class FileUploadHelper
    {
        public string SaveFile(HttpPostedFileBase file, string pathToUpload, string email)
        {
            if (!Directory.Exists(pathToUpload))
            {
                Directory.CreateDirectory(pathToUpload); //Create Path of not exists
            }

            if (!string.IsNullOrEmpty(email))
            {
                var userName = (email).Split('@')[0];
                if (!Directory.Exists(pathToUpload+"\\"+userName))
                {
                    Directory.CreateDirectory(pathToUpload + "\\" + userName); //Create Path of not exists
                }
                pathToUpload = pathToUpload + "\\" + userName;
            }
            var imageName = GetFileName(file,false);
            string pathwithfileName = pathToUpload + "\\" + imageName;
            var img = new WebImage(file.InputStream);
            img.Resize(512, 512,false);
            img.Save(pathwithfileName);
            
            return pathwithfileName;
        }


        public string CompanySaveFile(HttpPostedFileBase file, string pathToUpload, string email)
        {
            if (!Directory.Exists(pathToUpload))
            {
                Directory.CreateDirectory(pathToUpload); //Create Path of not exists
            }

            if (!string.IsNullOrEmpty(email))
            {
                var userName = (email).Split('@')[1];
                if (!Directory.Exists(pathToUpload + "\\" + userName))
                {
                    Directory.CreateDirectory(pathToUpload + "\\" + userName); //Create Path of not exists
                }
                pathToUpload = pathToUpload + "\\" + userName;
            }
            var imageName = GetFileName(file, false);
            string pathwithfileName = pathToUpload + "\\" + imageName;
            var img = new WebImage(file.InputStream);
            img.Resize(512, 512, false);
            img.Save(pathwithfileName);

            return pathwithfileName;
        }


        //      


        public string GetFileName(HttpPostedFileBase file, bool BuildUniqeName)
        {
            string fileName = string.Empty;

            string fileExtension = GetFileExtension(file);
            if (BuildUniqeName)
            {
                string strUniqName = GetUniqueName("img");
                fileName = strUniqName + fileExtension;
            }
            else
            {
                fileName = file.FileName;
            }
            return fileName;
        }

        public string GetUniqueName(string preFix)
        {
            string uName = preFix + DateTime.Now.ToString()
                               .Replace("/", "-")
                               .Replace(":", "-")
                               .Replace(" ", string.Empty)
                               .Replace("PM", string.Empty)
                               .Replace("AM", string.Empty);
            return uName;
        }

        public string GetFileExtension(HttpPostedFileBase file)
        {
            string fileExtension;
            fileExtension = (file != null)
                ? file.FileName.Substring(file.FileName.LastIndexOf('.')).ToLower()
                : string.Empty;
            return fileExtension;
        }
    }
}