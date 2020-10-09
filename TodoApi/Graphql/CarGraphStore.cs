using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Graphql
{
    public class CarGraphStore : IGraphStore<Car>
    {
        private readonly ApplicationDbContext _context;
        public CarGraphStore(ApplicationDbContext context)
        {
            _context=context;
        }

        public Task<Car> CreateAsync(Car model)
        {
            throw new System.NotImplementedException();
        }

        public async Task<System.Linq.ILookup<int?, Car>> GetAllAsync(IEnumerable<int?> ids)
        {
            var cars = await _context.Cars.Where(x=>ids.Contains(x.BrandKey)).ToListAsync();

            return cars.ToLookup(x => x.BrandKey);
        }

        public Task<IDictionary<int?, Car>> GetUsersByIdAsync(IEnumerable<int?> ids, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<Car> UpdateAsync(int id, Car model)
        {
            throw new System.NotImplementedException();
        }
    }
}