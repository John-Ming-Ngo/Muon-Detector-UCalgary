using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CosmicWatchServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UploadFileController : ControllerBase
    {
        private readonly ILogger<UploadFileController> _Logger;
        private readonly IWebHostEnvironment _Environment;

        private String KeyCode;

        public UploadFileController(ILogger<UploadFileController> logger,
            IWebHostEnvironment environment)
        {
            _Logger = logger;
            _Environment = environment ?? throw new ArgumentNullException(nameof(environment));
            //Load in the Key Code.
            String FilePath = Path.Combine(_Environment.ContentRootPath, "KeyFolder");
            Directory.CreateDirectory(FilePath);
            String KeyFilePath = Path.Combine(FilePath, "Key.txt");
            KeyCode = System.IO.File.ReadAllText(KeyFilePath);
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            try
            {
                HttpRequest httpRequest = HttpContext.Request;

                HttpContext httpContext = httpRequest.HttpContext;

                String AuthHeader = httpContext.Request.Headers["Authorization"];
                
                if (AuthHeader != null && AuthHeader.StartsWith("Basic"))
                {
                    String Key = AuthHeader.Substring("Basic ".Length).Trim();

                    if (KeyCode != Key) throw new Exception("Failed Authentication");
                }
                else
                {
                    throw new Exception("No Authentication");
                }
                if (httpRequest.Form.Files.Count > 0)
                {
                    foreach (IFormFile file in httpRequest.Form.Files)
                    {
                        String FilePath = Path.Combine(_Environment.ContentRootPath, "Uploads");
                        Directory.CreateDirectory(FilePath);
                        using MemoryStream MemoryStream = new MemoryStream();
                        await file.CopyToAsync(MemoryStream);
                        System.IO.File.WriteAllBytes(Path.Combine(FilePath, file.FileName), MemoryStream.ToArray());
                    }
                    return Ok();
                }
            }
            catch (Exception e)
            {
                _Logger.LogError(e, "Error");
                return BadRequest();
            }
            return BadRequest();
        }
    }
}
