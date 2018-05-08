CREATE TABLE [dbo].[pricecode_09182014] (
    [PriceCodeID]     INT             IDENTITY (1, 1) NOT NULL,
    [Code]            VARCHAR (50)    NOT NULL,
    [Price]           DECIMAL (18, 2) NOT NULL,
    [PublicationID]   INT             NOT NULL,
    [Term]            INT             NOT NULL,
    [IsActive]        BIT             NOT NULL,
    [DateCreated]     DATETIME        NOT NULL,
    [DateUpdated]     DATETIME        NULL,
    [CreatedByUserID] INT             NOT NULL,
    [UpdatedByUserID] INT             NULL,
    [NumberOfIssues]  INT             NULL
);

