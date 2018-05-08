CREATE TABLE [dbo].[DomainSuppression] (
    [DomainSuppressionID] INT           IDENTITY (1, 1) NOT NULL,
    [BaseChannelID]       INT           NULL,
    [CustomerID]          INT           NULL,
    [Domain]              VARCHAR (100) NOT NULL,
    [IsActive]            BIT           CONSTRAINT [DF_DomainSuppression_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedDate]         DATETIME      CONSTRAINT [DF_DomainSuppression_CREATEDDATE] DEFAULT (getdate()) NULL,
    [CreatedUserID]       INT           NULL,
    [IsDeleted]           BIT           CONSTRAINT [DF_DomainSuppression_IsDeleted] DEFAULT ((0)) NULL,
    [UpdatedDate]         DATETIME      NULL,
    [UpdatedUserID]       INT           NULL,
    CONSTRAINT [PK_DomainSuppression] PRIMARY KEY CLUSTERED ([DomainSuppressionID] ASC) WITH (FILLFACTOR = 100)
);

