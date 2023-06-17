﻿using DbAccessApplication.Models;
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
            SELECT [OpenDoorRequests].[Id]
                ,[OpenDoorRequests].[DoorId]
                ,[Gateways].[DeviceId]
                ,[OpenDoorRequests].[DeviceGeneratedCode]
                ,[OpenDoorRequests].[CloudGeneratedCode]
                ,[OpenDoorRequests].[AccessRequestTime]
            FROM [dbo].[OpenDoorRequests]
            JOIN [dbo].[Gateways]
            ON [Gateways].[Id] = [OpenDoorRequests].[GatewayId]
        ";
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<OpenDoorRequest>(query);
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
            FROM [dbo].[OpenDoorRequests]
            JOIN [dbo].[Gateways] 
            ON [Gateways].[Id] = [OpenDoorRequests].[GatewayId]
            WHERE [OpenDoorRequests].[DeviceGeneratedCode] = @code
        ";
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        return await connection.QueryFirstOrDefaultAsync<OpenDoorRequest>(query, new {code});
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
        await connection.ExecuteAsync(query, product);
    }

    //PUT: Modify a particular open door request
    public async Task UpdateOpenDoorRequestAsync(int id, OpenDoorRequest updatedRequest)
    {
        const string query = @"
            UPDATE [dbo].[OpenDoorRequests]
            SET [DoorId] = @DoorId,
                [GatewayId] = [Gateways].[Id],
                [DeviceGeneratedCode] = @DeviceGeneratedCode,
                [CloudGeneratedCode] = @CloudGeneratedCode,
                [AccessRequestTime] = @AccessRequestTime
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

    //DELETE: Ask the db for a particular open door request
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



}
