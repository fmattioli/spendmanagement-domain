CREATE TABLE [dbo].[Events] (
    [RoutingKey]  UNIQUEIDENTIFIER NOT NULL,
    [DataEvent] DATETIME NOT NULL,
    [NameEvent] Varchar(200) NOT NULL,
    [EventBody] NVARCHAR(MAX)         NOT NULL,
    PRIMARY KEY CLUSTERED ([RoutingKey] ASC) 
);