using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Graphql
{
    public class CombustionGraphStore : IGraphStore<Combustion>
    {

        private readonly ApplicationDbContext _context;

        public CombustionGraphStore(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Combustion> CreateAsync(Combustion model)
        {
            var obj = _context.Combustions.Add(model);
            await _context.SaveChangesAsync();
            return obj.Entity;
        }

        public Task<ILookup<int?, Combustion>> GetAllAsync(IEnumerable<int?> ids, string tipo)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IDictionary<int?, Combustion>> GetUsersByIdAsync(IEnumerable<int?> ids, CancellationToken cancellationToken)
        {
            var combustions = await _context.Combustions.Where(x => ids.Contains(x.Id)).ToListAsync();
            var dict = new Dictionary<int?, Combustion>();

            foreach (var id in ids)
            {
                dict.Add(id, combustions.Single(x => x.Id == id));
            }

            return dict;
        }

        public async Task<Combustion> UpdateAsync(int id, Combustion model)
        {
            var brandDb = _context.Combustions.Find(id);
            if (brandDb != null)
            {
                brandDb.Name = model.Name;
                await _context.SaveChangesAsync();
            }


            return brandDb;
        }
    }
}