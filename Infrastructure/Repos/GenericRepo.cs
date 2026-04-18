using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Infrastructure.Repos
{
    public class GenericRepo<T> : IGenericRepository<T> where T : class

    {
        protected  readonly Context _context;
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
            Expression<Func<T, bool>> filter = null ,
            string properties="")
        {
            
            IQueryable<T> query = _context.Set<T>();
            if (filter != null) {
                query = query.Where(filter);

            }
            if (!string.IsNullOrEmpty(properties)) {
                string[] values = properties.Split(',');
                foreach (var prop in values)
                {
                    query.Include(prop);
                }
            }
            return await query.AsNoTracking().ToListAsync();
        }   

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
            
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
        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {

            return await _context.Set<T>().FirstOrDefaultAsync(predicate);
        }
    }
}
