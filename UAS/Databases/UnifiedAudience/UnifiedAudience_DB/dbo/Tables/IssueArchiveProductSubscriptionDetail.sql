CREATE TABLE [dbo].[IssueArchiveProductSubscriptionDetail]
(
	[IAProductSubscriptionDetailID] int identity(1,1) not null,
	[IssueArchiveSubscriptionId] INT   NULL,
    [PubSubscriptionID]       INT      NULL,
    [SubscriptionID]          INT      NULL,
    [CodesheetID]             INT      NULL,
    [DateCreated]             DATETIME NULL DEFAULT (getdate()),
    [DateUpdated]             DATETIME NULL,
    [CreatedByUserID]         INT      NULL,
    [UpdatedByUserID]         INT      NULL,
	[ResponseOther]			  VARCHAR(256) NULL,
    CONSTRAINT [PK_IssueArchiveSubscriptonResponseMap] PRIMARY KEY CLUSTERED ([IAProductSubscriptionDetailID] ASC) WITH (FILLFACTOR = 90)
);
GO

CREATE NONCLUSTERED INDEX [IDX_IssueArchivePubSubscriptionDetail_IssueArchiveSubscriptionID]
    ON [dbo].[IssueArchiveProductSubscriptionDetail]([IssueArchiveSubscriptionId] ASC) WITH (FILLFACTOR = 90);
GO

CREATE NONCLUSTERED INDEX [IDX_IssueArchivePubSubscriptionDetail_PubSubscriptionID_CodesheetID]
    ON [dbo].[IssueArchiveProductSubscriptionDetail]([PubSubscriptionID] ASC, [CodesheetID] ASC) WITH (FILLFACTOR = 90);
GO

CREATE NONCLUSTERED INDEX [IX_IssueArchiveProductSubscriptionDetail_PubSubscriptionID]
    ON [dbo].[IssueArchiveProductSubscriptionDetail]([PubSubscriptionID] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_IssueArchiveProductSubscriptionDetail_SubscriptionID]
    ON [dbo].[IssueArchiveProductSubscriptionDetail]([SubscriptionID] ASC);
GO

