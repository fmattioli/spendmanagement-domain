CREATE TABLE [dbo].[ReceiptItem] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [ReceiptId] UNIQUEIDENTIFIER NOT NULL,
    [ItemName]      VARCHAR (200)    NOT NULL,
    [Quantity]      SMALLINT         NOT NULL,
    [ItemPrice]     MONEY            NOT NULL,
    [Observation]   VARCHAR (200)    NULL,
    [TotalPrice]    AS               ([ItemPrice]*[Quantity]),
    FOREIGN KEY ([ReceiptId]) REFERENCES [dbo].[Receipt]([Id])
);