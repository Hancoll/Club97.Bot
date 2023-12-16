using Club97.Bot.Common;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Club97.Bot.Commands;

internal class BackCommand : Command, INamedCommand
{
    public string Name => "Назад";

    private readonly Func<Task> endParentCommand;

    public BackCommand(Func<Task> endParentCommand)
    {
        this.endParentCommand = endParentCommand;
    }
        
    public override async Task Execute(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
    {
        await endParentCommand();
    }
}
