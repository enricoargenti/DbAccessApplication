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
    public Task Insert(OpenDoorRequest openDoorRequest)
    {
        return _dataAccess.InsertOpenDoorRequestAsync(openDoorRequest);
    }

    [HttpPut] // PUT api/opendoorrequests/{id}
    public Task Update(int id, OpenDoorRequest openDoorRequest)
    {
        return _dataAccess.UpdateOpenDoorRequestAsync(id, openDoorRequest);
    }

    [HttpDelete("{id}")] // DELETE api/opendoorrequests/{id}
    public Task Delete(int id)
    {
        return _dataAccess.DeleteOpenDoorRequestAsync(id);
    }
}
