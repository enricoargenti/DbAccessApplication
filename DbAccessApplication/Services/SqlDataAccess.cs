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
            SELECT [DoorId]
                   ,[GatewayId]
                   ,[DeviceGeneratedCode]
                   ,[CloudGeneratedCode]
                   ,[AccessRequestTime]
              FROM [dbo].[OpenDoorRequests]
            ";
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<OpenDoorRequest>(query);
    }

    public async Task<OpenDoorRequest> GetOpenDoorRequestAsync(int id)
    {
        const string query = @"
            SELECT [DoorId]
                   ,[GatewayId]
                   ,[DeviceGeneratedCode]
                   ,[CloudGeneratedCode]
                   ,[AccessRequestTime]
              FROM [dbo].[OpenDoorRequests]
              WHERE Id = @id
            ";
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        return await connection.QueryFirstOrDefaultAsync<OpenDoorRequest>(query, new { id });
    }

    // Insert  into the db an open door request
    public async Task InsertOpenDoorRequestAsync(OpenDoorRequest product)
    {
        const string query = @"
            INSERT INTO [dbo].[OpenDoorRequests]
                       ([DoorId]
                       ,[GatewayId]
                       ,[DeviceGeneratedCode]
                       ,[CloudGeneratedCode]
                       ,[AccessRequestTime])
                 VALUES
                       (@DoorId
                       ,@GatewayId
                       ,@DeviceGeneratedCode
                       ,@CloudGeneratedCode
                       ,@AccessRequestTime)
            ";
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync(query, product);
    }
}
