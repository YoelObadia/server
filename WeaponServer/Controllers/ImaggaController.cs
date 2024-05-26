using Microsoft.AspNetCore.Mvc;
using RestSharp;
using AspNetCoreWebApi6.Models;
using System.Text.Json;

namespace WeaponServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImaggaController : ControllerBase
    {
        private readonly WeaponContext _dbContext;
        private readonly ILogger<ImaggaController> _logger;

        public ImaggaController(WeaponContext dbContext, ILogger<ImaggaController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet]
        [Route("classify")]
        public async Task<IActionResult> ClassifyByKeyword([FromQuery] string keyword)
        {
            var matchingWeapons = new HashSet<Weapon>();

            // Recuparate the image urls from database to share with Imagga
            var allImages = _dbContext.Weapons.Select(w => w.Images).ToList();

            // Imagga API credentials
            string apiKey = ""; // Enter your API key from Imagga
            string apiSecret = ""; // Enter your API secret key from Imagga

            // Encode the credentials to base64
            string basicAuthValue = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{apiKey}:{apiSecret}"));

            // Call Imagga API
            var client = new RestClient("https://api.imagga.com/v2/tags");

            // Create a request to Imagga
            var request = new RestRequest(new Uri("https://api.imagga.com/v2/tags"), Method.Get);

            // Add the Authorization header
            request.AddHeader("Authorization", $"Basic {basicAuthValue}");

            // Share URLs to Imagga to get tags for each image and compare with the keyword
            foreach (var imageUrl in allImages)
            {
                request.AddOrUpdateParameter("image_url", imageUrl!, ParameterType.QueryString);
                var response = await client.ExecuteAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var imaggaResponse = JsonSerializer.Deserialize<ImaggaResponse>(response.Content!);

                    var tags = imaggaResponse?.result?.tags?.Select(t => t.tag?.en).ToList() ?? new List<string>()!;

                    if (tags.Contains(keyword, StringComparer.OrdinalIgnoreCase))
                    {
                        var matchingWeapon = _dbContext.Weapons.FirstOrDefault(w => w.Images == imageUrl);
                        if (matchingWeapon != null)
                        {
                            matchingWeapons.Add(matchingWeapon);
                        }
                    }
                }
                else
                {
                    _logger.LogError($"Erreur lors de l'appel à Imagga: {response.Content}");
                }
            }

            // Comparison with database
            var allWeapons = _dbContext.Weapons.ToList(); // Loading data from the database
            var localMatches = allWeapons
                .Where(w =>
                    w.Name!.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0 || 
                    w.Type!.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    w.Manufacturer!.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    w.Caliber!.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0
                )
                .ToList();

            // Add local matches without duplicates
            foreach (var weapon in localMatches)
            {
                matchingWeapons.Add(weapon);
            }
            
            return Ok(matchingWeapons.ToList());
        }
    }

    public class ImaggaResponse
    {
        public Result? result { get; set; }
    }

    public class Result
    {
        public List<Tag>? tags { get; set; }
    }

    public class Tag
    {
        public TagDetails? tag { get; set; }
    }

    public class TagDetails
    {
        public string? en { get; set; }
    }
}
