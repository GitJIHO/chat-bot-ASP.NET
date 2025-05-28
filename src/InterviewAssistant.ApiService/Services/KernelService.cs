using System.Reflection;
using System.Text.RegularExpressions;

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace InterviewAssistant.ApiService.Services;

public interface IKernelService
{
    IAsyncEnumerable<string> InvokeChatAsync(IEnumerable<ChatMessageContent>? messages = null);
}

public class KernelService(Kernel kernel) : IKernelService
{
    public async IAsyncEnumerable<string> InvokeChatAsync(IEnumerable<ChatMessageContent>? messages = null)
    {
        var chat = kernel.GetRequiredService<IChatCompletionService>();
        var chatHistory = new ChatHistory();

        if (messages != null)
        {
            foreach (var msg in messages)
            {
                chatHistory.Add(msg);
            }
        }

        await foreach (var response in chat.GetStreamingChatMessageContentsAsync(chatHistory))
        {
            if (response != null)
            {
                yield return response.Content ?? string.Empty;
            }
        }
    }
}
