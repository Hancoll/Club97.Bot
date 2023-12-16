using Club97.Bot.Infrastructure.Persistence;
using Club97.Bot.Infrastructure.Persistence.Specifications;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Club97.Bot.Common;

internal class Distributor
{
    private readonly Dictionary<long, CommandExecutor> listeners = new();

    public async Task GetUpdate(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
    {
        if (update.Message is not { } message)
            return;

        if (message.From is not { } from)
            return;

        if (from.Username is not { } username)
            return;

        if (message.Text is null)
            return;

        var chatId = message.Chat.Id;
        var listener = listeners.GetValueOrDefault(chatId);

        // TODO: move to startcommand and update chatid each startcommand execution
        if (listener is null)
        {
            var applicationContext = new ApplicationContext();
            var usersRepository = new UsersRepostitory(applicationContext);

            var userId = message.From.Id;
            var filter = new FindUserByIdSpecification(userId);

            var user = await usersRepository.GetUser(filter);

            if (user is null)
                await usersRepository.CreateUser(userId, username, chatId);

            var context = new Services
            {
                UsersRepository = usersRepository
            };

            listener = new(context);
            listeners.Add(chatId, listener);
        }

        await listener.GetUpdate(client, update, cancellationToken);
    }
}
