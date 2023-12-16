using Club97.Bot.Common;
using Club97.Bot.Infrastructure.Persistence.Specifications;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Club97.Bot.Commands.Transfer;

internal class TransferCommand : Command, INamedCommand
{
    public string Name => "Перевод";

    private IAsyncEnumerator<Task>? execution;
    private Transfer transfer = null!;

    public TransferCommand(Services services, Func<Task> onCommandEndHandler)
        : base(services, onCommandEndHandler)
    {
        commands = new()
        {
            new BackCommand(EndCommand)
        };
    }

    public override async Task Execute(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
    {
        await base.Execute(client, update, cancellationToken);

        if (await MapAndExecuteCommand())
            return;

        if (execution is null)
            execution = ExecuteByStep();

        if (await execution.MoveNextAsync())
        {
            await execution.Current;
            return;
        }

        var senderBalance = transfer.Sender.Balance - transfer.Amount;
        var receiverBalance = transfer.Receiver.Balance + transfer.Amount;

        await services.UsersRepository.UpdateBalance(transfer.Sender.Id, senderBalance);
        await services.UsersRepository.UpdateBalance(transfer.Receiver.Id, receiverBalance);

        await SendTextMessage(
            $"Перевод {transfer.Receiver.Username} на сумму {transfer.Amount} клубенсов выполнен!\n" +
            $"Баланс: {senderBalance} клубенсов");

        await client.SendTextMessageAsync(
            transfer.Receiver.ChatId,
            $"Получен перевод на сумму {transfer.Amount} клубенсов от {transfer.Sender.Username}!\n" +
            $"Баланс: {receiverBalance} клубенсов");

        await EndCommand();
    }

    private async IAsyncEnumerator<Task> ExecuteByStep()
    {
        var senderFilter = new FindUserByIdSpecification(context.UserId);
        var sender = (await services.UsersRepository.GetUser(senderFilter))!;

        transfer = new Transfer
        {
            Sender = sender
        };

        yield return SendTextMessage("Введите имя:");

        while (true)
        {
            var receiverUsername = context.Update.Message!.Text!;
            var filter = new FindUserByUsernameSpecification(receiverUsername);

            var receiver = await services.UsersRepository.GetUser(filter);

            if (receiver is null)
            {
                yield return SendTextMessage("Пользователь не найден, введите еще раз:");
                continue;
            }

            if (!TransferValidator.IsReceiverNotEqualToSender(receiver.Id, transfer.Sender.Id))
            {
                yield return SendTextMessage("Некорректный ввод");
                continue;
            }

            transfer.Receiver = receiver;
            break;
        }

        yield return SendTextMessage("Введите сумму перевода:");

        while (true)
        {
            var transferAmountSource = context.Update.Message!.Text!;

            if (TransferParser.ParseTransferAmount(transferAmountSource) is not { } transferAmount)
            {
                yield return SendTextMessage("Некорректный ввод");
                continue;
            }

            if (!TransferValidator.IsTransferAmountGreaterThanZero(transferAmount))
            {
                yield return SendTextMessage("Сумма перевода должна быть больше 0");
                continue;
            }

            transfer.Amount = transferAmount;

            if (!TransferValidator.IsFundsEnough(transfer.Sender.Balance, transferAmount))
            {
                yield return SendTextMessage("Не достаточно средств");
                continue;
            }

            break;
        }
    }

    protected override void OnCommandEnd()
    {
        execution = null;
    }

    protected override IReplyMarkup GetReplyMarkup()
    {
        return new ReplyKeyboardMarkup(new KeyboardButton[]
        {
            new KeyboardButton("Назад")
        })
        {
            ResizeKeyboard = true
        };
    }
}
