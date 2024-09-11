using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace Hackathon_Neworbit;

public class NativeFunctions
{
    [KernelFunction]
    [Description("Handles the errors, unrecogizable prompts and any other issue with generating the response, should also be invoked when the response cannot be generated")]
    public static void ErrorOrUnableToGenerateTheResponse()
    {
        var previousColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("There was a problem with your request, I'll reach out for help to the human :)");
        Console.ForegroundColor = previousColor;
    }

    [KernelFunction]
    [Description("Handle the issue with sound and the microphone")]
    public static void VolumeUpAndUnmute()
    {
        var previousColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Volume up and unmute");
        Console.ForegroundColor = previousColor;
    }
}