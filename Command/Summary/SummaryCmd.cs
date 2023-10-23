using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Octokit;

namespace GittySkitty.Command
{
    public class SummaryCmd : ICommand
    {
        private readonly DiscordSocketClient _discord;
        private readonly GitHubClient _gitHubClient;
        private List<Repository> _repositories;
        public SummaryCmd(DiscordSocketClient discord, Authenticate authenticate)
        {
            _discord = discord;
            _gitHubClient = authenticate.GitHubClient;
            
        }

        public string GetCommandName()
        {
            throw new NotImplementedException();
        }

        public string GetCommandDescription()
        {
            throw new NotImplementedException();
        }

        public Task CreateAndRegister(SocketGuild guild)
        {
            throw new NotImplementedException();
        }

        public async Task SendSummaryCommandResponseAsync(SocketSlashCommand command)
        {
            Console.WriteLine("Received Summary command.");
            
            _repositories = (await _gitHubClient.Repository.GetAllForCurrent()).ToList();
            var embed = BuildRepositoryEmbed(_repositories);
            await command.RespondAsync(embed: embed);
            
            _discord.MessageReceived += TemporaryEventHandler;
        }
        
        private async Task TemporaryEventHandler(SocketMessage message)
        {
            // Debug line to log the received message
            Console.WriteLine($"Received message: {message.Content}");

            // Do your check here. Is this the message you're waiting for? If yes, proceed.
            if (int.TryParse(message.Content, out int selection))
            {
                await ProcessSelectionAsync(selection, message.Channel);

                // Important: Unsubscribe the temporary event handler so it doesn't keep listening
                _discord.MessageReceived -= TemporaryEventHandler;
            }
        }
        
        private Embed BuildRepositoryEmbed(List<Repository> repositories)
        {
            var repoListMessage = string.Join("\n", repositories.Select((repo, index) => $"{index + 1}. {repo.FullName}"));
            var embedBuilder = new EmbedBuilder
            {
                Title = "Choose a Repository!",
                Description = repoListMessage,
                Color = new Discord.Color(250, 165, 244),
                Timestamp = DateTimeOffset.Now,
                ThumbnailUrl = "https://i.pinimg.com/564x/65/f8/f6/65f8f6968ee9fd837a0ffdfea7449dfd.jpg"
            };
            return embedBuilder.Build();
        }
        public async Task ProcessSelectionAsync(int selection, ISocketMessageChannel channel)
         {
             try
             {
                 Console.WriteLine("Processing selection.");
                 var repository = _repositories[selection - 1];
                 var details = $"{repository.FullName}\n{repository.Description}\n{repository.HtmlUrl}";
                 await channel.SendMessageAsync(details);
             }
             catch (Exception ex)
             {
                 Console.WriteLine($"Error processing selection: {ex.Message}");
             }
         }
    }
}
