CREATE TABLE [dbo].[BlastActivitySocial] (
    [SocialID]             INT            IDENTITY (1, 1) NOT NULL,
    [BlastID]              INT            NOT NULL,
    [EmailID]              INT            NULL,
    [RefEmailID]           INT            NULL,
    [SocialActivityCodeID] INT            NOT NULL,
    [ActionDate]           DATETIME       NOT NULL,
    [URL]                  VARCHAR (2048) NULL,
    [SocialMediaID]        INT            NOT NULL,
    CONSTRAINT [PK_BlastActivitySocial] PRIMARY KEY CLUSTERED ([SocialID] ASC) WITH (FILLFACTOR = 80)
);

GO

CREATE NONCLUSTERED INDEX [IDX_BlastActivitySocial_BlastID] ON [dbo].[BlastActivitySocial] 
(
	[BlastID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


