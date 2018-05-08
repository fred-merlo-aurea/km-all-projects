CREATE TABLE [dbo].[BlastGrouping] (
    [BlastGroupID] INT            IDENTITY (1, 1) NOT NULL,
    [BlastIDs]     VARCHAR (7999) NULL,
    [EmailSubject] VARCHAR (255)  NULL,
    [UserID]       INT            NULL,
    [DateAdded]    DATETIME       NULL,
    CONSTRAINT [PK_BlastGrouping] PRIMARY KEY CLUSTERED ([BlastGroupID] ASC) WITH (FILLFACTOR = 80)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'list of blasts that were sent for in this batch as a group (separated by comma '','')', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BlastGrouping', @level2type = N'COLUMN', @level2name = N'BlastIDs';

