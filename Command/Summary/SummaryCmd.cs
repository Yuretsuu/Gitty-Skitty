using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using GittySkitty.Authentication;
using Octokit;

namespace GittySkitty.Command
{
    public class SummaryCmd
    {
        private readonly DiscordSocketClient _discord;
        private readonly GitHubClient _gitHubClient;

        public SummaryCmd(DiscordSocketClient discord)
        {
            _discord = discord;
            var authentication = new Authenticate();
            _gitHubClient = authentication.GetGitHubClient();

            _discord.InteractionCreated += OnInteractionCreatedAsync;
        }

        private async Task OnInteractionCreatedAsync(SocketInteraction interaction)
        {
            if (interaction is not SocketSlashCommand slashCommand) return;

            if (slashCommand.Data.Name == "summary")
            {
                Console.WriteLine("Received Summary command.");  // Debugging statement

                var repositories = await _gitHubClient.Repository.GetAllForCurrent();
                var repoListMessage = string.Join("\n", repositories.Select((repo, index) => $"{index + 1}. {repo.FullName}"));

                var embedBuilder = new EmbedBuilder
                {
                    Title = "List of Repositories",
                    Description = repoListMessage,
                    Color = new Discord.Color(250, 165, 244),
                    Timestamp = DateTimeOffset.Now,
                    ThumbnailUrl = "https://i.pinimg.com/564x/c3/d2/a2/c3d2a210e6b95cd95b953da272152d94.jpg"
                };

                var embed = embedBuilder.Build();
                await interaction.RespondAsync(embed: embed);
            }
        }
    }
}