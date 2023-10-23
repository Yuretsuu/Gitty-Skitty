using Discord;
using Discord.WebSocket;
using GittySkitty.Authentication;
using GittySkitty.Command;
using Microsoft.Extensions.DependencyInjection;

public class Program
{
    private readonly DiscordSocketClient _client;
    private readonly Authenticate _authenticate;
    private readonly SummaryCmd _summaryCmd;

    public Program(DiscordSocketClient client, Authenticate authenticate, SummaryCmd summaryCmd)
    {
        _client = client;
        _authenticate = authenticate;
        _summaryCmd = summaryCmd; // This is your Singleton instance
    }

    public static async Task Main(string[] args)
    {
        using (var services = ConfigureServices().BuildServiceProvider())
        {
            var program = services.GetRequiredService<Program>();

            try
            {
                string discordToken = program._authenticate.DiscordToken;
                await program._client.LoginAsync(TokenType.Bot, discordToken);
                await program._client.StartAsync();
                
                Console.WriteLine($"Bot started successfully with token: {discordToken}");

                var commandHandler = services.GetRequiredService<CommandHandler>();
                commandHandler.Initialize(program._client);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting bot: {ex.Message}");
            }

            await Task.Delay(Timeout.Infinite);
        }
    }

    private static IServiceCollection ConfigureServices()
    {
        var config = new DiscordSocketConfig
        {
            MessageCacheSize = 100,  // Cache the last 100 messages per text channel
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
        };

        return new ServiceCollection()
            .AddSingleton(new DiscordSocketClient(config))  // Pass the config here
            .AddSingleton<Authenticate>()
            .AddSingleton<SummaryCmd>()
            .AddSingleton<CommandHandler>()
            .AddSingleton<Program>();
    }


}