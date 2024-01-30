namespace Domain.Entities
{
    public class SpendManagementCommand(string routingKey, DateTime dataCommand, string nameCommand, string commandBody)
    {
        public string RoutingKey { get; set; } = routingKey;
        public DateTime DataCommand { get; set; } = dataCommand;
        public string NameCommand { get; set; } = nameCommand;
        public string CommandBody { get; set; } = commandBody;
    }
}
