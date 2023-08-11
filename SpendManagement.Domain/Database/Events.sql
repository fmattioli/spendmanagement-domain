CREATE TABLE [dbo].[Events] (
    [FK_Command_Id] INT NOT NULL,
    [RoutingKey]  UNIQUEIDENTIFIER NOT NULL,
    [DataEvent] DATETIME NOT NULL,
    [NameEvent] Varchar(200) NOT NULL,
    [EventBody] NVARCHAR(MAX)         NOT NULL, 
    CONSTRAINT [FK_Events_Commands] FOREIGN KEY ([FK_Command_Id]) REFERENCES [Commands](Id) 
);