CREATE TABLE [dbo].[BlastActivityUnSubscribes] (
    [UnsubscribeID]     INT           IDENTITY (1, 1) NOT NULL,
    [BlastID]           INT           NOT NULL,
    [EmailID]           INT           NOT NULL,
    [UnsubscribeTime]   DATETIME      NOT NULL,
    [UnsubscribeCodeID] INT           NOT NULL,
    [Comments]          VARCHAR (355) NULL,
    [EAID]              INT           NULL,
    CONSTRAINT [PK_BlastActivityUnSubscribes] PRIMARY KEY CLUSTERED ([UnsubscribeID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_BlastActivityUnSubscribes_UnsubscribeCodes] FOREIGN KEY ([UnsubscribeCodeID]) REFERENCES [dbo].[UnsubscribeCodes] ([UnsubscribeCodeID])
);


GO
CREATE NONCLUSTERED INDEX [IDX_BlastActivityUnSubscribes_BlastID]
    ON [dbo].[BlastActivityUnSubscribes]([BlastID] ASC)
    INCLUDE([UnsubscribeID], [EmailID], [UnsubscribeCodeID]) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IDX_BlastActivityUnsubscribes_EmailID]
    ON [dbo].[BlastActivityUnSubscribes]([EmailID] ASC) WITH (FILLFACTOR = 80);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Mastersuppression, Unsubscribe, FBL, Abuse', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BlastActivityUnSubscribes', @level2type = N'COLUMN', @level2name = N'UnsubscribeCodeID';

