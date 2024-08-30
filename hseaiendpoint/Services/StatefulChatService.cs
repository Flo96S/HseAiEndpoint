using hseaiendpoint.Models;
using hseaiendpoint.Src;
using LLama;
using Microsoft;
using System.Runtime.CompilerServices;
using System.Text;

namespace hseaiendpoint.Services
{
    public class StatefulChatService : IDisposable
    {
        private readonly ChatSession _session;
        private readonly LLamaContext _content;
        private readonly ILogger<StatefulChatService> _logger;
        private bool _continue = false;

        private string SystemPrompt = "";

        public StatefulChatService(IConfiguration config, ILogger<StatefulChatService> logger)
        {
            var @params = new LLama.Common.ModelParams(config["AiModel"]!)
            {
                ContextSize = 2048,
            };

            using var weight = LLamaWeights.LoadFromFile(@params);

            _logger = logger;
            _content = new LLamaContext(weight, @params);
            _session = new ChatSession(new InteractiveExecutor(_content));
            //SystemPrompt change
            SystemPromptGetter getter = new();
            SystemPrompt = getter.GetSystemPrompt();
            _session.History.AddMessage(LLama.Common.AuthorRole.System, SystemPrompt);
        }

        public void Dispose()
        {
            _content?.Dispose();
        }

        public async Task<string> Send(SendMessageInput input)
        {
            if(!_continue)
            {
                _logger.LogInformation("Prompt: {0}", SystemPrompt);
                _continue = true;
            }
            _logger.LogInformation("Input: {0}", input.Text);
            var outputs = _session.ChatAsync(
                new LLama.Common.ChatHistory.Message(LLama.Common.AuthorRole.User, input.Text),
                new LLama.Common.InferenceParams()
                {
                    RepeatPenalty = 0.0f,
                    AntiPrompts = new string[] {"User:", "Q:", "¥" }
                });
            StringBuilder builder = new();
            await foreach(var output in outputs)
            {
                _logger.LogInformation("Message: {out}", output);
                builder.Append(output);
            }
            return builder.ToString();
        }

        public async IAsyncEnumerable<string> SendStream(SendMessageInput input)
        {
            if(!_continue)
            {
                _logger.LogInformation(SystemPrompt);
                _continue = true;
            }

            _logger.LogInformation(input.Text);

            var outputs = _session.ChatAsync(
                new LLama.Common.ChatHistory.Message(LLama.Common.AuthorRole.User, input.Text),
                new LLama.Common.InferenceParams()
                {
                    RepeatPenalty = 0.0f,
                    AntiPrompts = new string[] { "User:", "Q:", "¥" }
                });
            await foreach(var output in outputs)
            {
                _logger.LogInformation(output);
                yield return output;
            }
        }
    }
}
