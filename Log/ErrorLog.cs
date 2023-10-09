using Discord;
using Discord.Commands;

namespace GittySkitty.Log;

//This class handles error logs in the terminal
public class ErrorLog
{
    public static Task LogAsync(LogMessage log)
    {
        Console.WriteLine(log);
        return Task.CompletedTask;
    }
    
    public Task LogErrorAsync(Exception exception)
    {
        Console.WriteLine($"Error: {exception.Message}");
        return Task.CompletedTask;
    }
}