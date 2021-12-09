using System;
using System.IO;

using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CosmicWatch_Library
{
    public static class WebServerFileHandler
    {
        //Post
        public static async Task<bool> Post(String ServerToUploadTo, String FileLocation, String FileToSend, String key)
        {
            using FileStream FileToSendStream = File.OpenRead(Path.Combine(new string[] { FileLocation, FileToSend }));
            using MultipartFormDataContent HttpContent = new MultipartFormDataContent();

            HttpContent.Add(new StreamContent(FileToSendStream), "file", FileToSend);

            using HttpClient WebClient = new HttpClient();
            
            WebClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", key);

            try
            {
                using HttpResponseMessage Response = await WebClient.PostAsync(ServerToUploadTo, HttpContent);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
