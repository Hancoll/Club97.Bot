using Club97.Bot.Infrastructure.Persistence.Entities;
using Club97.Bot.Infrastructure.Persistence.Specifications;
using MongoDB.Driver;

namespace Club97.Bot.Infrastructure.Persistence;

internal class UsersRepostitory
{
    private readonly IApplicationContext applicationContext;

    public UsersRepostitory(IApplicationContext applicationContext)
    {
        this.applicationContext = applicationContext;
    }

    public async Task<User?> GetUser(ISpecification<User> filter)
    {
        var mongoFilter = (FilterDefinition<User>)filter.Expression;
        var user = (await applicationContext.Users.FindAsync(mongoFilter)).FirstOrDefault();

        return user;
    }

    public async Task<User> CreateUser(long userId, string username, long chatId)
    {
        var user = new User
        {
            Id = userId,
            Username = username,
            ChatId = chatId,
            Balance = 0
        };

        await applicationContext.Users.InsertOneAsync(user);

        return user;
    }

    public async Task UpdateBalance(long userId, int balance)
    {
        var filter = Builders<User>.Filter.Eq(user => user.Id, userId);
        var update = Builders<User>.Update.Set(user => user.Balance, balance);

        await applicationContext.Users.FindOneAndUpdateAsync(filter, update);
    }
}
