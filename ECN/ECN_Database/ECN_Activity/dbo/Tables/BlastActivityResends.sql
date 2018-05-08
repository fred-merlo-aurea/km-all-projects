CREATE TABLE [dbo].[BlastActivityResends] (
    [ResendID]   INT      IDENTITY (1, 1) NOT NULL,
    [BlastID]    INT      NOT NULL,
    [EmailID]    INT      NOT NULL,
    [ResendTime] DATETIME NOT NULL,
    [EAID]       INT      NULL,
    CONSTRAINT [PK_BlastActivityResends] PRIMARY KEY CLUSTERED ([ResendID] ASC) WITH (FILLFACTOR = 80)
);


GO
CREATE NONCLUSTERED INDEX [IDX_BlastActivityResends_BlastID]
    ON [dbo].[BlastActivityResends]([BlastID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IDX_BlastActivityResends_EmailID]
    ON [dbo].[BlastActivityResends]([EmailID] ASC) WITH (FILLFACTOR = 80);

