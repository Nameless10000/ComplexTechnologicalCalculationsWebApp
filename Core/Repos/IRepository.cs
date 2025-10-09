using Core.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repos
{
    public interface IRepository<T> where T : Entity
    {
        Task<T> GetByIDAsync(int id);
        Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize);
        Task<T> AddAsync (T entity, int UserID);
        Task<T> UpdateAsync (T entity, int userID);
        Task<bool> DeleteAsync(int id);
    }
}
