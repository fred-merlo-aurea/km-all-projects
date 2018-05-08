CREATE TABLE [dbo].[IncomingDataDetails] (
    [cdid]           INT           IDENTITY (1, 1) NOT NULL,
    [pubid]          INT           NULL,
    [subscriptionID] INT           NULL,
    [responsegroup]  VARCHAR (100) NULL,
    [responsevalue]  VARCHAR (200) NULL,
    [NotExists]      BIT           NULL,
    CONSTRAINT [PK_IncominDataDetails] PRIMARY KEY CLUSTERED ([cdid] ASC) WITH (FILLFACTOR = 90)
);
GO
CREATE NONCLUSTERED INDEX [IDX_IncomingDataDetails_PubID]
    ON [dbo].[IncomingDataDetails]([pubid] ASC) WITH (FILLFACTOR = 70);
GO
CREATE NONCLUSTERED INDEX [IDX_IncomingDataDetails_PubID_SubscriptionID]
    ON [dbo].[IncomingDataDetails]([pubid] ASC, [subscriptionID] ASC) WITH (FILLFACTOR = 70);
GO
CREATE NONCLUSTERED INDEX [IDX_IncomingDataDetails_SubscriptionID]
    ON [dbo].[IncomingDataDetails]([subscriptionID] ASC) WITH (FILLFACTOR = 70);
GO
