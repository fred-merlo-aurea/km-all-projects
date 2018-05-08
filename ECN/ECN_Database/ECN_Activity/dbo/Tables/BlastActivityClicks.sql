CREATE TABLE [dbo].[BlastActivityClicks] (
    [ClickID]      INT            IDENTITY (1, 1) NOT NULL,
    [BlastID]      INT            NOT NULL,
    [EmailID]      INT            NOT NULL,
    [ClickTime]    DATETIME       NOT NULL,
    [URL]          VARCHAR (2048) NULL,
    [BlastLinkID]  INT            NULL,
    [EAID]         INT            NULL,
    [UniqueLinkID] INT            NULL,
    [RoundedTime]  DATETIME       NULL,
    CONSTRAINT [PK_BlastActivityClicks] PRIMARY KEY CLUSTERED ([ClickID] ASC) WITH (FILLFACTOR = 90) ON [ECN_Activity_Data_1]
);




GO
CREATE NONCLUSTERED INDEX [IDX_BlastActivityClicks_blastID_2]
    ON [dbo].[BlastActivityClicks]([BlastID] ASC)
    INCLUDE([EmailID], [URL]) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IDX_BlastActivityClicks_EmailID]
    ON [dbo].[BlastActivityClicks]([EmailID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IDX_BlastActivityClicks_BlastEmailRoundTime]
    ON [dbo].[BlastActivityClicks]([BlastID] ASC, [EmailID] ASC, [RoundedTime] ASC) WITH (IGNORE_DUP_KEY = ON)
    ON [ECN_Activity_Index];

