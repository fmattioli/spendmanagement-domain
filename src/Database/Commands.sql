CREATE TABLE [dbo].[Commands] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [RoutingKey]  UNIQUEIDENTIFIER NOT NULL,
    [DataCommand] DATETIME NOT NULL,
    [NameCommand] Varchar(200) NOT NULL,
    [CommandBody] NVARCHAR(MAX)         NOT NULL 
);
