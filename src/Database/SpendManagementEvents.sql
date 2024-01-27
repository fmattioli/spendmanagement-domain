CREATE TABLE [dbo].[SpendManagementEvents] (
    [RoutingKey]  VARCHAR(200) NOT NULL,
    [DataEvent] DATETIME NOT NULL,
    [NameEvent] Varchar(200) NOT NULL,
    [EventBody] NVARCHAR(MAX)         NOT NULL
);