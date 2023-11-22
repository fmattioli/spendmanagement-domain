namespace Domain.Entities
{
    public class Command
    {
        public Command(int id, string routingKey, DateTime dataCommand, string nameCommand, string commandBody)
        {
            Id = id;
            RoutingKey = routingKey;
            DataCommand = dataCommand;
            NameCommand = nameCommand;
            CommandBody = commandBody;
        }

        public Command(string routingKey, DateTime dataCommand, string nameCommand, string commandBody)
        {
            RoutingKey = routingKey;
            DataCommand = dataCommand;
            NameCommand = nameCommand;
            CommandBody = commandBody;
        }
        public int Id { get; set; }
        public string RoutingKey { get; set; }
        public DateTime DataCommand { get; set; }
        public string NameCommand { get; set; }
        public string CommandBody { get; set; }
    }
}
