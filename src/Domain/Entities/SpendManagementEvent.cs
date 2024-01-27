using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class SpendManagementEvent(int FK_Command_Id, string routingKey, DateTime dataEvent, string nameEvent, string eventBody)
    {
        [Column("FK_Command_Id")]
        public int Id { get; set; } = FK_Command_Id;
        public string RoutingKey { get; set; } = routingKey;
        public DateTime DataEvent { get; set; } = dataEvent;
        public string NameEvent { get; set; } = nameEvent;
        public string EventBody { get; set; } = eventBody;
    }
}
