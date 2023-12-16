using MongoDB.Bson.Serialization.Attributes;

namespace Club97.Bot.Infrastructure.Persistence.Entities;

internal class User
{
    [BsonId]
    public long Id { get; set; }

    public string Username { get; set; } = null!;

    public long ChatId { get; set; }

    public int Balance { get; set; }
}
