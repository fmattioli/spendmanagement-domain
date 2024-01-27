CREATE TABLE [dbo].[SpendManagementCommands] (
    [RoutingKey]  VARCHAR(200) NOT NULL,
    [DataCommand] DATETIME NOT NULL,
    [NameCommand] Varchar(200) NOT NULL,
    [CommandBody] NVARCHAR(MAX)         NOT NULL
);
