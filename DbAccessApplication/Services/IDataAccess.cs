using DbAccessApplication.Models;

namespace DbAccessApplication.Services
{
    public interface IDataAccess
    {
        Task<IEnumerable<OpenDoorRequest>> GetOpenDoorRequestsAsync();

        Task<OpenDoorRequest> GetOpenDoorRequestAsync(int id);

        Task<OpenDoorRequest> GetOpenDoorRequestWhereCodeIsMatchedAsync(string code);

        Task<UserPermissions> GetUserPermissionsAsync(string userId, string deviceId);

        Task InsertOpenDoorRequestAsync(OpenDoorRequest openDoorRequest);

        Task UpdateOpenDoorRequestAsync(int id, OpenDoorRequest updatedRequest);

        Task DeleteOpenDoorRequestAsync(int id);
        Task DeleteOpenDoorRequestsAsync(int minutes);

    }
}
