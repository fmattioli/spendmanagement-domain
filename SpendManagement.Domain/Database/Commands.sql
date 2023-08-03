CREATE TABLE [dbo].[Commands] (
    [RoutingKey]  UNIQUEIDENTIFIER NOT NULL,
    [DataCommand] DATETIME NOT NULL,
    [NameCommand] Varchar(200) NOT NULL,
    [CommandBody] NVARCHAR(MAX)         NOT NULL,
    PRIMARY KEY CLUSTERED ([RoutingKey] ASC) 
);
