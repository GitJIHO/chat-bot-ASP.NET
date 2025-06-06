using InterviewAssistant.ApiService.Delegates;
using InterviewAssistant.Common.Models;

namespace InterviewAssistant.ApiService.Endpoints;

/// <summary>
/// This represents the chat completion endpoint for the API service.
/// </summary>
public static class ChatCompletionEndpoint
{
    /// <summary>
    /// Maps the chat completion endpoint to the specified route builder.
    /// </summary>
    /// <param name="routeBuilder"><see cref="IEndpointRouteBuilder"/> instance.</param>
    /// <returns>Returns the <see cref="IEndpointRouteBuilder"/> instance.</returns>
    public static IEndpointRouteBuilder MapChatCompletionEndpoint(this IEndpointRouteBuilder routeBuilder)
    {
        // Chat Completion API 그룹
        var api = routeBuilder.MapGroup("api/chat").WithTags("Chat");

        // 채팅 메시지 전송 엔드포인트
        api.MapPost("complete", ChatCompletionDelegate.PostChatCompletionAsync)
           .Accepts<ChatRequest>(contentType: "application/json")
           .Produces<IEnumerable<ChatResponse>>(statusCode: StatusCodes.Status200OK, contentType: "application/json")
           .WithName("PostChatCompletion")
           .WithOpenApi();

        return routeBuilder;
    }
}
