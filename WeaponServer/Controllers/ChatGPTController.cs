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
            // Clé API OpenAI
            var apiKey = "sk-proj-6sxvn51HbGYugBK0e8AVT3BlbkFJkcKe0hagKuqhtkCYvJgR"; // Remplacez par votre clé OpenAI
            _openAiApi = new OpenAIAPI(apiKey);
        }

        // POST: api/ChatGPT
        [HttpPost]
        public async Task<IActionResult> PostChatGPT([FromBody] ChatRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Message))
            {
                return BadRequest("Requête invalide.");
            }

            try
            {
                var completionRequest = new CompletionRequest
                {
                    Prompt = request.Message,
                    Model = "gpt-3.5-turbo",  // Utilisez un modèle valide
                    MaxTokens = 100,
                    Temperature = 0.7,
                    TopP = 1.0,
                };

                var result = await _openAiApi.Completions.CreateCompletionAsync(completionRequest);

                var responseText = result.Completions[0].Text.Trim();

                return Ok(new { response = responseText });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur du serveur : {ex.Message}");
            }
        }
    }

    public class ChatRequest
    {
        public string Message { get; set; }
    }
}
