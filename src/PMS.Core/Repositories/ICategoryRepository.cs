using System;
using PMS.Core.Entities;
using PMS.Core.Repositories.Base;

namespace PMS.Core.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
    Task<Category> GetByIdAsync(int id);

}
