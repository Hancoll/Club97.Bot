using Club97.Bot.Commands;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Club97.Bot.Common;

internal abstract class Command
{
    protected Command? currentCommand;
    protected Services services;
    protected ExecutionContext context = null!;
    protected Func<Task> onCommandEndHandler;

    protected List<Command> commands { get; init; } = null!;

    public Command (Services? services = null,
        Func<Task>? onCommandEndHandler = null)
    {
        this.services = services!;
        this.onCommandEndHandler = onCommandEndHandler!;
    }

    public Command GetCurrent()
    {
        if (currentCommand is null)
            return this;

        return currentCommand.GetCurrent();
    }

#pragma warning disable CS1998

    public virtual async Task Execute(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
    {
        context = new(client, update, update.Message!.From!.Id, cancellationToken);
    }   

#pragma warning restore

    protected async Task<bool> MapAndExecuteCommand()
    {
        var messageText = context.Update.Message!.Text!;

        if (MapCommand(messageText) is { } command)
        {
            if (command is not BackCommand)
                currentCommand = command;

            await command.Execute(context.Client, context.Update, context.CancellationToken);
            return true;
        }

        return false;
    }

    protected Command? MapCommand(string commandName)
    {
        foreach (var command in commands)
        {
            if (command is not INamedCommand namedCommand)
                continue;

            if (namedCommand.Name == commandName)
                return command;
        }

        return null;
    }

    protected virtual async Task OnChildCommandEndHandler()
    {
        currentCommand = null;
        await DefaultAction();
    }

    protected async Task EndCommand()
    {
        OnCommandEnd();
        await onCommandEndHandler?.Invoke()!;
    }

    protected virtual void OnCommandEnd() { }

    protected virtual Task DefaultAction()
        => throw new NotImplementedException();

    protected async Task SendTextMessage(string messageText)
    {
        await context.Client.SendTextMessageAsync(
            context.Update.Message!.Chat.Id,
            messageText,
            replyMarkup: GetReplyMarkup(),
            cancellationToken: context.CancellationToken);
    }

    protected virtual IReplyMarkup GetReplyMarkup()
    {
        return new ReplyKeyboardRemove();
    }
}
