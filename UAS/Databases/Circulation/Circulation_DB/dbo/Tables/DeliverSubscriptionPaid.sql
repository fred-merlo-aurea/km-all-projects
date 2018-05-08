CREATE TABLE [dbo].[DeliverSubscriptionPaid] (
    [DeliverID]       INT           IDENTITY (1, 1) NOT NULL,
    [DeliverName]     VARCHAR (100) NOT NULL,
    [DeliverCode]     VARCHAR (10)  NOT NULL,
    [IsActive]        BIT           NOT NULL,
    [DateCreated]     DATETIME      NOT NULL,
    [DateUpdated]     DATETIME      NULL,
    [CreatedByUserID] INT           NOT NULL,
    [UpdatedByUserID] INT           NULL,
    CONSTRAINT [pk_DeliverCode] PRIMARY KEY CLUSTERED ([DeliverCode] ASC)
);

