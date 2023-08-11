using System.Text;

namespace Data.Statements
{
    public static class SQLStatements
    {
        private static readonly StringBuilder QueryBuilder = new();
        public static string InsertCommand()
        {
            QueryBuilder.Clear();
            QueryBuilder.AppendLine($"INSERT INTO [Commands]");
            QueryBuilder.AppendLine("( ");
            QueryBuilder.AppendLine("[RoutingKey], ");
            QueryBuilder.AppendLine("[DataCommand], ");
            QueryBuilder.AppendLine("[NameCommand], ");
            QueryBuilder.AppendLine("[CommandBody] ");
            QueryBuilder.AppendLine(") ");
            QueryBuilder.AppendLine("OUTPUT Inserted.ID ");
            QueryBuilder.AppendLine(" VALUES ");
            QueryBuilder.AppendLine(" ( ");
            QueryBuilder.AppendLine("  @RoutingKey,  ");
            QueryBuilder.AppendLine("  @DataCommand,  ");
            QueryBuilder.AppendLine("  @NameCommand,  ");
            QueryBuilder.AppendLine("  @CommandBody  ");
            QueryBuilder.AppendLine("  )  ");
            return QueryBuilder.ToString();
        }

        public static string InsertEvent()
        {
            QueryBuilder.Clear();
            QueryBuilder.AppendLine($"INSERT INTO [Events]");
            QueryBuilder.AppendLine("( ");
            QueryBuilder.AppendLine("[FK_Command_Id], ");
            QueryBuilder.AppendLine("[RoutingKey], ");
            QueryBuilder.AppendLine("[DataEvent], ");
            QueryBuilder.AppendLine("[NameEvent], ");
            QueryBuilder.AppendLine("[EventBody] ");
            QueryBuilder.AppendLine(") ");
            QueryBuilder.AppendLine(" VALUES ");
            QueryBuilder.AppendLine(" ( ");
            QueryBuilder.AppendLine("  @Id,  ");
            QueryBuilder.AppendLine("  @RoutingKey,  ");
            QueryBuilder.AppendLine("  @DataEvent,  ");
            QueryBuilder.AppendLine("  @NameEvent,  ");
            QueryBuilder.AppendLine("  @EventBody  ");
            QueryBuilder.AppendLine("  )  ");
            return QueryBuilder.ToString();
        }
    }
}
