using Club97.Bot.Infrastructure.Persistence.Entities;
using System.Linq.Expressions;

namespace Club97.Bot.Infrastructure.Persistence.Specifications;

internal class FindUserByIdSpecification : ISpecification<User>
{
    public Expression<Func<User, bool>> Expression { get; }

    public FindUserByIdSpecification(long userId)
    {
        Expression = user => user.Id == userId;
    }
}
