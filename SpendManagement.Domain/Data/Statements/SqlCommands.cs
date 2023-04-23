using System.Text;

namespace Data.Statements
{
    internal static class SqlCommands
    {
        private static StringBuilder QueryBuilder = new StringBuilder();
        public static string InsertReceipt(string table)
        {
            QueryBuilder.Clear();
            QueryBuilder.AppendLine($"INSERT INTO [{table}]");
            QueryBuilder.AppendLine(" OUTPUT Inserted.Id  ");
            QueryBuilder.AppendLine(" VALUES ");
            QueryBuilder.AppendLine(" ( ");
            QueryBuilder.AppendLine("  @ReceiptGuid,  ");
            QueryBuilder.AppendLine("  @EstablishmentName,  ");
            QueryBuilder.AppendLine("  @ReceiptDate  ");
            QueryBuilder.AppendLine("  )  ");
            return QueryBuilder.ToString();
        }

        public static string InsertReceiptItems(string table)
        {
            QueryBuilder.Clear();
            QueryBuilder.AppendLine($" INSERT INTO [{table}]");
            QueryBuilder.AppendLine("  (  ");
            QueryBuilder.AppendLine("  [ReceiptId], ");
            QueryBuilder.AppendLine("  [ReceiptItemGuid], ");
            QueryBuilder.AppendLine("  [ItemName], ");
            QueryBuilder.AppendLine("  [Quantity], ");
            QueryBuilder.AppendLine("  [ItemPrice], ");
            QueryBuilder.AppendLine("  [Observation] ");
            QueryBuilder.AppendLine("  ) ");
            QueryBuilder.AppendLine(" VALUES ");
            QueryBuilder.AppendLine(" ( ");
            QueryBuilder.AppendLine("  @ReceiptId,  ");
            QueryBuilder.AppendLine("  @ReceiptItemGuid,  ");
            QueryBuilder.AppendLine("  @ItemName,  ");
            QueryBuilder.AppendLine("  @Quantity,  ");
            QueryBuilder.AppendLine("  @ItemPrice,  ");
            QueryBuilder.AppendLine("  @Observation  ");
            QueryBuilder.AppendLine("  )  ");
            return QueryBuilder.ToString();
        }
    }
}
