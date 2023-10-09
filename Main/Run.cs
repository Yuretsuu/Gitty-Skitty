using System.Threading.Tasks;
using Discord.WebSocket;
using Discord;
using GittySkitty.Command;
using GittySkitty.Authentication;
using GittySkitty.Log;

namespace GittySkitty.Main
{
    public class Run
    {
        // Declaring private readonly fields for holding instances of various necessary classes
        private readonly DiscordSocketClient _discord;
        private readonly ErrorLog _errorLog;
        private readonly Authenticate _authenticate;
        private readonly SummaryCmd _summaryCmd;  // Field for SummaryCmd instance

        // Constructor: Initializes the fields with new instances of the classes
        public Run()
        {
            _discord = new DiscordSocketClient();
            _errorLog = new ErrorLog();
            _authenticate = new Authenticate();
            _summaryCmd = new SummaryCmd(_discord);  // Creates SummaryCmd instance, passing DiscordSocketClient instance
        }

        // Entry point of the program
        public static Task Main(string[] args) => new Run().StartAsync();

        // Asynchronous method to start the bot
        public async Task StartAsync()
        {
            // Getting Discord token from Authenticate instance and logging into Discord
            string discordToken = _authenticate.GetDiscordToken();
            await _discord.LoginAsync(TokenType.Bot, discordToken);
            await _discord.StartAsync();

            // Subscribing to Discord client events
            _discord.Log += ErrorLog.LogAsync;  // Subscribing to Log event
            _discord.Ready += OnReadyAsync;     // Subscribing to Ready event

            // Keeping the program running indefinitely
            await Task.Delay(-1);
        }

        // Event handler method for when the Discord client is ready
        private async Task OnReadyAsync()
        {
            // Looping through each guild the bot is connected to and registering the slash command
            foreach (var guild in _discord.Guilds)
            {
                // Calling CreateAndRegisterSlashCommand method from CreateCmd class to register the command in each guild
                await CreateCmd.CreateAndRegisterSlashCommand(guild as SocketGuild, _errorLog);
            }
        }
    }
}
