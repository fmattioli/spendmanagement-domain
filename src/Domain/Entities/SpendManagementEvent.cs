using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class SpendManagementEvent
    {
        public SpendManagementEvent(int FK_Command_Id, string routingKey, DateTime dataEvent, string nameEvent, string eventBody)
        {
            Id = FK_Command_Id;
            RoutingKey = routingKey;
            DataEvent = dataEvent;
            NameEvent = nameEvent;
            EventBody = eventBody;
        }

        [Column("FK_Command_Id")]
        public int Id { get; set; }
        public string RoutingKey { get; set; }
        public DateTime DataEvent { get; set; }
        public string NameEvent { get; set; }
        public string EventBody { get; set; }
    }
}
