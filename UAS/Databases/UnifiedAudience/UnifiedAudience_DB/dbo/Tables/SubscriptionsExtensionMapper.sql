CREATE TABLE [dbo].[SubscriptionsExtensionMapper] (
    [SubscriptionsExtensionMapperID] INT           IDENTITY (1, 1) NOT NULL,
    [StandardField]                  VARCHAR (255) NOT NULL,
    [CustomField]                    VARCHAR (255) NOT NULL,
    [CustomFieldDataType]            VARCHAR (25)  NOT NULL,
    [Active]                         BIT           CONSTRAINT [DF_SubscriptionsExtensionMapper_Active] DEFAULT ((1)) NOT NULL,
    [DateCreated]                    DATETIME      CONSTRAINT [DF_SubscriptionsExtensionMapper_DateCreated] DEFAULT (getdate()) NULL,
    [DateUpdated]                    DATETIME      NULL,
    [CreatedByUserID]                INT           NULL,
    [UpdatedByUserID]                INT           NULL,
    CONSTRAINT [PK_SubscriptionExtentionMap] PRIMARY KEY CLUSTERED ([SubscriptionsExtensionMapperID] ASC) WITH (FILLFACTOR = 90)
);



