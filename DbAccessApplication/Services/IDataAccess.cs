using DbAccessApplication.Models;

namespace DbAccessApplication.Services
{
    public interface IDataAccess
    {
        Task<IEnumerable<OpenDoorRequest>> GetOpenDoorRequestsAsync();

        Task<OpenDoorRequest> GetOpenDoorRequestAsync(int id);

        Task<OpenDoorRequest> GetOpenDoorRequestWhereCodeIsMatchedAsync(int code);

        Task InsertOpenDoorRequestAsync(OpenDoorRequest openDoorRequest);

        Task UpdateOpenDoorRequestAsync(int id, OpenDoorRequest updatedRequest);

        Task DeleteOpenDoorRequestAsync(int id);

    }
}
