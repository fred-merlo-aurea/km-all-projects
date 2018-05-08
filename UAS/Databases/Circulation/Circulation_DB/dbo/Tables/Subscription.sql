CREATE TABLE [dbo].[Subscription] (
    [SubscriptionID]                 INT          IDENTITY (1, 1) NOT NULL,
    [SequenceID]                     INT          NULL,
    [PublisherID]                    INT          NOT NULL,
    [SubscriberID]                   INT          NOT NULL,
    [PublicationID]                  INT          NOT NULL,
    [ActionID_Current]               INT          NOT NULL,
    [ActionID_Previous]              INT          NULL,
    [SubscriptionStatusID]           INT          NULL,
    [IsPaid]                         BIT          NOT NULL,
    [QSourceID]                      INT          NULL,
    [QSourceDate]                    DATE         NULL,
    [DeliverabilityID]               INT          NULL,
    [IsSubscribed]                   BIT          CONSTRAINT [DF_Subscription_IsSubscribed] DEFAULT ((1)) NOT NULL,
    [SubscriberSourceCode]           varchar(256) NULL,
    [Copies]                         INT          CONSTRAINT [DF_Subscription_Copies] DEFAULT ((1)) NOT NULL,
    [OriginalSubscriberSourceCode]   varchar(256) NULL,
    [DateCreated]                    DATETIME     NOT NULL,
    [DateUpdated]                    DATETIME     NULL,
    [CreatedByUserID]                INT          NOT NULL,
    [UpdatedByUserID]                INT          NULL,
    [Par3cID]                        INT          NULL,
    [SubsrcTypeID]                   INT          NULL,
    [AccountNumber]                  VARCHAR (50) NULL,
	GraceIssues						 INT		  NULL,
    [OnBehalfOf]					 VARCHAR(256) NULL, 
    [MemberGroup]					 VARCHAR(256) NULL, 
	[Verify]						 VARCHAR(256) NULL, 
	IsNewSubscription				 bit		  NULL,
    [AddRemoveID]					 INT		  NULL, 
    [IMBSeq]						 VARCHAR(10)  NULL, 
	[IsActive]						 BIT		  NULL
    CONSTRAINT [PK_Subscription] PRIMARY KEY CLUSTERED ([SubscriberID] ASC, [PublicationID] ASC) WITH (FILLFACTOR = 80)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IDX_SubscriptionID]
    ON [dbo].[Subscription]([SubscriptionID] ASC) WITH (FILLFACTOR = 80);

