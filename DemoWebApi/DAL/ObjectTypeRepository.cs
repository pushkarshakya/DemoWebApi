using DemoWebApi.Models;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace DemoWebApi.DAL
{
    public class ObjectTypeRepository : IObjectTypeRepository
    {
        private readonly string _connectionString;
        public ObjectTypeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DemoDatabase");
        }

        public async Task<List<ObjectType>> GetAllAsync()
        {
            List<ObjectType> result = null;

            using MySqlConnection mySqlConnection = new MySqlConnection(_connectionString);
            mySqlConnection.Open();

            using MySqlCommand command = mySqlConnection.CreateCommand();
            command.CommandText = "object_type_get_all";
            command.CommandType = CommandType.StoredProcedure;

            using DbDataReader d = await command.ExecuteReaderAsync();
            while (await d.ReadAsync())
            {
                result ??= new List<ObjectType>();

                result.Add(new ObjectType
                {
                    ObjectTypeId = d.GetInt32(0),
                    ObjectTypeName = d.GetString(1),
                    Description = d.GetString(2),
                    Level = d.GetInt32(3)
                });
            }

            return result;
        }

        public async Task<ObjectType> GetAsync(int objectTypeId)
        {
            using MySqlConnection mySqlConnection = new MySqlConnection(_connectionString);
            mySqlConnection.Open();

            using MySqlCommand command = mySqlConnection.CreateCommand();
            command.CommandText = "object_type_get_by_id";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@in_object_type_id", objectTypeId);

            using DbDataReader d = await command.ExecuteReaderAsync();
            if (await d.ReadAsync())
            {
                return new ObjectType
                {
                    ObjectTypeId = d.GetInt32(0),
                    ObjectTypeName = d.GetString(1),
                    Description = d.GetString(2),
                    Level = d.GetInt32(3)
                };
            }
            else
                return null;
        }

        public async Task<int> InsertAsync(ObjectType objectType)
        {
            if (objectType == null || IsDefaultObjectType(objectType))
                return -1;

            if (objectType.Level < 1 || objectType.Level > 5)
                throw new Exception("Level must be in the range 1 to 5");

            using MySqlConnection mySqlConnection = new MySqlConnection(_connectionString);
            mySqlConnection.Open();

            using MySqlCommand command = mySqlConnection.CreateCommand();
            command.CommandText = "object_type_insert";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@in_object_type_name", objectType.ObjectTypeName);
            command.Parameters.AddWithValue("@in_description", objectType.Description);
            command.Parameters.AddWithValue("@in_level", objectType.Level);

            return await command.ExecuteNonQueryAsync();
        }

        public async Task<int> UpdateAsync(ObjectType objectType)
        {
            if (objectType == null || IsDefaultObjectType(objectType))
                return -1;

            if (objectType.Level < 1 || objectType.Level > 5)
                throw new Exception("Level must be in the range 1 to 5");

            using MySqlConnection mySqlConnection = new MySqlConnection(_connectionString);
            mySqlConnection.Open();

            using MySqlCommand command = mySqlConnection.CreateCommand();
            command.CommandText = "object_type_update";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@in_object_type_id", objectType.ObjectTypeId);
            command.Parameters.AddWithValue("@in_object_type_name", objectType.ObjectTypeName);
            command.Parameters.AddWithValue("@in_description", objectType.Description);
            command.Parameters.AddWithValue("@in_level", objectType.Level);

            return await command.ExecuteNonQueryAsync();
        }

        public async Task<int> DeleteAsync(int objectTypeId)
        {
            using MySqlConnection mySqlConnection = new MySqlConnection(_connectionString);
            mySqlConnection.Open();

            using MySqlCommand command = mySqlConnection.CreateCommand();
            command.CommandText = "object_type_delete";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@in_object_type_id", objectTypeId);

            return await command.ExecuteNonQueryAsync();
        }

        private bool IsDefaultObjectType(ObjectType objectType)
        {
            return objectType.ObjectTypeId == default(int) && string.IsNullOrWhiteSpace(objectType.ObjectTypeName) && string.IsNullOrWhiteSpace(objectType.Description) && objectType.Level == default(int);
        }

    }
}
