using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        //Add
        public void Add(T entity);

        //Delete
        public void Remove(T Entity);

        //Update
        public void Update(T entity);

        //SearchById
        public Task<T> GetByIdAsync(int id);

        //GetAll 
        public Task<IEnumerable<T>> GetAllAsync(Expression<Func<T,bool>> filter = null);


    }
}
