CREATE TABLE [dbo].[AdventurePublishing_Transactions] (
    [AdvendurePublishingTransactionId] INT           IDENTITY (1, 1) NOT NULL,
    [EmailID]                          INT           NOT NULL,
    [OrderNumber]                      VARCHAR (50)  NULL,
    [OrderAmount]                      FLOAT (53)    NULL,
    [OrderStatus]                      VARCHAR (50)  NULL,
    [OrderPublications]                VARCHAR (255) NULL,
    [OrderReferenceCode]               VARCHAR (255) NULL,
    [CreatedOn]                        DATETIME      NULL,
    [ReasonCode]                       VARCHAR (100) NULL
);

