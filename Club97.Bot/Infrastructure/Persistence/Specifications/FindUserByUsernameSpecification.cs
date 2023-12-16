using Club97.Bot.Infrastructure.Persistence.Entities;
using System.Linq.Expressions;

namespace Club97.Bot.Infrastructure.Persistence.Specifications;

internal class FindUserByUsernameSpecification : ISpecification<User>
{
    public Expression<Func<User, bool>> Expression { get; }

    public FindUserByUsernameSpecification(string username)
    {
        Expression = user => user.Username == username;
    }  
}
 