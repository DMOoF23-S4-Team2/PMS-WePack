using System;
using PMS.Core.Entities;

namespace PMS.Core.Repositories.Base;

 public interface IRepository<T> where T : class
 {
     Task<IReadOnlyList<T>> GetAllAsync();
     Task<T> AddAsync(T entity);
     Task UpdateAsync(T entity);
     Task DeleteAsync(T entity);
    Task AddManyAsync(IEnumerable<T> entities);
    Task UpdateManyAsync(IEnumerable<T> entities);
    Task DeleteManyAsync(IEnumerable<T> entities);
 }
