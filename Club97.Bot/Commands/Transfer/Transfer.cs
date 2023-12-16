using Club97.Bot.Infrastructure.Persistence.Entities;

namespace Club97.Bot.Commands.Transfer;

internal class Transfer
{
    public User Sender { get; set; } = null!;

    public User Receiver { get; set; } = null!;

    public int Amount { get; set; }
}
