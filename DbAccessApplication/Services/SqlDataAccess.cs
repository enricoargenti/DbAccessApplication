using DbAccessApplication.Models;
using Microsoft.Data.SqlClient;
using Dapper;

namespace DbAccessApplication.Services;

public class SqlDataAccess : IDataAccess
{
    private readonly string _connectionString;

    public SqlDataAccess(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("AzureDb");
    }

    public async Task<IEnumerable<OpenDoorRequest>> GetOpenDoorRequestsAsync()
    {
        const string query = @"
            SELECT [Id]
                  ,[Name]
                  ,[Description]
                  ,[Price]
              FROM [dbo].[Products]
            ";
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<OpenDoorRequest>(query);
    }

    public async Task<OpenDoorRequest> GetOpenDoorRequestAsync(int id)
    {
        const string query = @"
            SELECT [Id]
                  ,[Name]
                  ,[Description]
                  ,[Price]
              FROM [dbo].[Products]
              WHERE Id = @id
            ";
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        return await connection.QueryFirstOrDefaultAsync<OpenDoorRequest>(query, new { id });
    }

    public async Task InsertOpenDoorRequestAsync(OpenDoorRequest product)
    {
        const string query = @"
            INSERT INTO [dbo].[Products]
                       ([Name]
                       ,[Description]
                       ,[Price])
                 VALUES
                       (@Name
                       ,@Description
                       ,@Price)
            ";
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync(query, product);
    }
}
