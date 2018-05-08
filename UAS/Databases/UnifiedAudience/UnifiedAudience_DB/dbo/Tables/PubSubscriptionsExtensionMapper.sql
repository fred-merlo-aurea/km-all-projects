CREATE TABLE [dbo].[PubSubscriptionsExtensionMapper] (
    [PubSubscriptionsExtensionMapperID] INT           IDENTITY (1, 1) NOT NULL,
	[PubID]                             INT        NOT NULL,
    [StandardField]                  VARCHAR (255) NOT NULL,
    [CustomField]                    VARCHAR (255) NOT NULL,
    [CustomFieldDataType]            VARCHAR (25)  NOT NULL,
    [Active]                         BIT           CONSTRAINT [DF_PubSubscriptionsExtensionMapper_Active] DEFAULT ((1)) NOT NULL,
    [DateCreated]                    DATETIME      CONSTRAINT [DF_PubSubscriptionsExtensionMapper_DateCreated] DEFAULT (getdate()) NULL,
    [DateUpdated]                    DATETIME      NULL,
    [CreatedByUserID]                INT           NULL,
    [UpdatedByUserID]                INT           NULL,
    CONSTRAINT [PK_PubSubscriptionExtentionMap] PRIMARY KEY CLUSTERED ([PubSubscriptionsExtensionMapperID] ASC) WITH (FILLFACTOR = 90)
);