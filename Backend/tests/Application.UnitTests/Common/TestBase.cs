using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq.Expressions;

namespace Afama.Go.Api.Application.UnitTests.Common;

public abstract class TestBase
{
    protected static Mock<DbSet<T>> CreateMockDbSet<T>(List<T> data) where T : class
    {
        var mockSet = new Mock<DbSet<T>>();
        var queryable = data.AsQueryable();

        mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
        mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
        mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

        // Mock async methods for Entity Framework
        mockSet.Setup(x => x.FindAsync(It.IsAny<object[]>()))
               .Returns<object[]>(keyValues => 
               {
                   var entity = data.FirstOrDefault();
                   return new ValueTask<T?>(entity);
               });

        // Mock FirstOrDefaultAsync with predicate
        mockSet.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<T, bool>>>(), It.IsAny<CancellationToken>()))
               .Returns<Expression<Func<T, bool>>, CancellationToken>((predicate, ct) =>
               {
                   var compiledPredicate = predicate.Compile();
                   var result = data.FirstOrDefault(compiledPredicate);
                   return Task.FromResult(result);
               });

        // Mock ToListAsync
        mockSet.Setup(x => x.ToListAsync(It.IsAny<CancellationToken>()))
               .Returns<CancellationToken>(ct => Task.FromResult(data));

        return mockSet;
    }
}
