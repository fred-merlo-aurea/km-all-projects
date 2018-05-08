CREATE TABLE [dbo].[SubscriberSourceCode] (
    [SubscriberSourceCodeID]    INT       IDENTITY (1, 1) NOT NULL,
    [PublicationID]             INT       NOT NULL,
    [SubscriberSourceTypeID]    INT       NOT NULL,
    [SubscriberSourceCodeValue] CHAR (25) NOT NULL,
    [IsActive]                  BIT       NOT NULL,
    [DateCreated]               DATETIME  NOT NULL,
    [DateUpdated]               DATETIME  NULL,
    [CreatedByUserID]           INT       NOT NULL,
    [UpdatedByUserID]           INT       NULL,
    CONSTRAINT [PK_SubSourceCode] PRIMARY KEY CLUSTERED ([PublicationID] ASC, [SubscriberSourceTypeID] ASC, [SubscriberSourceCodeValue] ASC) WITH (FILLFACTOR = 80)
);

