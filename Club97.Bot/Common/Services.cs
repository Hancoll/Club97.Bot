using Club97.Bot.Infrastructure.Persistence;
using Club97.Bot.Infrastructure.Persistence.Entities;

namespace Club97.Bot.Common;

internal class Services
{
    public long UserId { get; init; }

    public UsersRepostitory UsersRepository { get; init; } = null!;
}
