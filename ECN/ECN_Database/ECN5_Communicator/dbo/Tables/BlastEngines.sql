CREATE TABLE [dbo].[BlastEngines] (
    [BlastEngineID]     INT           IDENTITY (1, 1) NOT NULL,
    [Name]              NVARCHAR (50) NOT NULL,
    [IsActive]          BIT           NULL,
    [Counter]           INT           NULL,
    [IsDedicatedEngine] BIT           NULL,
    [IsPort25]          BIT           NULL,
    [IsSMSEngine]       BIT           NULL,
    CONSTRAINT [PK_BlastEngines] PRIMARY KEY CLUSTERED ([BlastEngineID] ASC) WITH (FILLFACTOR = 80)
);

