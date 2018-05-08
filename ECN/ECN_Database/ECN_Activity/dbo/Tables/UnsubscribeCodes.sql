CREATE TABLE [dbo].[UnsubscribeCodes] (
    [UnsubscribeCodeID] INT          IDENTITY (1, 1) NOT NULL,
    [UnsubscribeCode]   VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_UnsubscribeCodes] PRIMARY KEY CLUSTERED ([UnsubscribeCodeID] ASC) WITH (FILLFACTOR = 80)
);

