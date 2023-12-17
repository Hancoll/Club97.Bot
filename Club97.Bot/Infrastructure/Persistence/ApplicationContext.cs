using Club97.Bot.Infrastructure.Persistence.Entities;
using MongoDB.Driver;

namespace Club97.Bot.Infrastructure.Persistence;

internal class ApplicationContext : IApplicationContext
{
    private const string connectionString = "mongodb://127.0.0.1:27017";
    private const string databaseName = "Club";
    private const string usersCollectionName = "Users";

    public IMongoCollection<User> Users { get; }

    public ApplicationContext()
    {
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);

        Users = database.GetCollection<User>(usersCollectionName);
    }
}
