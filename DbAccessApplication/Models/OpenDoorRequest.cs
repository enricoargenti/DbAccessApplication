namespace DbAccessApplication.Models
{
    public class OpenDoorRequest
    {
        public string DoorId { get; set; }
        public string GatewayId { get; set; }
        public int DeviceGeneratedCode { get; set; }
        public int CloudGeneratedCode { get; set; }
        public DateTime AccessRequestTime { get; set; }
    }
}
