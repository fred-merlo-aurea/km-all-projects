CREATE TABLE [dbo].[IssueArchiveSubscriptonResponseMap]
(
	IssueArchiveSubscriptionId int identity(1,1) not null,
	[SubscriptionID]  INT           NOT NULL,
    [ResponseID]      INT           NOT NULL,
    [IsActive]        BIT           NOT NULL,
	[ResponseOther]   VARCHAR (300) NULL,
    [DateCreated]     DATETIME      NOT NULL,
    [DateUpdated]     DATETIME      NULL,
    [CreatedByUserID] INT           NOT NULL,
    [UpdatedByUserID] INT           NULL, 
    CONSTRAINT [PK_IssueArchiveSubscriptonResponseMap] PRIMARY KEY ([IssueArchiveSubscriptionId])
)
