using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Graphql
{
    public class BrandGraphStore : IGraphStore<Brand>
    {
        private readonly ApplicationDbContext _context;

        public BrandGraphStore(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Brand> CreateAsync(Brand model)
        {
            var obj = _context.Brands.Add(model);
            await _context.SaveChangesAsync();
            return obj.Entity;
        }

        public Task<ILookup<int?, Brand>> GetAllAsync(IEnumerable<int?> ids, string tipo)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IDictionary<int?, Brand>> GetUsersByIdAsync(IEnumerable<int?> ids, CancellationToken cancellationToken)
        {
            var brands = await _context.Brands.Where(x => ids.Contains(x.Id)).ToListAsync();
            var dict = new Dictionary<int?, Brand>();

            foreach (var id in ids)
            {
                dict.Add(id, brands.Single(x => x.Id == id));
            }

            return dict;
        }

        public async Task<Brand> UpdateAsync(int id, Brand model)
        {
            var brandDb = _context.Brands.Find(id);
            if (brandDb != null)
            {
                brandDb.Name = model.Name;
                await _context.SaveChangesAsync();
            }


            return brandDb;
        }
    }
}