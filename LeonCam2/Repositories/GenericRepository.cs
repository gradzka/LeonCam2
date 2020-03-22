// GenericRepository.cs by Gradzka & Kazimierczak

namespace LeonCam2.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using Dapper;

    public abstract class GenericRepository<T> : IGenericRepository<T>
        where T : class
    {
        private readonly IDbConnection connection;

        protected GenericRepository(IDbConnection dbConnection)
        {
            this.TableName = typeof(T).Name;
            this.connection = dbConnection;

            if (!this.CheckIfTableExists())
            {
                this.CreateTable();
            }
            else
            {
                this.CheckTableColumns();
            }
        }

        protected string TableName { get; }

        private IEnumerable<PropertyInfo> Properties => typeof(T).GetProperties().Where(x =>
        {
            var attributes = x.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length <= 0 || (attributes[0] as DescriptionAttribute)?.Description != "ignore";
        });

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await this.connection.QueryAsync<T>($"SELECT * FROM {this.TableName}");
        }

        public async Task DeleteRowAsync(Guid id)
        {
            await this.connection.ExecuteAsync($"DELETE FROM {this.TableName} WHERE Id=@Id", new { Id = id });
        }

        public async Task<T> GetAsync(Guid id)
        {
            return await this.connection.QuerySingleOrDefaultAsync<T>($"SELECT * FROM {this.TableName} WHERE Id=@Id", new { Id = id }) ?? throw new KeyNotFoundException($"{this.TableName} with id [{id}] could not be found.");
        }

        public async Task<int> SaveRangeAsync(IEnumerable<T> list)
        {
            return await this.connection.ExecuteAsync(this.GenerateInsertQuery(), list);
        }

        public async Task InsertAsync(T t)
        {
            await this.connection.ExecuteAsync(this.GenerateInsertQuery(), t);
        }

        public async Task UpdateAsync(T t)
        {
            await this.connection.ExecuteAsync(this.GenerateUpdateQuery(), t);
        }

        private string GenerateInsertQuery()
        {
            var properties = this.Properties.Select(x => x.Name).Where(x => x != "Id").ToList();

            var insertQuery = new StringBuilder($"INSERT INTO {this.TableName} ").Append("(");

            properties.ForEach(prop => { insertQuery.Append($"[{prop}],"); });

            insertQuery.Remove(insertQuery.Length - 1, 1).Append(") VALUES (");

            properties.ForEach(prop => { insertQuery.Append($"@{prop},"); });

            insertQuery.Remove(insertQuery.Length - 1, 1).Append(")");

            return insertQuery.ToString();
        }

        private string GenerateUpdateQuery()
        {
            var updateQuery = new StringBuilder($"UPDATE {this.TableName} SET ");

            var properties = this.Properties.Select(x => x.Name).ToList();

            properties.ForEach(property =>
            {
                if (!property.Equals("Id"))
                {
                    updateQuery.Append($"{property}=@{property},");
                }
            });

            updateQuery.Remove(updateQuery.Length - 1, 1); //// Remove last comma
            updateQuery.Append(" WHERE Id=@Id");

            return updateQuery.ToString();
        }

        private void CreateTable()
        {
            var query = new StringBuilder($"CREATE TABLE {this.TableName} ").Append("(");

            foreach (var property in this.Properties)
            {
                var sqlType = this.ConvertToSQLType(property.PropertyType);

                if ((RequiredAttribute)property.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault() != null)
                {
                    sqlType += $" NOT NULL";
                }

                if (property.Name == "Id")
                {
                    sqlType += $" PRIMARY KEY";
                }

                query.Append($"{property.Name} {sqlType}, ");
            }

            query.Remove(query.Length - 2, 2).Append(")");

            this.connection.Execute(query.ToString());
        }

        private void CheckTableColumns()
        {
            var dbColumns = this.connection.Query<string>($"SELECT name FROM pragma_table_info(@name);", new { name = this.TableName });
            var missingDbColumns = this.Properties.Where(x => !dbColumns.Contains(x.Name));

            foreach (var dbColumn in missingDbColumns)
            {
                var sqlType = this.ConvertToSQLType(dbColumn.PropertyType);

                if ((RequiredAttribute)dbColumn.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault() != null)
                {
                    sqlType += $" NOT NULL";
                }

                this.connection.Execute($"ALTER TABLE {this.TableName} ADD {dbColumn.Name} {sqlType}");
            }
        }

        private string ConvertToSQLType(Type type)
        {
            return type.Name switch
            {
                "String" => "VARCHAR",
                "DateTime" => "DATETIME",
                "Int32" => "INTEGER",
                _ => string.Empty,
            };
        }

        private bool CheckIfTableExists()
        {
            return this.connection.QuerySingle<int>($"SELECT COUNT(1) FROM sqlite_master WHERE type = 'table' AND name = @name", new { name = this.TableName }) == 1;
        }
    }
}
