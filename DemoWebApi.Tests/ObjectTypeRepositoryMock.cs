using DemoWebApi.DAL;
using DemoWebApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DemoWebApi.Tests
{
    public class ObjectTypeRepositoryMock : IObjectTypeRepository
    {
        public Task<List<ObjectType>> GetAllAsync()
        {
            return Task.FromResult(
                new List<ObjectType>
                {
                    new ObjectType { ObjectTypeId = 1, ObjectTypeName = "o1", Description = "object1", Level = 1 },
                    new ObjectType { ObjectTypeId = 2, ObjectTypeName = "o2", Description = "object2", Level = 2 },
                    new ObjectType { ObjectTypeId = 3, ObjectTypeName = "o3", Description = "object3", Level = 3 },
                    new ObjectType { ObjectTypeId = 4, ObjectTypeName = "o4", Description = "object4", Level = 4 },
                    new ObjectType { ObjectTypeId = 5, ObjectTypeName = "o5", Description = "object5", Level = 5 },
                });
        }

        public Task<ObjectType> GetAsync(int objectTypeId)
        {
            if (objectTypeId == 1)
            {
                return Task.FromResult(
                    new ObjectType { ObjectTypeId = 1, ObjectTypeName = "o1", Description = "object1", Level = 1 });
            }
            else
                return Task.FromResult<ObjectType>(null);
        }

        public Task<int> InsertAsync(ObjectType objectType)
        {
            if (objectType == null || IsDefaultObjectType(objectType))
                return Task.FromResult(-1);

            if (objectType.Level < 1 || objectType.Level > 5)
                throw new Exception("Level must be in the range 1 to 5");

            return Task.FromResult(1);
        }

        public Task<int> UpdateAsync(ObjectType objectType)
        {
            if (objectType == null || IsDefaultObjectType(objectType))
                return Task.FromResult(0);

            if (objectType.Level < 1 || objectType.Level > 5)
                throw new Exception("Level must be in the range 1 to 5");

            return Task.FromResult(1);
        }

        public Task<int> DeleteAsync(int objectTypeId)
        {
            if (objectTypeId == -1)
                return Task.FromResult(0);
            else
                return Task.FromResult(1);
        }

        private bool IsDefaultObjectType(ObjectType objectType)
        {
            return objectType.ObjectTypeId == default(int) && string.IsNullOrWhiteSpace(objectType.ObjectTypeName) && string.IsNullOrWhiteSpace(objectType.Description) && objectType.Level == default(int);
        }
    }
}
