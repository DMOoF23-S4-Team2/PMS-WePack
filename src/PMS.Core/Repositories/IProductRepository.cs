using System;
using PMS.Core.Entities;
using PMS.Core.Repositories.Base;

namespace PMS.Core.Repositories;

public interface IProductRepository : IRepository<Product>
{
    //REVIEW - Skal vi implementere en specifik metode til at uploade fra csv-fil?
}
