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
        private readonly ILogger<UploadFileController> _logger;
        private readonly IWebHostEnvironment _environment;

        public UploadFileController(ILogger<UploadFileController> logger,
            IWebHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            try
            {
                HttpRequest httpRequest = HttpContext.Request;

                HttpContext httpContext = httpRequest.HttpContext;

                string authHeader = httpContext.Request.Headers["Authorization"];
                
                if (authHeader != null && authHeader.StartsWith("Basic"))
                {
                    string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();

                    //Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                    //string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));

                    //int seperatorIndex = usernamePassword.IndexOf(':');

                    //var username = usernamePassword.Substring(0, seperatorIndex);
                    //var password = usernamePassword.Substring(seperatorIndex + 1);
                }
                else
                {
                    throw new Exception("Failed Authentication");
                }
                if (httpRequest.Form.Files.Count > 0)
                {
                    foreach (IFormFile file in httpRequest.Form.Files)
                    {
                        string filePath = Path.Combine(_environment.ContentRootPath, "uploads");
                        Directory.CreateDirectory(filePath);
                        using MemoryStream memoryStream = new MemoryStream();
                        await file.CopyToAsync(memoryStream);
                        System.IO.File.WriteAllBytes(Path.Combine(filePath, file.FileName), memoryStream.ToArray());
                    }
                    return Ok();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error");
                return new StatusCodeResult(500);
            }

            return BadRequest();
        }
    }
}
