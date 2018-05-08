CREATE TABLE [dbo].[BlastActivitySends] (
    [SendID]      INT           IDENTITY (1, 1) NOT NULL,
    [BlastID]     INT           NOT NULL,
    [EmailID]     INT           NOT NULL,
    [SendTime]    DATETIME      NOT NULL,
    [IsOpened]    BIT           NULL,
    [IsClicked]   BIT           NULL,
    [SMTPMessage] VARCHAR (255) NULL,
    [IsResend]    BIT           CONSTRAINT [DF_BlastActivitySends_IsResend] DEFAULT ((0)) NOT NULL,
    [EAID]        INT           NULL,
    [SourceIP]    VARCHAR (50)  NULL,
    CONSTRAINT [PK_BlastActivitySends] PRIMARY KEY CLUSTERED ([SendID] ASC) WITH (FILLFACTOR = 80)
);


GO
CREATE NONCLUSTERED INDEX [IDX_BlastActivitySends_EmailID]
    ON [dbo].[BlastActivitySends]([EmailID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IDX_BlastActivitySends_BlastID_EmailID]
    ON [dbo].[BlastActivitySends]([BlastID] ASC)
    INCLUDE([EmailID]) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IDX_BlastActivitySends_BlastID_EmailID_Composite]
    ON [dbo].[BlastActivitySends]([BlastID] ASC, [EmailID] ASC) WITH (FILLFACTOR = 80);

