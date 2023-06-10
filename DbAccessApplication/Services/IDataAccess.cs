using DbAccessApplication.Models;

namespace DbAccessApplication.Services
{
    public interface IDataAccess
    {
        Task<IEnumerable<OpenDoorRequest>> GetOpenDoorRequestsAsync();

        Task<OpenDoorRequest> GetOpenDoorRequestAsync(int id);

        Task InsertOpenDoorRequestAsync(OpenDoorRequest openDoorRequest);
    }
}
