using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Services.Services.Contcrete;
using Microsoft.AspNetCore.Http;


namespace App.Services.Services.Abstract
{
    public interface IFileService
    {
        List<string> GetFiles();
        Task UploadFileAsync(IFormFile formFile);
        Task<FileResponse?> DownloadFileAsync(string fileName);
        Task DeleteAsync(string fileName);
    }
}
