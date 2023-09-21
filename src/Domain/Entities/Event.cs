using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Event
    {
        public Event(int id, string routingKey, DateTime dataEvent, string nameEvent, string commandEvent)
        {
            Id = id;
            RoutingKey = routingKey;
            DataEvent = dataEvent;
            NameEvent = nameEvent;
            EventBody = commandEvent;
        }

        [Column("FK_Command_Id")]
        public int Id { get; set; }
        public string RoutingKey { get; set; }
        public DateTime DataEvent { get; set; }
        public string NameEvent { get; set; }
        public string EventBody { get; set; }
    }
}
