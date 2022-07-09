using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

namespace Bunkograph.DAL
{
    public static class EntityFrameworkExtensionMethods
    {
        public static async Task<TSource?> FirstOrDefaultServerAndLocalAsync<TSource>(this DbSet<TSource> source,
            Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
            where TSource : class
        {
            return await source.FirstOrDefaultAsync(predicate, cancellationToken)
                ?? source.Local.FirstOrDefault(predicate.Compile());
        }
    }
}
