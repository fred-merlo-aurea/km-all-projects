CREATE TABLE [dbo].[EmailHistory] (
    [OldEmailID] INT          NOT NULL,
    [Action]     VARCHAR (25) NOT NULL,
    [NewEmailID] INT          NULL,
    [OldGroupID] INT          NULL,
    [ActionTime] DATETIME     NOT NULL
);

GO
CREATE NONCLUSTERED INDEX [IDX_EmailHistory_NewEmailID_OldEmailID] ON [dbo].[EmailHistory] 
(
	[NewEmailID] ASC,
	[OldEmailID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
