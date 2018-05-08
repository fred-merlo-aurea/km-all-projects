CREATE TABLE [dbo].[SubscriberDemographic]
(
	[subscriber_id] INT           NOT NULL,
    [account_id]    INT           NOT NULL,
	[field_id]		INT			  NOT NULL,
	[text_value]	VARCHAR (255) NULL,
	DateCreated		DATETIME	  NOT NULL,
	[IsProcessed]		 BIT DEFAULT('false') NOT NULL,
	[ProcessedDate]		 DATETIME	  NULL
)
