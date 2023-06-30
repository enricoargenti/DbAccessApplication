using DbAccessApplication.Models;
using Microsoft.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;

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
            SELECT [OpenDoorRequests].[Id]
                ,[OpenDoorRequests].[DoorId]
                ,[Gateways].[DeviceId]
                ,[OpenDoorRequests].[DeviceGeneratedCode]
                ,[OpenDoorRequests].[CloudGeneratedCode]
                ,[OpenDoorRequests].[AccessRequestTime]
                ,[OpenDoorRequests].[UserId]
            FROM [dbo].[OpenDoorRequests]
            JOIN [dbo].[Gateways]
            ON [Gateways].[Id] = [OpenDoorRequests].[GatewayId]
        ";
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<OpenDoorRequest>(query);
    }

    // GET: Ask the db for every access saved
    public async Task<IEnumerable<AccessExtended>> GetAccessesAsync()
    {
        const string query = @"
            SELECT [AspNetUsers].[UserName]
                ,[Accesses].[DoorId]
                ,[Gateways].[DeviceId]
                ,[Buildings].[Name] AS BuildingName
                ,[Buildings].[Description] AS BuildingDescription
                ,[Accesses].[AccessRequestTime]
            FROM [dbo].[Accesses]
            JOIN [dbo].[Gateways]
            ON [Gateways].[Id] = [Accesses].[GatewayId]
            JOIN [dbo].[Buildings]
            ON [Gateways].[BuildingId] = [Buildings].[Id]
            JOIN [dbo].[AspNetUsers]
            ON [Accesses].[UserId] = [AspNetUsers].[Id]
        ";
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<AccessExtended>(query);
    }

    //GET: Ask the db for a particular open door request
    public async Task<OpenDoorRequest> GetOpenDoorRequestAsync(int id)
    {
        const string query = @"
            SELECT [OpenDoorRequests].[Id]
                ,[OpenDoorRequests].[DoorId]
                ,[Gateways].[DeviceId]
                ,[OpenDoorRequests].[DeviceGeneratedCode]
                ,[OpenDoorRequests].[CloudGeneratedCode]
                ,[OpenDoorRequests].[AccessRequestTime]
                ,[OpenDoorRequests].[UserId]
            FROM [dbo].[OpenDoorRequests]
            JOIN [dbo].[Gateways]
            ON [Gateways].[Id] = [OpenDoorRequests].[GatewayId]
            WHERE [OpenDoorRequests].[Id] = @id
        ";
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        return await connection.QueryFirstOrDefaultAsync<OpenDoorRequest>(query, new { id });
    }

    //GET: Ask the db for the open door request where the deviceGeneratedCode
    //is equal to the one inserted by the user
    public async Task<OpenDoorRequest> GetOpenDoorRequestWhereCodeIsMatchedAsync(string code)
    {
        const string query = @"
            SELECT [OpenDoorRequests].[Id]
                ,[OpenDoorRequests].[DoorId]
                ,[Gateways].[DeviceId]
                ,[OpenDoorRequests].[DeviceGeneratedCode]
                ,[OpenDoorRequests].[CloudGeneratedCode]
                ,[OpenDoorRequests].[AccessRequestTime]
                ,[OpenDoorRequests].[UserId]
            FROM [dbo].[OpenDoorRequests]
            JOIN [dbo].[Gateways] 
            ON [Gateways].[Id] = [OpenDoorRequests].[GatewayId]
            WHERE [OpenDoorRequests].[DeviceGeneratedCode] = @code
        ";
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        return await connection.QueryFirstOrDefaultAsync<OpenDoorRequest>(query, new {code});
    }

    //GET: Ask the db for the user permission to access the specific gateway/building
    public async Task<UserPermissions> GetUserPermissionsAsync(string userId, string deviceId)
    {
        const string query = @"
            SELECT [AspNetUsers].[UserName]
                ,[AspNetUsers].[Id] AS [UserId] 
                ,[Gateways].[DeviceId]
            FROM [AspNetUsers]
            JOIN [UsersGateways]
            ON [AspNetUsers].[Id] = [UsersGateways].[UserId]
            JOIN [Gateways]
            ON [Gateways].[Id] = [UsersGateways].[GatewayId]
            WHERE [AspNetUsers].[Id] = @userId
            AND [Gateways].[DeviceId] = @deviceId
        ";
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        return await connection.QueryFirstOrDefaultAsync<UserPermissions>(query, new { userId, deviceId });
    }


    // POST: Insert into the db an open door request
    public async Task InsertOpenDoorRequestAsync(OpenDoorRequest openDoorRequest)
    {
        const string query = @"
            INSERT INTO [dbo].[OpenDoorRequests]
                ([DoorId]
                ,[GatewayId]
                ,[DeviceGeneratedCode]
                ,[CloudGeneratedCode]
                ,[AccessRequestTime])
            SELECT
                @DoorId,
                [Gateways].[Id],
                @DeviceGeneratedCode,
                @CloudGeneratedCode,
                @AccessRequestTime
            FROM
                [dbo].[Gateways]
            WHERE
                [Gateways].[DeviceId] = @DeviceId
        ";
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync(query, openDoorRequest);
    }

    // POST: Insert into the db a new access
    public async Task InsertNewAccessAsync(Access access)
    {
        const string query = @"
            INSERT INTO [dbo].[Accesses]
                ([UserId]
                ,[DoorId]
                ,[GatewayId]
                ,[AccessRequestTime])
            SELECT
                @UserId,
                @DoorId,
                [Gateways].[Id],
                @AccessRequestTime
            FROM
                [dbo].[Gateways]
            WHERE
                [Gateways].[DeviceId] = @DeviceId
        ";
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync(query, access);
    }

    //PUT: Modify a particular open door request
    public async Task UpdateOpenDoorRequestAsync(int id, OpenDoorRequest updatedRequest)
    {
        Console.WriteLine("Id: " + id);
        Console.WriteLine("updatedRequest: " + updatedRequest.UserId);
        const string query = @"
            UPDATE [dbo].[OpenDoorRequests]
            SET [DoorId] = @DoorId,
                [GatewayId] = [Gateways].[Id],
                [DeviceGeneratedCode] = @DeviceGeneratedCode,
                [CloudGeneratedCode] = @CloudGeneratedCode,
                [AccessRequestTime] = @AccessRequestTime,
                [UserId] = @UserId
            FROM [dbo].[OpenDoorRequests]
            JOIN [dbo].[Gateways] 
            ON [Gateways].[DeviceId] = @DeviceId
            WHERE [OpenDoorRequests].[Id] = @Id
        ";

        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        updatedRequest.Id = id; // Set the ID of the updated request

        await connection.ExecuteAsync(query, updatedRequest);
    }


    //DELETE: Drop a particular open door request
    public async Task DeleteOpenDoorRequestAsync(int id)
    {
        const string query = @"
            DELETE FROM [dbo].[OpenDoorRequests]
                WHERE [OpenDoorRequests].[Id] = @id
        ";

        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        await connection.ExecuteAsync(query, new { Id = id });
    }


    //DELETE: Drop every open door request older than time passed
    public async Task DeleteOpenDoorRequestsAsync(int minutes)
    {
        DateTime minutesAgo = DateTime.Now.AddMinutes(-minutes);


        const string query = @"
            DELETE FROM [dbo].[OpenDoorRequests]
                WHERE [OpenDoorRequests].[AccessRequestTime] < @minutesAgo
        ";

        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        await connection.ExecuteAsync(query, new { MinutesAgo = minutesAgo });
    }



}
