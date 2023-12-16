using Club97.Bot.Infrastructure.Persistence.Entities;
using MongoDB.Driver;

namespace Club97.Bot.Infrastructure.Persistence;

internal interface IApplicationContext
{
    IMongoCollection<User> Users { get; }
}
