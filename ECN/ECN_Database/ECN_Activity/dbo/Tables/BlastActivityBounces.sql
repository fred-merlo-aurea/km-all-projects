CREATE TABLE [dbo].[BlastActivityBounces] (
    [BounceID]      INT           IDENTITY (1, 1) NOT NULL,
    [BlastID]       INT           NOT NULL,
    [EmailID]       INT           NOT NULL,
    [BounceTime]    DATETIME      NOT NULL,
    [BounceCodeID]  INT           NOT NULL,
    [BounceMessage] VARCHAR (355) NULL,
    [EAID]          INT           NULL,
    CONSTRAINT [PK_BlastActivityBounces] PRIMARY KEY CLUSTERED ([BounceID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_BlastActivityBounces_BounceCodes] FOREIGN KEY ([BounceCodeID]) REFERENCES [dbo].[BounceCodes] ([BounceCodeID])
);


GO
CREATE NONCLUSTERED INDEX [IDX_BlastActivityBounces_BlastID_2]
    ON [dbo].[BlastActivityBounces]([BlastID] ASC)
    INCLUDE([BounceID], [EmailID], [BounceCodeID]) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IDX_BlastActivityBounces_EmailID]
    ON [dbo].[BlastActivityBounces]([EmailID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_BlastActivityBounces_BlastID_BounceCodeID]
    ON [dbo].[BlastActivityBounces]([BlastID] ASC, [BounceCodeID] ASC)
    INCLUDE([BounceID], [EmailID]) WITH (FILLFACTOR = 80);

