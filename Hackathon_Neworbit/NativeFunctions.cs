using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace Hackathon_Neworbit;

public class NativeFunctions
{
    [KernelFunction]
    [Description("Handle the errors and any other problems with generating the response")]
    public static void ErrorOrUnableToGenerateTheResponse()
    {
        var previousColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("I don't know");
        Console.ForegroundColor = previousColor;
    }

    //[KernelFunction]
    //public static string GetCurrentTime()
    //{
    //    return DateTime.UtcNow.ToString();
    //}
}