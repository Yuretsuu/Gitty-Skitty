using Discord;
using Discord.Net;
using Discord.WebSocket;
using GittySkitty.Command;

//This file handles code to create commands. 
public class CreateCmd
{
    public static async Task CreateAndRegisterSlashCommand(SocketGuild guild)
    {
        var summaryObject = new SummaryObject(); 
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
            
            Console.WriteLine($"Failed to create guild command: {summaryObject.CommandName}");
            await CreateAndRegisterSlashCommand(guild);  // Recursive call to retry command registration
        }
    }
}