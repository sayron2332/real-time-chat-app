using Azure.AI.TextAnalytics;
using ChatInRealTime.Core.Dtos.Message;
using ChatInRealTime.Core.Entites;
using ChatInRealTime.Core.Interfaces;
using Microsoft.AspNetCore.SignalR;


public class ChatHub(IChatService chatService, TextAnalyticsClient textAnalyticsClient) : Hub
{
    private readonly IChatService _chatService = chatService;
    private readonly TextAnalyticsClient _textAnalyticsClient = textAnalyticsClient;
    public async Task SendMessage(string user, string message)
    {

        DocumentSentiment sentimentResult = await 
            _textAnalyticsClient.AnalyzeSentimentAsync(message);
      
        Sentiment sentiment = sentimentResult.Sentiment switch
        {
            TextSentiment.Positive => Sentiment.Positive,
            TextSentiment.Negative => Sentiment.Negative,
            _ => Sentiment.Neutral
        };

        var messageDto = new MessageDto
        {
            Text = message,
            UserName = user,
            Timestamp = DateTime.UtcNow,
            Sentiment = sentiment
        };

        await _chatService.SaveMessage(messageDto);
        await Clients.All.SendAsync("ReceiveMessage", messageDto);
    }
    public async Task GetMessageHistory()
    {
        var messages = await _chatService.GetMessages(50);
        foreach (var message in messages)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}