CREATE TABLE [dbo].[Tmp_EmailGroupSync] (
    [EmailId]      INT           NULL,
    [EmailAddress] VARCHAR (255) NULL,
    [GroupID]      INT           NULL
);




GO
CREATE NONCLUSTERED INDEX [IDX_Tmp_EmailGroupSync_email]
    ON [dbo].[Tmp_EmailGroupSync]([GroupID] ASC, [EmailAddress] ASC);



