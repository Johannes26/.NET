using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TodoApi.Graphql
{
    public interface IGraphStore<T> where T : class
    {
        Task<T> CreateAsync(T model);
        Task<T> UpdateAsync(int id, T model);
        Task<ILookup<int?, T>> GetAllAsync(IEnumerable<int?> ids,String tipo);
        Task<IDictionary<int?, T>> GetUsersByIdAsync(IEnumerable<int?> ids, CancellationToken cancellationToken);
    }
}