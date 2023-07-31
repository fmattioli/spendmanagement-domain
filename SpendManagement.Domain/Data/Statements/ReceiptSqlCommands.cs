using System.Text;

namespace Data.Statements
{
    public static class ReceiptSqlCommands
    {
        private static readonly StringBuilder QueryBuilder = new();
        public static string InsertReceipt()
        {
            QueryBuilder.Clear();
            QueryBuilder.AppendLine($"INSERT INTO [Receipt]");
            QueryBuilder.AppendLine(" OUTPUT Inserted.Id  ");
            QueryBuilder.AppendLine(" VALUES ");
            QueryBuilder.AppendLine(" ( ");
            QueryBuilder.AppendLine("  @Id,  ");
            QueryBuilder.AppendLine("  @EstablishmentName,  ");
            QueryBuilder.AppendLine("  @ReceiptDate  ");
            QueryBuilder.AppendLine("  )  ");
            return QueryBuilder.ToString();
        }

        public static string InsertReceiptItems()
        {
            QueryBuilder.Clear();
            QueryBuilder.AppendLine($" INSERT INTO [ReceiptItem]");
            QueryBuilder.AppendLine("  (  ");
            QueryBuilder.AppendLine("  [Id], ");
            QueryBuilder.AppendLine("  [ReceiptId], ");
            QueryBuilder.AppendLine("  [CategoryId], ");
            QueryBuilder.AppendLine("  [ItemName], ");
            QueryBuilder.AppendLine("  [Quantity], ");
            QueryBuilder.AppendLine("  [ItemPrice], ");
            QueryBuilder.AppendLine("  [Observation] ");
            QueryBuilder.AppendLine("  ) ");
            QueryBuilder.AppendLine(" VALUES ");
            QueryBuilder.AppendLine(" ( ");
            QueryBuilder.AppendLine("  @Id,  ");
            QueryBuilder.AppendLine("  @ReceiptId,  ");
            QueryBuilder.AppendLine("  @CategoryId,  ");
            QueryBuilder.AppendLine("  @ItemName,  ");
            QueryBuilder.AppendLine("  @Quantity,  ");
            QueryBuilder.AppendLine("  @ItemPrice,  ");
            QueryBuilder.AppendLine("  @Observation  ");
            QueryBuilder.AppendLine("  )  ");
            return QueryBuilder.ToString();
        }
    }
}
