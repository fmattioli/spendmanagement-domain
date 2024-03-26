IF NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'SpendManagement')
BEGIN
	CREATE DATABASE SpendManagement
END

GO

USE SpendManagement

GO
IF NOT EXISTS (SELECT * FROM sysobjects WHERE NAME='SpendManagementCommands' AND xtype='U')
BEGIN
   CREATE TABLE [dbo].[SpendManagementCommands] (
    [RoutingKey]  VARCHAR(200) NOT NULL,
    [DataCommand] DATETIME NOT NULL,
    [NameCommand] Varchar(200) NOT NULL,
    [CommandBody] NVARCHAR(MAX)         NOT NULL
);


END

GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE NAME='SpendManagementEvents' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[SpendManagementEvents] (
    [RoutingKey]  VARCHAR(200) NOT NULL,
    [DataEvent] DATETIME NOT NULL,
    [NameEvent] Varchar(200) NOT NULL,
    [EventBody] NVARCHAR(MAX)         NOT NULL
);
END
GO