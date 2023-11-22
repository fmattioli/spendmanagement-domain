CREATE TABLE [dbo].[SpendManagementCommands] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [RoutingKey]  VARCHAR(200) NOT NULL,
    [DataCommand] DATETIME NOT NULL,
    [NameCommand] Varchar(200) NOT NULL,
    [CommandBody] NVARCHAR(MAX)         NOT NULL
);
