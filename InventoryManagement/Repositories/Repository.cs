using InventoryManagement.DbContexts;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace InventoryManagement.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger _logger;


        public Repository(ApplicationDbContext dbContext, ILogger logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        protected string EntityName => typeof(TEntity).Name;

        protected async Task CheckConnectionOpenAsync()
        {
            if (_dbContext.Connection.State != ConnectionState.Open)
            {
                await _dbContext.Connection.OpenAsync();
            }
        }

        protected SqlCommand CreateCommand(string storedProcedureName)
        {
            var cmd = new SqlCommand(storedProcedureName, _dbContext.Connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            return cmd;
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            try
            {
                using (var cmd = CreateCommand($"dbo.Get{EntityName}ById"))
                {

                    cmd.Parameters.Add(new SqlParameter($"@{EntityName}ID", id));
                    await CheckConnectionOpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return MapToEntity(reader);
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error retrieving {EntityName} by ID: {id}");
                throw;
            }
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            var entities = new List<TEntity>();
            try
            {
                using (var cmd = CreateCommand($"dbo.Get{EntityName}List"))
                {
                    await CheckConnectionOpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            entities.Add(MapToEntity(reader));
                        }
                    }
                }
                return entities;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error retrieving {EntityName} list", ex);
                throw;
            }

        }
        public async Task AddAsync(TEntity entity)
        {
            try
            {
                using (var cmd = CreateCommand($"dbo.Insert{EntityName}"))
                {
                    AddParameters(cmd, entity);
                    await CheckConnectionOpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Error adding {EntityName}", ex);
                throw;
            }
        }
        public async Task UpdateAsync(TEntity entity)
        {
            try
            {
                using (var cmd = CreateCommand($"dbo.Update{EntityName}"))
                {

                    AddParameters(cmd, entity);
                    await CheckConnectionOpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Error updating {EntityName}", ex);
                throw;
            }
        }
        public async Task DeleteAsync(int id)
        {
            try
            {
                using (var cmd = CreateCommand($"dbo.Delete{EntityName}"))
                {

                    cmd.Parameters.Add(new SqlParameter($"@{EntityName}ID", id));

                    if (_dbContext.Connection.State != ConnectionState.Open)
                        _dbContext.Connection.Open();

                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error deleting {EntityName}", ex);
                throw;
            }
        }


        protected virtual TEntity MapToEntity(SqlDataReader reader)
        {
            throw new NotImplementedException();
        }
        protected virtual void AddParameters(SqlCommand command, TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}