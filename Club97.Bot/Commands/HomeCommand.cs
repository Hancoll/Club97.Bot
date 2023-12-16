using Club97.Bot.Commands.Transfer;
using Club97.Bot.Common;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Club97.Bot.Commands;

internal class HomeCommand : Command
{
    public HomeCommand(Services services) : base(services)
    {
        commands = new()
        {
            new TransferCommand(services, OnChildCommandEndHandler),
            new BalanceCommand(services, OnChildCommandEndHandler)
        };
    }
     
    public override async Task Execute(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
    {
        await base.Execute(client, update, cancellationToken);

        if (await MapAndExecuteCommand())
            return;

        await DefaultAction();
    }

    protected override async Task DefaultAction()
    {
        await context.Client.SendTextMessageAsync(
            context.Update.Message!.Chat.Id,
            "Выбери действие:",
            replyMarkup: GetReplyMarkup(),
            cancellationToken: context.CancellationToken);
    }

    protected override IReplyMarkup GetReplyMarkup()
    {
        return new ReplyKeyboardMarkup(new KeyboardButton[]
        {
            new KeyboardButton("Баланс"),
            new KeyboardButton("Перевод")
        })
        {
            ResizeKeyboard = true
        };
    }
}
