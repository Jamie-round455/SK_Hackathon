using Hackathon_Neworbit.Persona;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using System.Net;
using System.Reflection;
using Hackathon_Neworbit;
using Microsoft.SemanticKernel.Connectors.OpenAI;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args); // Change to CreateDefaultBuilder

        builder.ConfigureAppConfiguration((context, config) =>
        {
            config
                .AddUserSecrets(Assembly.GetExecutingAssembly());
        });

        var app = builder.Build();

        var aiKey = app.Services.GetRequiredService<IConfiguration>()["ai_key"];
        var kernelBuilder = Kernel.CreateBuilder().AddAzureOpenAIChatCompletion(
            "pp-hack-gtp4o",
            "https://pp-azure-open-ai-test.openai.azure.com/",
            aiKey ?? "",
            modelId: "gpt-4o");
        var kernel = kernelBuilder.Build();
        kernel.ImportPluginFromType<NativeFunctions>();

        var chat = kernel.GetRequiredService<IChatCompletionService>();

        var chatHistory = ChatbotPersona.SetupChatbotPersona();

        OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
        {
            ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
        };
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("User: ");
            var prompt = Console.ReadLine();
            chatHistory.AddUserMessage(prompt!);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("AI: ");

            var response = await chat.GetChatMessageContentsAsync(chatHistory, openAIPromptExecutionSettings, kernel);
            var lastMessage = response.Last();
            Console.WriteLine(lastMessage);
            Console.WriteLine();
        }
    }
}

// You can create a kernal and pass it to `GetChatMessageContentsAsync`, you can add plugins to this kernal to control the behavior of the AI 
// https://learn.microsoft.com/en-us/semantic-kernel/get-started/quick-start-guide?pivots=programming-language-csharp

// detailed samples
// https://learn.microsoft.com/en-us/semantic-kernel/get-started/detailed-samples?pivots=programming-language-csharp


