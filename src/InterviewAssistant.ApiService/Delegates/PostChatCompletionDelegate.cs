using InterviewAssistant.Common.Models;
using InterviewAssistant.ApiService.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel.ChatCompletion;

using ChatMessageContent = Microsoft.SemanticKernel.ChatMessageContent;

namespace InterviewAssistant.ApiService.Delegates;

/// <summary>
/// This represents the partial delegate entity that takes care of the chat completion endpoint.
/// </summary>
public static partial class ChatCompletionDelegate
{
    /// <summary>
    /// Invokes the chat completion endpoint.
    /// </summary>
    /// <param name="req"><see cref="ChatRequest"/> instance as a request payload.</param>
    /// <returns>Returns an asynchronous stream of <see cref="ChatResponse"/>.</returns>
    public static async IAsyncEnumerable<ChatResponse> PostChatCompletionAsync(
        [FromBody] ChatRequest req,
        IKernelService kernelService)
    {
        var messages = new List<ChatMessageContent> { };

        foreach (var msg in req.Messages)
        {
            ChatMessageContent message = msg.Role switch
            {
                MessageRoleType.User => new ChatMessageContent(AuthorRole.User, msg.Message),
                MessageRoleType.Assistant => new ChatMessageContent(AuthorRole.Assistant, msg.Message),
                MessageRoleType.System => new ChatMessageContent(AuthorRole.System, msg.Message),
                MessageRoleType.Tool => new ChatMessageContent(AuthorRole.Tool, msg.Message),
                _ => throw new ArgumentException($"Invalid role: {msg.Role}")
            };
            messages.Add(message);
        }

        await foreach (var text in kernelService.InvokeChatAsync(messages))
        {
            yield return new ChatResponse { Message = text };
        }
    }
}
