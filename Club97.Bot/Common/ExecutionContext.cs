using Telegram.Bot.Types;
using Telegram.Bot;

namespace Club97.Bot.Common;

record ExecutionContext(
    ITelegramBotClient Client,
    Update Update,
    long UserId,
    CancellationToken CancellationToken);
