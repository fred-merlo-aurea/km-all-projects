CREATE TABLE [dbo].[Deliverability] (
    [DeliverabilityID]   INT          IDENTITY (1, 1) NOT NULL,
    [DeliverabilityName] VARCHAR (50) NOT NULL,
    [DeliverabilityCode] CHAR (10)    NOT NULL,
    [IsActive]           BIT          CONSTRAINT [DF_Deliverability_IsActive] DEFAULT ((0)) NOT NULL,
    [DateCreated]        DATETIME     NOT NULL,
    [DateUpdated]        DATETIME     NULL,
    [CreatedByUserID]    INT          NOT NULL,
    [UpdatedByUserID]    INT          NULL,
    CONSTRAINT [PK_Deliverability] PRIMARY KEY CLUSTERED ([DeliverabilityCode] ASC) WITH (FILLFACTOR = 80)
);

