using Discord;
using Discord.Net;
using Discord.WebSocket;
using GittySkitty.Command;
using GittySkitty.Log;

public class CreateCmd
{
    public static async Task CreateAndRegisterSlashCommand(SocketGuild guild, ErrorLog errorLog)
    {
        var summaryObject = new SummaryObject();  // Assumes CommandName and CommandDesc are hardcoded in SummaryObject constructor

        var guildCommand = new SlashCommandBuilder();

        guildCommand.WithName(summaryObject.CommandName);
        guildCommand.WithDescription(summaryObject.CommandDesc);

        try
        {
            await guild.CreateApplicationCommandAsync(guildCommand.Build());
            Console.WriteLine($"Successfully created guild command: {summaryObject.CommandName}");

        }
        catch(ApplicationCommandException exception)
        {
            await errorLog.LogErrorAsync(exception);
            Console.WriteLine($"Failed to create guild command: {summaryObject.CommandName}");
            await CreateAndRegisterSlashCommand(guild, errorLog);  // Recursive call to retry command registration
        }
    }
}