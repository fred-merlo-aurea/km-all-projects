CREATE TABLE [dbo].[Marketing] (
    [MarketingID]     INT          IDENTITY (1, 1) NOT NULL,
    [MarketingName]   VARCHAR (50) NOT NULL,
    [MarketingCode]   CHAR (10)    NOT NULL,
    [IsActive]        BIT          CONSTRAINT [DF_Marketing_IsActive] DEFAULT ((0)) NOT NULL,
    [DateCreated]     DATETIME     NOT NULL,
    [DateUpdated]     DATETIME     NULL,
    [CreatedByUserID] INT          NOT NULL,
    [UpdatedByUserID] INT          NULL,
    CONSTRAINT [PK_Marketing] PRIMARY KEY CLUSTERED ([MarketingCode] ASC) WITH (FILLFACTOR = 80)
);

