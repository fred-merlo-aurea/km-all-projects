CREATE TABLE [dbo].[BlastActivityRefer] (
    [ReferID]      INT           IDENTITY (1, 1) NOT NULL,
    [BlastID]      INT           NOT NULL,
    [EmailID]      INT           NOT NULL,
    [ReferTime]    DATETIME      NOT NULL,
    [EmailAddress] VARCHAR (255) NULL,
    [EAID]         INT           NULL,
    CONSTRAINT [PK_BlastActivityRefer] PRIMARY KEY CLUSTERED ([ReferID] ASC) WITH (FILLFACTOR = 80)
);


GO
CREATE NONCLUSTERED INDEX [IDX_BlastActivityRefer_BlastID]
    ON [dbo].[BlastActivityRefer]([BlastID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IDX_BlastActivityRefer_EmailID]
    ON [dbo].[BlastActivityRefer]([EmailID] ASC) WITH (FILLFACTOR = 80);

