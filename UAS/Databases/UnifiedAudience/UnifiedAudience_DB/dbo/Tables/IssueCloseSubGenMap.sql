CREATE TABLE [dbo].[IssueCloseSubGenMap]
(
	[IssueID]             INT           IDENTITY (1, 1) NOT NULL,
    [PubSubscriptionID]	  INT			NOT NULL,
    [SubGenSubscriberID]  INT           NOT NULL
	CONSTRAINT [PK_IssueCloseSubGenMap] PRIMARY KEY CLUSTERED ([IssueID] ASC) WITH (FILLFACTOR = 90)
)
GO

CREATE NONCLUSTERED INDEX [IX_IssueCloseSubGenMap_PubSubscriptionID]
    ON [dbo].[IssueCloseSubGenMap]([PubSubscriptionID] ASC);
GO