using DbAccessApplication.Models;
using Microsoft.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace DbAccessApplication.Services;

public class SqlDataAccess : IDataAccess
{
    private readonly string _connectionString;

    public SqlDataAccess(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("AzureDb");
    }

    // GET: Ask the db for every open door request
    public async Task<IEnumerable<OpenDoorRequest>> GetOpenDoorRequestsAsync()
    {
        const string query = @"
            SELECT [Id]
                   ,[DoorId]
                   ,[GatewayId]
                   ,[DeviceGeneratedCode]
                   ,[CloudGeneratedCode]
                   ,[AccessRequestTime]
              FROM [dbo].[OpenDoorRequests]
            ";
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<OpenDoorRequest>(query);
    }

    //GET: Ask the db for a particular open door request
    public async Task<OpenDoorRequest> GetOpenDoorRequestAsync(int id)
    {
        const string query = @"
            SELECT [Id]
                   ,[DoorId]
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

    // POST: Insert into the db an open door request
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

    //PUT: Modify a particular open door request
    public async Task UpdateOpenDoorRequestAsync(int id, OpenDoorRequest updatedRequest)
    {
        const string query = @"
            UPDATE [dbo].[OpenDoorRequests]
            SET [DoorId] = @DoorId,
                [GatewayId] = @GatewayId,
                [DeviceGeneratedCode] = @DeviceGeneratedCode,
                [CloudGeneratedCode] = @CloudGeneratedCode,
                [AccessRequestTime] = @AccessRequestTime
            WHERE Id = @Id
        ";

        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        updatedRequest.Id = id; // Set the ID of the updated request

        await connection.ExecuteAsync(query, updatedRequest);
    }

    //DELETE: Ask the db for a particular open door request
    public async Task DeleteOpenDoorRequestAsync(int id)
    {
        const string query = @"
            DELETE FROM [dbo].[OpenDoorRequests]
              WHERE Id = @id
            ";

        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        await connection.ExecuteAsync(query, new { Id = id });
    }



}
