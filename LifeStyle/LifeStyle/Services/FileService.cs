using System.IO;
using System;
using Core;
using Core.RequestModel;
using System.Linq;
using StackExchange.Profiling.Internal;
using System.Drawing;
using Core.Extensions;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using AgileObjects.AgileMapper.Extensions;
using LifeStyle;

namespace LifeStyle.Services
{
    public class FileService
    {

        void FileValidate(FileRequest req)
        {
            if (req.File == null)
                throw Constants.Invalid("File");
            if (!Constants.FileTypes.Contains(req.Type))
                throw Constants.Invalid("File Type");
        }

       
        public Core.Models.File Upload(FileRequest req, int userId)
        {
            FileValidate(req);
            Common.Log.Data($"Uploading file..", $"UserId:- {userId}");
            var fileResult = new Core.Models.File();
            if (req.File != null)
            {
                var imageUrl = FileUpload(req.File);
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    fileResult.FileUrl = imageUrl.Replace(Utility.GetBaseUrl(), "");
                    fileResult.FileName = req.File.FileName;
                    fileResult = req.Map().Over(fileResult);
                }
            }
            else
            {
                return fileResult;
            }
            fileResult = Common.Instances.FileInst.Save(fileResult, userId);
            return fileResult;
        }

        public string FileUpload(IFormFile file)
        {
            string imageUrl = "";
            var filename = DateTime.Now.ToFileTime() + file.FileName.Replace(" ", "_");
            var path = Path.Combine(Common.UploadPath);
            if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }
            using (var fileStream = new FileStream(Path.Combine(path, filename), FileMode.Create))
            {
                imageUrl = $"{Utility.GetBaseUrl()}{Common.UploadPath}/{filename}";
                file.CopyTo(fileStream);
            }
             return imageUrl;
        }
    }
}
