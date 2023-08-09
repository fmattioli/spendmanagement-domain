namespace Domain.Entities
{
    public class Command
    {
        public Command(string routingKey, DateTime dataCommand, string nameCommand, string commandBody)
        {
            RoutingKey = routingKey;
            DataCommand = dataCommand;
            NameCommand = nameCommand;
            CommandBody = commandBody;
        }

        public string RoutingKey { get; set; }
        public DateTime DataCommand { get; set; }
        public string NameCommand { get; set; }
        public string CommandBody { get; set; }
    }
}
