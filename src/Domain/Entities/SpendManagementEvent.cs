namespace Domain.Entities
{
    public class SpendManagementEvent(string routingKey, DateTime dataEvent, string nameEvent, string eventBody)
    {
        public string RoutingKey { get; set; } = routingKey;
        public DateTime DataEvent { get; set; } = dataEvent;
        public string NameEvent { get; set; } = nameEvent;
        public string EventBody { get; set; } = eventBody;
    }
}
