using System.Linq.Expressions;

namespace Club97.Bot.Infrastructure.Persistence.Specifications;

internal interface ISpecification<T>
{
    Expression<Func<T, bool>> Expression { get; }
}
