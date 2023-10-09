using Discord;
using Discord.Commands;
using Discord.Net;
using Discord.WebSocket;
using Newtonsoft.Json;
namespace GittySkitty.Command
{
    public interface ICommand
    {
        string GetCommandName();
        string GetCommandDescription();
    }
}

