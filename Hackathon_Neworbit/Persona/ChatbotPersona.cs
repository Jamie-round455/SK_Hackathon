using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace Hackathon_Neworbit.Persona;

internal class ChatbotPersona
{
    public static ChatHistory SetupChatbotPersona()
    {
        var lines = GetPersonaLines();
        return new ChatHistory(lines);
    }

    private static IEnumerable<ChatMessageContent> GetPersonaLines()
    {
        var personaConfigurationFile = Path.Combine(Directory.GetCurrentDirectory(), "persona.json");

        var lines = File.ReadAllLines(personaConfigurationFile);

        foreach (var line in lines)
            yield return new ChatMessageContent(AuthorRole.System, line);
    }
}
