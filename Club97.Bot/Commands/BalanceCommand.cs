using Club97.Bot.Common;
using Club97.Bot.Infrastructure.Persistence.Specifications;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Club97.Bot.Commands;

internal class BalanceCommand : Command, INamedCommand
{
    public string Name => "Баланс";

    public BalanceCommand(Services services, Func<Task> onCommandEndHandler) 
        : base(services, onCommandEndHandler) { }

    public override async Task Execute(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
    {
        await base.Execute(client, update, cancellationToken);

        var filter = new FindUserByIdSpecification(context.UserId);
        var balance = (await services.UsersRepository.GetUser(filter))!.Balance;

        await client.SendTextMessageAsync(
            update.Message!.Chat.Id,
            $"Баланс: {balance} клубенсов",
            cancellationToken: cancellationToken);

        await EndCommand();
    }
}
