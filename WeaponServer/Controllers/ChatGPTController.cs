﻿using Microsoft.AspNetCore.Mvc;
using OpenAI.API.Completions;
using OpenAI.API;

namespace WeaponServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatGPTController : ControllerBase
    {
        private readonly OpenAIAPI _openAiApi;

        public ChatGPTController()
        {
            // OpenAI API key
            var apiKey = ""; // Enter your API key from OpanAI
            _openAiApi = new OpenAIAPI(apiKey);
        }

        // POST: api/ChatGPT
        [HttpPost]
        public async Task<IActionResult> PostChatGPT([FromBody] ChatRequest request)
        {
            // Check if the request is valid
            if (request == null || string.IsNullOrWhiteSpace(request.Message))
            {
                return BadRequest("Requête invalide.");
            }

            // Call OpenAI API
            try
            {
                var completionRequest = new CompletionRequest
                {
                    Prompt = request.Message,
                    Model = "",  // Enter a valid model according to your credential account
                    MaxTokens = 100,
                    Temperature = 0.7,
                    TopP = 1.0,
                };

                // Get the response from OpenAI
                var result = await _openAiApi.Completions.CreateCompletionAsync(completionRequest);

                // Get the response text
                var responseText = result.Completions[0].Text.Trim();

                return Ok(new { response = responseText });
            }
            
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error : {ex.Message}");
            }
        }
    }

    public class ChatRequest
    {
        public string? Message { get; set; }
    }
}

