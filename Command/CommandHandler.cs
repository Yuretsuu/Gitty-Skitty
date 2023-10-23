using Discord.WebSocket;
using GittySkitty.Authentication;
using GittySkitty.Command;

public class CommandHandler
{
    private readonly Authenticate _authenticate;
    private readonly DiscordSocketClient _client;
    private readonly SummaryCmd _summaryCmd;

    public CommandHandler(Authenticate authenticate, DiscordSocketClient client, SummaryCmd summaryCmd)
    {
        _authenticate = authenticate;
        _client = client;
        _summaryCmd = summaryCmd; // Injected Singleton
    }

    public void Initialize(DiscordSocketClient client)
    {
        client.InteractionCreated += HandleInteractionAsync;
    }

    private async Task HandleInteractionAsync(SocketInteraction interaction)
    {
        if (interaction is SocketSlashCommand command)
        {
            switch (command.Data.Name)
            {
                case "summary":
                    await _summaryCmd.SendSummaryCommandResponseAsync(command);
                    break;
            }
        }
    }
}