using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WeaponServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImaggaController : ControllerBase
    {
        private const string ApiKey = "acc_363d732b2281b6e";
        private const string ApiSecret = "d54ee367e04f912255db312689a2fe10";
        private const string ImaggaApiUrl = "https://api.imagga.com/v2/tags";

        [HttpGet("classify")]
        public async Task<IActionResult> ClassifyImage([FromQuery] string imageUrl)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    string basicAuthValue = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{ApiKey}:{ApiSecret}"));
                    httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {basicAuthValue}");

                    var response = await httpClient.GetAsync($"{ImaggaApiUrl}?image_url={imageUrl}");
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        return Ok(content);
                    }
                    else
                    {
                        return StatusCode((int)response.StatusCode, "Failed to classify image.");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
