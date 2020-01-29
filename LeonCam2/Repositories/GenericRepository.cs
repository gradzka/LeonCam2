// GenericRepository.cs by Gradzka & Kazimierczak

namespace LeonCam2.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using Dapper;

    public abstract class GenericRepository<T> : IGenericRepository<T>
        where T : class
    {
        private readonly string tableName;

        protected GenericRepository(string tableName)
        {
            this.tableName = tableName;
        }

        private SqlConnection SqlConnection => new SqlConnection(ConfigurationManager.ConnectionStrings["MainDb"].ConnectionString);

        private IEnumerable<PropertyInfo> GetProperties => typeof(T).GetProperties();

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            using (var connection = this.CreateConnection())
            {
                return await connection.QueryAsync<T>($"SELECT * FROM {this.tableName}");
            }
        }

        public async Task DeleteRowAsync(Guid id)
        {
            using (var connection = this.CreateConnection())
            {
                await connection.ExecuteAsync($"DELETE FROM {this.tableName} WHERE Id=@Id", new { Id = id });
            }
        }

        public async Task<T> GetAsync(Guid id)
        {
            using (var connection = this.CreateConnection())
            {
                var result = await connection.QuerySingleOrDefaultAsync<T>($"SELECT * FROM {this.tableName} WHERE Id=@Id", new { Id = id });
                if (result == null)
                {
                    throw new KeyNotFoundException($"{this.tableName} with id [{id}] could not be found.");
                }

                return result;
            }
        }

        public async Task<int> SaveRangeAsync(IEnumerable<T> list)
        {
            var inserted = 0;
            var query = this.GenerateInsertQuery();
            using (var connection = this.CreateConnection())
            {
                inserted += await connection.ExecuteAsync(query, list);
            }

            return inserted;
        }

        public async Task InsertAsync(T t)
        {
            var insertQuery = this.GenerateInsertQuery();

            using (var connection = this.CreateConnection())
            {
                await connection.ExecuteAsync(insertQuery, t);
            }
        }

        public async Task UpdateAsync(T t)
        {
            var updateQuery = this.GenerateUpdateQuery();

            using (var connection = this.CreateConnection())
            {
                await connection.ExecuteAsync(updateQuery, t);
            }
        }

        private static List<string> GenerateListOfProperties(IEnumerable<PropertyInfo> listOfProperties)
        {
            return (from prop in listOfProperties
                    let attributes = prop.GetCustomAttributes(typeof(DescriptionAttribute), false)
                    where attributes.Length <= 0 || (attributes[0] as DescriptionAttribute)?.Description != "ignore"
                    select prop.Name).ToList();
        }

        private IDbConnection CreateConnection()
        {
            var conn = this.SqlConnection;
            conn.Open();
            return conn;
        }

        private string GenerateInsertQuery()
        {
            var insertQuery = new StringBuilder($"INSERT INTO {this.tableName} ");

            insertQuery.Append("(");

            var properties = GenerateListOfProperties(this.GetProperties);
            properties.ForEach(prop => { insertQuery.Append($"[{prop}],"); });

            insertQuery
                .Remove(insertQuery.Length - 1, 1)
                .Append(") VALUES (");

            properties.ForEach(prop => { insertQuery.Append($"@{prop},"); });

            insertQuery
                .Remove(insertQuery.Length - 1, 1)
                .Append(")");

            return insertQuery.ToString();
        }

        private string GenerateUpdateQuery()
        {
            var updateQuery = new StringBuilder($"UPDATE {this.tableName} SET ");
            var properties = GenerateListOfProperties(this.GetProperties);

            properties.ForEach(property =>
            {
                if (!property.Equals("Id"))
                {
                    updateQuery.Append($"{property}=@{property},");
                }
            });

            updateQuery.Remove(updateQuery.Length - 1, 1); // remove last comma
            updateQuery.Append(" WHERE Id=@Id");

            return updateQuery.ToString();
        }
    }
}
