using DemoWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoWebApi.DAL
{
    public interface IObjectTypeRepository
    {
        Task<List<ObjectType>> GetAllAsync();
        Task<ObjectType> GetAsync(int objectTypeId);
        Task<int> InsertAsync(ObjectType objectType);
        Task<int> UpdateAsync(ObjectType objectType);
        Task<int> DeleteAsync(int objectTypeId);
    }
}
