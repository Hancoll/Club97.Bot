using Club97.Bot.Infrastructure.Persistence.Entities;

namespace Club97.Bot.Commands.Transfer;

internal static class TransferValidator
{
    public static bool IsFundsEnough(int receiverBalance, int transferAmount)
    {
        return receiverBalance - transferAmount >= 0;
    }

    public static bool IsReceiverNotEqualToSender(long receiverId, long senderId)
    {
        return receiverId != senderId;
    }

    public static bool IsTransferAmountGreaterThanZero(int transferAmount)
    {
        return transferAmount > 0;
    }
}
