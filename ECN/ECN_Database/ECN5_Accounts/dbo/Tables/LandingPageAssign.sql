CREATE TABLE [dbo].[LandingPageAssign] (
    [LPAID]                   INT          IDENTITY (1, 1) NOT NULL,
    [LPID]                    INT          NULL,
    [IsDefault]               BIT          NULL,
    [BaseChannelID]           INT          NULL,
    [CustomerCanOverride]     BIT          NULL,
    [CustomerID]              INT          NULL,
    [CustomerDoesOverride]    BIT          NULL,
    [Label]                   VARCHAR (50) NULL,
    [Header]                  TEXT         NULL,
    [Footer]                  TEXT         NULL,
    [CreatedUserID]           INT          NULL,
    [CreatedDate]             DATETIME     NULL,
    [UpdatedUserID]           INT          NULL,
    [UpdatedDate]             DATETIME     NULL,
    [BaseChannelDoesOverride] BIT          NULL,
    CONSTRAINT [LandingPageAssign_PK] PRIMARY KEY CLUSTERED ([LPAID] ASC) WITH (FILLFACTOR = 80)
);

