
#pragma warning disable SKEXP0001, SKEXP0010, SKEXP0050
using Hackathon_Neworbit.Persona;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.KernelMemory;
using System.Reflection;
using Hackathon_Neworbit;

internal class Program
{
    const string deploymentChatName = "pp-hack-gtp4o";
    const string deploymentEmbeddingName = "pp-test-embedding";
    const string endpoint = "https://pp-azure-open-ai-test.openai.azure.com/";

    const string searchEndpoint = "https://pp-azure-open-ai-test.openai.azure.com/";

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
            deploymentChatName,
            endpoint,
            aiKey ?? "",
            modelId: "gpt-4o");
        var kernel = kernelBuilder.Build();
        kernel.ImportPluginFromType<NativeFunctions>();

        var chat = kernel.GetRequiredService<IChatCompletionService>();

        var embeddingConfig = new AzureOpenAIConfig
        {
            APIKey = aiKey,
            Deployment = deploymentEmbeddingName,
            Endpoint = endpoint,
            APIType = AzureOpenAIConfig.APITypes.EmbeddingGeneration,
            Auth = AzureOpenAIConfig.AuthTypes.APIKey
        };

        var chatConfig = new AzureOpenAIConfig
        {
            APIKey = aiKey,
            Deployment = deploymentChatName,
            Endpoint = endpoint,
            APIType = AzureOpenAIConfig.APITypes.ChatCompletion,
            Auth = AzureOpenAIConfig.AuthTypes.APIKey
        };

        var chatHistory = ChatbotPersona.SetupChatbotPersona();

        var kernelMemory = new KernelMemoryBuilder()
            .WithAzureOpenAITextGeneration(chatConfig)
            .WithAzureOpenAITextEmbeddingGeneration(embeddingConfig)
            .Build<MemoryServerless>();

        var dir = Environment.CurrentDirectory + "\\Troubleshooting-Guide.pptx";
        await kernelMemory.ImportDocumentAsync(dir);

        var memoryPlugin = new MemoryPlugin(kernelMemory, waitForIngestionToComplete: true);
        kernel.ImportPluginFromObject(memoryPlugin);

        OpenAIPromptExecutionSettings settings = new()
        {
            ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
        };

        while (true) 
        { 
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("User: ");
            var userMessage = Console.ReadLine();

            chatHistory.AddUserMessage(userMessage!);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("AI: ");

            var response = await chat.GetChatMessageContentAsync(chatHistory, settings, kernel);
            Console.WriteLine(response);

            chatHistory.AddMessage(AuthorRole.Assistant, response.Content!);

            Console.WriteLine();
        }
    }
}

// You can create a kernal and pass it to `GetChatMessageContentsAsync`, you can add plugins to this kernal to control the behavior of the AI 
// https://learn.microsoft.com/en-us/semantic-kernel/get-started/quick-start-guide?pivots=programming-language-csharp

// detailed samples
// https://learn.microsoft.com/en-us/semantic-kernel/get-started/detailed-samples?pivots=programming-language-csharp

