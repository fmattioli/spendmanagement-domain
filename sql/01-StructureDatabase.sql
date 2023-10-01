IF NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'SpendManagement')
BEGIN
	CREATE DATABASE SpendManagement
END

GO

USE SpendManagement

GO
IF NOT EXISTS (SELECT * FROM sysobjects WHERE NAME='Commands' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[Commands] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [RoutingKey]  UNIQUEIDENTIFIER NOT NULL,
    [DataCommand] DATETIME NOT NULL,
    [NameCommand] Varchar(200) NOT NULL,
    [CommandBody] NVARCHAR(MAX)         NOT NULL 
);

END

GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE NAME='Events' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[Events] (
        [FK_Command_Id] INT NOT NULL,
        [RoutingKey]  UNIQUEIDENTIFIER NOT NULL,
        [DataEvent] DATETIME NOT NULL,
        [NameEvent] Varchar(200) NOT NULL,
        [EventBody] NVARCHAR(MAX)         NOT NULL,
        CONSTRAINT [FK_Events_Commands] FOREIGN KEY ([FK_Command_Id]) REFERENCES [Commands](Id) 
    );
END
GO