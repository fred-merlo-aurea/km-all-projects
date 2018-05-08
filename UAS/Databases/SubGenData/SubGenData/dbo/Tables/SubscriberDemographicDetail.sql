CREATE TABLE [dbo].[SubscriberDemographicDetail]
(
	[subscriber_id] INT         NOT NULL,
	[option_id]    INT          NOT NULL,
    [field_id]     INT          NULL,
    [account_id]   INT          NOT NULL,
    [value]        VARCHAR (255) NULL,
	DateCreated		DATETIME	  NOT NULL,
	[IsProcessed]		 BIT DEFAULT('false') NOT NULL,
	[ProcessedDate]		 DATETIME	  NULL
)
