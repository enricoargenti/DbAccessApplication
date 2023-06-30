namespace DbAccessApplication.Models;

public class OpenDoorRequest
{
    public int? Id { get; set; }
    public int DoorId { get; set; }
    public string DeviceId { get; set; }
    public string? DeviceGeneratedCode { get; set; }
    public string? CloudGeneratedCode { get; set; }
    public DateTime AccessRequestTime { get; set; }
    public string? UserId { get; set; }
}
