using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WzFrame.Shared.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
                return BadRequest("没有选择文件");

            var dateFolder = DateTime.Now.ToString("yyyyMMdd");
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", dateFolder);

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var filePaths = new List<string>();

            var request = HttpContext.Request;
            var serverUrl = $"{request.Scheme}://{request.Host}/uploads/{dateFolder}";

            foreach (var file in files)
            {
                var path = Path.Combine(uploadPath, file.FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                var fileUrl = $"{serverUrl}/{file.FileName}";
                filePaths.Add(fileUrl);
            }

            return Ok(new { filePaths });
        }
    }
}
