using System.Collections.Generic;
using System.Threading.Tasks;

namespace LogService.Repositories
{
    /// <summary>
    /// Interface for the implementation of the repository pattern
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T>
    {
        void Add(T entity);
        Task AddAsync(T entity);
        void AddRange(IList<T> entities);
        Task AddRangeAsync(IList<T> entities);
        void Save();
        Task SaveAsync();
        //void DeleteById(int id);
        //Task DeleteByIdAsync(int id);
        void Delete(T entity);
        Task DeleteAsync(T entity);
        T GetOne(int? id);
        Task<T> GetOneAsync(int? id);
        List<T> GetAll();
        Task<List<T>> GetAllAsync();
    }
}
