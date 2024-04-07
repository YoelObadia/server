using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace WeaponServer.ImaggaController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImaggaController : ControllerBase
    {
        [HttpGet]
        [Route("classify")]
        public IActionResult ClassifyImage([FromQuery(Name = "imageUrl")] string imageUrl)
        {
            string apiKey = "acc_363d732b2281b6e";
            string apiSecret = "d54ee367e04f912255db312689a2fe10";

            string basicAuthValue = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{apiKey}:{apiSecret}"));

            var client = new RestClient("https://api.imagga.com/v2/tags");
            var request = new RestRequest(new Uri("https://api.imagga.com/v2/tags"), Method.Get);
            request.AddParameter("image_url", imageUrl);
            request.AddHeader("Authorization", $"Basic {basicAuthValue}");

            RestResponse response = client.Execute(request);
            return Ok(response.Content);
        }
    }
}
