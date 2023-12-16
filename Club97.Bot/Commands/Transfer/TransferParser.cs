using Club97.Bot.Infrastructure.Persistence;

namespace Club97.Bot.Commands.Transfer;

internal static class TransferParser
{
    public static int? ParseTransferAmount(string transferAmountSource)
    {
        if (int.TryParse(transferAmountSource, out var transferAmount))
            return transferAmount;

        return null;
    }
}
