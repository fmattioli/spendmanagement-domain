using System.Text;

namespace Data.Statements
{
    public static class SQLStatements
    {
        private static readonly StringBuilder QueryBuilder = new();
        public static string InsertCommand()
        {
            QueryBuilder.Clear();
            QueryBuilder.AppendLine("INSERT INTO SpendManagementCommands");
            QueryBuilder.AppendLine("( ");
            QueryBuilder.AppendLine("RoutingKey, ");
            QueryBuilder.AppendLine("DataCommand, ");
            QueryBuilder.AppendLine("NameCommand, ");
            QueryBuilder.AppendLine("CommandBody ");
            QueryBuilder.AppendLine(") ");
            QueryBuilder.AppendLine(" VALUES ");
            QueryBuilder.AppendLine(" ( ");
            QueryBuilder.AppendLine("  @RoutingKey,  ");
            QueryBuilder.AppendLine("  @DataCommand,  ");
            QueryBuilder.AppendLine("  @NameCommand,  ");
            QueryBuilder.AppendLine("  @CommandBody  ");
            QueryBuilder.AppendLine("  )  ");
            return QueryBuilder.ToString();
        }
    }
}
