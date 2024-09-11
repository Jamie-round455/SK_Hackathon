using Microsoft.SemanticKernel.ChatCompletion;
using System.Text.Json;

namespace Hackathon_Neworbit.Persona;

internal class ChatbotPersona
{
    public static ChatHistory SetupChatbotPersona()
    {
        var lines = GetPersonaLines();
        return new ChatHistory(lines);
    }

    private static string GetPersonaLines()
    {
        var personaConfigurationFile = Path.Combine(Directory.GetCurrentDirectory(), "persona.json");

        var fileContent = File.ReadAllText(personaConfigurationFile);
        var lines = JsonSerializer.Deserialize<IEnumerable<string>>(fileContent);

        return string.Join(". ", lines!);
    }
}
