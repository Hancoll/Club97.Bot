using Club97.Bot.Common;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Club97.Bot.Commands;

internal class StartCommand : Command, INamedCommand
{
    public string Name => "/start";

    public override async Task Execute(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
    {
        await base.Execute(client, update, cancellationToken);

        await SendTextMessage($"Добро пожаловать, {update.Message!.From!.Username}");
    }
}
