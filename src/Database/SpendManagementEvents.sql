CREATE TABLE [dbo].[SpendManagementEvents] (
    [FK_Command_Id] INT NOT NULL,
    [RoutingKey]  VARCHAR(200) NOT NULL,
    [DataEvent] DATETIME NOT NULL,
    [NameEvent] Varchar(200) NOT NULL,
    [EventBody] NVARCHAR(MAX)         NOT NULL,
    CONSTRAINT [FK_SpendManagementEvents_Commands] FOREIGN KEY ([FK_Command_Id]) REFERENCES [SpendManagementCommands](Id)
);