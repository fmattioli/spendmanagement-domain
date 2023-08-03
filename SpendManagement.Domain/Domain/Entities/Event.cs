namespace Domain.Entities
{
    public class Event
    {
        public Event(string routingKey, DateTime dataEvent, string nameEvent, string commandEvent)
        {
            RoutingKey = routingKey;
            DataEvent = dataEvent;
            NameEvent = nameEvent;
            EventBody = commandEvent;
        }

        public string RoutingKey { get; set; }
        public DateTime DataEvent { get; set; }
        public string NameEvent { get; set; }
        public string EventBody { get; set; }
    }
}
