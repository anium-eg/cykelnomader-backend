using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CycleManagement.Services
{

    public class ImageServerService
    {
        readonly IConfiguration _configuration;
        private string? _imageServerLink;
        private static HttpClient _httpClient = new HttpClient();

        public ImageServerService(IConfiguration configuration) {
            _configuration = configuration;
            _imageServerLink = _configuration.GetSection("ExternalHosts:ImageServer").Value;
        }

        public async Task<string> uploadImage(IFormFile fileToUpload, string location)
        {
            Stream stream = fileToUpload.OpenReadStream();
            StreamContent fileContent = new StreamContent(stream);

            // Add content type if known
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(fileToUpload.ContentType);

            MultipartFormDataContent form = new MultipartFormDataContent();

            // Include the filename in the form data (this is the key change)
            form.Add(fileContent, "file", fileToUpload.FileName);
            form.Add(new StringContent(location), "location");

            HttpResponseMessage uploadResponse = await _httpClient.PostAsync(_imageServerLink + "/upload", form);
            uploadResponse.EnsureSuccessStatusCode();

            string resultJson = await uploadResponse.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(resultJson);
            string result = json.GetValue("url").ToString();

            return result;
        }
    }
}
