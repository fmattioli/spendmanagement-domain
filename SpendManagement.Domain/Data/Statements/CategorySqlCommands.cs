using System.Text;

namespace Data.Statements
{
    public static class CategorySqlCommands
    {
        private static readonly StringBuilder QueryBuilder = new();

        public static string InsertCategory()
        {
            QueryBuilder.Clear();
            QueryBuilder.AppendLine($"INSERT INTO [Category]");
            QueryBuilder.AppendLine(" (  ");
            QueryBuilder.AppendLine(" Id,  ");
            QueryBuilder.AppendLine(" Name,  ");
            QueryBuilder.AppendLine(" CreatedDate  ");
            QueryBuilder.AppendLine(" )  ");
            QueryBuilder.AppendLine(" VALUES ");
            QueryBuilder.AppendLine(" ( ");
            QueryBuilder.AppendLine("  @Id,  ");
            QueryBuilder.AppendLine("  @Name,  ");
            QueryBuilder.AppendLine("  @CreatedDate  ");
            QueryBuilder.AppendLine("  )  ");
            return QueryBuilder.ToString();
        }
    }
}
