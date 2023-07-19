CREATE TABLE [dbo].[Receipt] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [EstablishmentName] VARCHAR (200)    NOT NULL,
    [ReceiptDate]       DATETIME         NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC) 
);
