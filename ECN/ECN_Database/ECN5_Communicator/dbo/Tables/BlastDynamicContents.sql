CREATE TABLE [dbo].[BlastDynamicContents] (
    [ID]      INT IDENTITY (1, 1) NOT NULL,
    [BlastID] INT NULL,
    [EmailID] INT NULL,
    [Slot1]   INT NULL,
    [Slot2]   INT NULL,
    [Slot3]   INT NULL,
    [Slot4]   INT NULL,
    [Slot5]   INT NULL,
    [Slot6]   INT NULL,
    [Slot7]   INT NULL,
    [Slot8]   INT NULL,
    [Slot9]   INT NULL,
    CONSTRAINT [PK_BlastDynamicContents] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 80)
);

