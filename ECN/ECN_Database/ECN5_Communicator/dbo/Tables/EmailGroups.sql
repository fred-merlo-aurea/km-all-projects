CREATE TABLE [dbo].[EmailGroups] (
    [EmailGroupID]      INT          IDENTITY (1, 1) NOT NULL,
    [EmailID]           INT          NOT NULL,
    [GroupID]           INT          NOT NULL,
    [FormatTypeCode]    VARCHAR (5)  NOT NULL,
    [SubscribeTypeCode] VARCHAR (50) NULL,
    [CreatedOn]         DATETIME     NULL,
    [LastChanged]       DATETIME     NULL,
    [SMSEnabled]        BIT          NULL,
    [CreatedSource] VARCHAR (200) NULL,
    [LastChangedSource] VARCHAR (200) NULL,
    CONSTRAINT [PK_EmailGroups] PRIMARY KEY NONCLUSTERED ([EmailGroupID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [UQ_EmailGroups] UNIQUE CLUSTERED ([GroupID] ASC, [EmailID] ASC) WITH (FILLFACTOR = 80)
);




GO
CREATE NONCLUSTERED INDEX [IX_GroupID]
    ON [dbo].[EmailGroups]([GroupID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_EmailGroups_EmailID]
    ON [dbo].[EmailGroups]([EmailID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_EmailGroups_GroupID_SubscribeTypeCode]
    ON [dbo].[EmailGroups]([GroupID] ASC, [SubscribeTypeCode] ASC) WITH (FILLFACTOR = 80);


GO
GRANT DELETE
    ON OBJECT::[dbo].[EmailGroups] TO [ecn5writer]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[EmailGroups] TO [ecn5writer]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[EmailGroups] TO [ecn5writer]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[EmailGroups] TO [ecn5writer]
    AS [dbo];


GO



GO



GO



GO
GRANT SELECT
    ON OBJECT::[dbo].[EmailGroups] TO [reader]
    AS [dbo];

