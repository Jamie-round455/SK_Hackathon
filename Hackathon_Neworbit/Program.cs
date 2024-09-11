using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = Host.CreateEmptyApplicationBuilder(new HostApplicationBuilderSettings()
        {
            Args = args
        });

        //builder.ConfigureAppConfiguration((context, config) =>
        //    {
        //        config
        //            .AddUserSecrets(Assembly.GetExecutingAssembly());
        //    });

        // don't forget to add your api key / endpoint / deployment name/and model id ( deployments found here: https://oai.azure.com/ )
        builder.Services
            //.AddAzureOpenAIAudioToText()
            //.AddAzureOpenAITextEmbeddingGeneration()
            //.AddAzureOpenAITextToImage()
            .AddAzureOpenAIChatCompletion(
                "pp-hack-gtp4o",
                "https://pp-azure-open-ai-test.openai.azure.com/",
                "",
                modelId: "gpt-4o");

        var app = builder.Build();

        var chat = app.Services.GetRequiredService<IChatCompletionService>();

        var chatHistory = new ChatHistory();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("AI: What am I?");
        Console.ForegroundColor = ConsoleColor.Yellow;

        Console.WriteLine("Jay: ");
        var whatAmI = Console.ReadLine();
        chatHistory.AddSystemMessage(whatAmI!);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("AI: Cool! How can I help?");
        Console.ForegroundColor = ConsoleColor.Yellow;

        Console.WriteLine("Jay: ");
        var prompt = Console.ReadLine();
        chatHistory.AddUserMessage(prompt!);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("AI: ");
        var response = await chat.GetChatMessageContentsAsync(chatHistory);
        var lastMessage = response.Last();
        Console.WriteLine(lastMessage);
    }
}

// You can create a kernal and pass it to `GetChatMessageContentsAsync`, you can add plugins to this kernal to control the behavior of the AI 
// https://learn.microsoft.com/en-us/semantic-kernel/get-started/quick-start-guide?pivots=programming-language-csharp

// detailed samples
// https://learn.microsoft.com/en-us/semantic-kernel/get-started/detailed-samples?pivots=programming-language-csharp


