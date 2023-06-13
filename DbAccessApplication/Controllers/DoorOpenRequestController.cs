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

    [HttpGet]   // GET api/opendoorrequests
    public Task<IEnumerable<OpenDoorRequest>> GetAll()
    {
        return _dataAccess.GetOpenDoorRequestsAsync();
    }

    [HttpGet("{id}")]   // GET api/opendoorrequests/{id}
    public Task<OpenDoorRequest> GetById(int id)
    {
        return _dataAccess.GetOpenDoorRequestAsync(id);
    }

    [HttpPost] // POST api/opendoorrequests
    public Task Insert(OpenDoorRequest openDoorRequest)
    {
        return _dataAccess.InsertOpenDoorRequestAsync(openDoorRequest);
    }

    [HttpPut] //PUT api/opendoorrequests/{id}
    public Task Update(int id, OpenDoorRequest openDoorRequest)
    {
        return _dataAccess.UpdateOpenDoorRequestAsync(id, openDoorRequest);
    }
}
