using System.ClientModel;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenAI;
using Microsoft.Extensions.Configuration;

// basic setup of you AI app

var builder = Host.CreateApplicationBuilder();

//builder.Services.AddChatClient(new OllamaChatClient(new Uri("http://localhost:8089/"), "llama3"));

var app = builder.Build();


// - * - * - * - * - * - * - * - * - * - * - * - * - * - * - * - * - * - * - * - * - * - * - * - * 
// Add your first chat client ...

//IChatClient chatClient = app.Services.GetRequiredService<IChatClient>();

//var response = await chatClient.GetResponseAsync(new ChatMessage(ChatRole.User, "What is AI?")); // just like that
//Console.WriteLine(response.Text);




// - * - * - * - * - * - * - * - * - * - * - * - * - * - * - * - * - * - * - * - * - * - * - * - * 
// How easy it is to switch between service providers. 

var credential = new ApiKeyCredential("");
var openAiOption = new OpenAIClientOptions()
{
    Endpoint = new Uri("https://models.inference.ai.azure.com")

};
var openAiClient = new OpenAIClient(credential, openAiOption);
//var chatlient = openAiClient.AsChatClient("gpt-4o-mini");

var chatlient = openAiClient.AsEmbeddingGenerator("text-embedding-3-small");

//var response = await chatlient.GetResponseAsync(new ChatMessage(ChatRole.User, "Tell me something about AI"));
//Console.WriteLine(response.Text);




// - * - * - * - * - * - * - * - * - * - * - * - * - * - * - * - * - * - * - * - * - * - * - * - * 
// Let our chat keep going with AI agent (showcasing - ChatHistory)

var chatHistory = new List<ChatMessage>();
//while (true)
//{
//    Console.ForegroundColor = ConsoleColor.Green;
//    Console.WriteLine("-------------------------------------------");
//    Console.WriteLine("You :");
//    Console.ForegroundColor = ConsoleColor.White;
//    var input = Console.ReadLine();
//    if (string.IsNullOrEmpty(input)) break;

//    chatHistory.Add(new ChatMessage(ChatRole.User, input));

//    var response = await chatlient.GetResponseAsync(chatHistory);
//    Console.ForegroundColor = ConsoleColor.Yellow;
//    Console.WriteLine("-------------------------------------------");
//    Console.WriteLine("AI Response :");
//    Console.ForegroundColor = ConsoleColor.White;
//    Console.WriteLine(response.Text);
//}




// - * - * - * - * - * - * - * - * - * - * - * - * - * - * - * - * - * - * - * - * - * - * - * - * 
// Let's get data as streamed (showcasing - GetStreamingResponseAsync)

//while (true)
//{
//    Console.ForegroundColor = ConsoleColor.Green;
//    Console.WriteLine("");
//    Console.WriteLine("-------------------------------------------");
//    Console.WriteLine("You :");
//    Console.ForegroundColor = ConsoleColor.White;
//    var input = Console.ReadLine();
//    if (string.IsNullOrEmpty(input)) break;

//    chatHistory.Add(new ChatMessage(ChatRole.User, input));

//    var chatResponse = "";
//    Console.ForegroundColor = ConsoleColor.Yellow;
//    Console.WriteLine("-------------------------------------------");
//    Console.WriteLine("AI Response :");
//    Console.ForegroundColor = ConsoleColor.White;
//    await foreach (var i in chatlient.GetStreamingResponseAsync(chatHistory))
//    {
//        Console.Write(i.Text);
//        chatResponse += i.Text;
//    }

//    chatHistory.Add(new ChatMessage(ChatRole.Assistant, chatResponse));
//    Console.WriteLine();
//}




// This is very basic example of Chat with AI agent.
// But now let's explore more that what else it can do

// - * - * - * - * - * - * - * - * - * - * - * - * - * - * - * - * - * - * - * - * - * - * - * - * 
// It also come with feature to implement the Embeddings


Console.WriteLine("Enter text to generate embeddings:");
string inputText = Console.ReadLine();

// Generate Embedding
var embeddingResult = await chatlient.GenerateEmbeddingAsync(inputText);

if (embeddingResult != null)
{
    Console.WriteLine("✅ Embedding generated successfully!");
    Console.WriteLine($"Text: {inputText}");
    Console.WriteLine("Embedding:");
    Console.WriteLine(string.Join(", ", embeddingResult.Vector.ToArray()));
}
else
{
    Console.WriteLine("❌ Failed to generate embeddings.");
}