using Club97.Bot.Commands;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Club97.Bot.Common;

internal class CommandExecutor
{
    private StartCommand startCommand;
    private Command? rootCommand;
    private readonly Services services;

    public CommandExecutor(Services services)
    {
        startCommand = new StartCommand();
        this.services = services;
    }

    public async Task GetUpdate(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
    {
        // сделать аунтификацию
        var messageText = update.Message!.Text!;
        if (messageText == startCommand.Name)
        {
            await startCommand.Execute(client, update, cancellationToken);
            rootCommand = new HomeCommand(services);
        }

        if (rootCommand is not null)
            await rootCommand.GetCurrent().Execute(client, update, cancellationToken);
    }
}
