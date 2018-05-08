CREATE TABLE [dbo].[BlastRules] (
    [ID]                INT          IDENTITY (1, 1) NOT NULL,
    [NodeID]            VARCHAR (20) NULL,
    [Input_BlastID]     INT          NULL,
    [RuleType]          VARCHAR (50) NULL,
    [Operator]          NCHAR (10)   NULL,
    [Value]             VARCHAR (50) NULL,
    [Yes_BlastID]       INT          NULL,
    [No_BlastID]        INT          NULL,
    [TriggerTime]       DATE         NULL,
    [DripCampaignID]    INT          NULL,
    [DripRulesEngineID] INT          NULL,
    CONSTRAINT [PK_BlastRules] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 80)
);

