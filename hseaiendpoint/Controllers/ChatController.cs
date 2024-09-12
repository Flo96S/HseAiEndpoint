using hseaiendpoint.Models;
using hseaiendpoint.Services;
using hseaiendpoint.Src;
using Microsoft.AspNetCore.Mvc;

namespace hseaiendpoint.Controllers
{
    public class ChatController : ControllerBase
    {
        private readonly ILogger<ChatController> _logger;
        public ChatController(ILogger<ChatController> logger)
        {
            _logger = logger;
        }

        [HttpPost("/chat")]
        public async Task<IActionResult> Chat([FromBody] SendMessageInput text, [FromServices] StatefulChatService _service)
        {
            string result = await _service.Send(text);
            result = CleanOutput.Clean(result);
            return Ok(result);
        }

        [HttpPost("/chat/stream")]
        public async Task SendMessageStream([FromBody] SendMessageInput text, [FromServices] StatefulChatService _service, CancellationToken cancel)
        {
            Response.ContentType = "text/event-stream";

            await foreach (var r in _service.SendStream(text))
            {
                await Response.WriteAsync(r, cancel);
                await Response.Body.FlushAsync(cancel);
            }
            await Response.CompleteAsync();
        }
    }
}
