CREATE TABLE [dbo].[Wizard_EmailRates] (
    [EmailRateID]     INT             IDENTITY (1, 1) NOT NULL,
    [BaseChannelID]   INT             NOT NULL,
    [Basefee]         DECIMAL (18, 2) NULL,
    [EmailRangeStart] INT             NULL,
    [EmailRangeEnd]   INT             NULL,
    [EmailRate]       DECIMAL (18, 4) NULL,
    CONSTRAINT [PK_Wizard_EmailRates] PRIMARY KEY CLUSTERED ([EmailRateID] ASC) WITH (FILLFACTOR = 80)
);

