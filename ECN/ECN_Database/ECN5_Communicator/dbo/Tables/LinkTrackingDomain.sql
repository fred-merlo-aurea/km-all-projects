CREATE TABLE [dbo].[LinkTrackingDomain] (
    [LinkTrackingDomainID] INT           IDENTITY (1, 1) NOT NULL,
    [LTID]                 INT           NULL,
    [CustomerID]           INT           NULL,
    [Domain]               VARCHAR (250) NULL,
    [CreatedDate]          DATETIME      NULL,
    [CreatedUserID]        INT           NULL,
    [UpdatedDate]          DATETIME      NULL,
    [UpdatedUserID]        INT           NULL,
    [IsDeleted]            BIT           NULL,
    CONSTRAINT [PK_LinkTrackingDomain] PRIMARY KEY CLUSTERED ([LinkTrackingDomainID] ASC) WITH (FILLFACTOR = 80)
);

