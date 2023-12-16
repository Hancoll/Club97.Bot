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
        /*
        // fix
        await client.SendPhotoAsync(
            update.Message!.Chat.Id,
            InputFile.FromUri("https://стенды-калипсо.рф/upload/iblock/ed9/shk-0112-dobro-pozhalovat-2-5h0-5.jpg"),
            caption: $"{update.Message!.From!.Username}",
            cancellationToken: cancellationToken);
        */
    }
}
