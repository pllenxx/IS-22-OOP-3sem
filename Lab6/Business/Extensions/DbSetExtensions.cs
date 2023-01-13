using Business.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Business.Extensions;

public static class DbSetExtensions
{
    public static async Task<T> GetEntityAsync<T>(
        this DbSet<T> set,
        Guid id, CancellationToken cancellationToken)
    where T : class
    {
        var entity = await set.FindAsync(new object[] { id }, cancellationToken);

        if (entity is null)
            throw new MessageProcessingSystemException($"Object {id} was not found");
        
        return entity;
    }

}