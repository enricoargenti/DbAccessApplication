using Microsoft.AspNetCore.Mvc;
using DbAccessApplication.Models;
using DbAccessApplication.Services;

namespace DbAccessApplication.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DoorOpenRequestController : Controller
{
    private readonly IDataAccess _dataAccess;

    public DoorOpenRequestController(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    [HttpGet] // GET api/opendoorrequests
    public Task<IEnumerable<OpenDoorRequest>> GetAll()
    {
        return _dataAccess.GetOpenDoorRequestsAsync();
    }

    [HttpGet("accesses")] // GET api/doorOpenRequest/accesses
    public Task<IEnumerable<AccessExtended>> GetAllAccesses()
    {
        return _dataAccess.GetAccessesAsync();
    }


    [HttpGet("{id}")] // GET api/opendoorrequests/{id}
    public Task<OpenDoorRequest> GetById(int id)
    {
        return _dataAccess.GetOpenDoorRequestAsync(id);
    }
    
    
    [HttpGet("deviceGeneratedCode/{code}")] // GET api/opendoorrequests/{code}
    public Task<OpenDoorRequest> GetWhereCodeMatches(string code)
    {
        return _dataAccess.GetOpenDoorRequestWhereCodeIsMatchedAsync(code);
    }

    [HttpGet("user/{userId}/deviceId/{deviceId}")] // GET api/opendoorrequests/userPermissions
    public Task<UserPermissions> GetUserPermissions(string userId, string deviceId)
    {
        return _dataAccess.GetUserPermissionsAsync(userId, deviceId);
    }


    [HttpPost] // POST api/opendoorrequests
    public Task InsertNewOpenDoorRequest(OpenDoorRequest openDoorRequest)
    {
        return _dataAccess.InsertOpenDoorRequestAsync(openDoorRequest);
    }

    [HttpPost("newaccess")] // POST api/opendoorrequests/newaccess
    public Task InsertNewAccess(Access access)
    {
        return _dataAccess.InsertNewAccessAsync(access);
    }

    [HttpPut("{id}")] // PUT api/opendoorrequests/{id}
    public Task Update(int id, OpenDoorRequest openDoorRequest)
    {
        return _dataAccess.UpdateOpenDoorRequestAsync(id, openDoorRequest);
    }

    [HttpDelete("{id}")] // DELETE api/opendoorrequests/{id}
    public Task Delete(int id)
    {
        return _dataAccess.DeleteOpenDoorRequestAsync(id);
    }

    [HttpDelete("minutes/{minutes}")] // DELETE api/opendoorrequests/minutes/{minutes}
    public Task DeleteWhereTimeIsExpired(int minutes)
    {
        return _dataAccess.DeleteOpenDoorRequestsAsync(minutes);
    }
}
