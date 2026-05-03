using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace Infrastructure.Repos
{
    public class GenericRepo<T> : IGenericRepository<T> where T : class

    {
        protected readonly Context _context;
        public GenericRepo(Context context)
        {
            _context = context;
        }
        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Remove(T Entity)
        {
            _context.Set<T>().Remove(Entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>> filter = null,
            string properties = "")
        {

            IQueryable<T> query = _context.Set<T>();
            if (filter != null)
            {
                query = query.Where(filter);

            }
            if (!string.IsNullOrEmpty(properties))
            {
                string[] values = properties.Split(',');
                foreach (var prop in values)
                {
                    query = query.Include(prop); query.Include(prop);
                }
            }
            return await query.AsNoTracking().ToListAsync();
        }
        //public async Task<IEnumerable<T>> GetAllReadOnlyAsync() { };

        public async Task<T?> GetByIdAsync(int id, bool tracked = true)
        {
            IQueryable<T> query = _context.Set<T>();

            if (!tracked)
            {
                query.AsNoTracking();
            }
            return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return;
        }
        public async Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, bool tracked = true,string? includeProperties = null)
        {
            IQueryable<T> query = _context.Set<T>();
            
            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            if (includeProperties != null)
            {

                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }

            }
            if (predicate != null)
            {
               return await query.FirstOrDefaultAsync(predicate);
            }
            return await query.FirstOrDefaultAsync();
            }
        public async Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }
    }
}
