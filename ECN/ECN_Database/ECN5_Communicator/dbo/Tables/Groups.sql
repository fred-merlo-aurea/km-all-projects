CREATE TABLE [dbo].[Groups] (
    [GroupID]          INT           IDENTITY (1, 1) NOT NULL,
    [CustomerID]       INT           NOT NULL,
    [FolderID]         INT           NULL,
    [GroupName]        VARCHAR (50)  NOT NULL,
    [GroupDescription] VARCHAR (500) CONSTRAINT [DF_Groups_GroupDescription] DEFAULT ('') NULL,
    [OwnerTypeCode]    VARCHAR (50)  NULL,
    [MasterSupression] INT           NULL,
    [PublicFolder]     INT           NULL,
    [OptinHTML]        TEXT          CONSTRAINT [DF_Groups_OptinHTML] DEFAULT ('') NULL,
    [OptinFields]      TEXT          CONSTRAINT [DF_Groups_OptinFields] DEFAULT ('EmailAddress|SubscribeTypeCode|FormatTypeCode|GroupID|CustomerID|') NULL,
    [AllowUDFHistory]  CHAR (1)      CONSTRAINT [DF_Groups_AllowUDFHistory] DEFAULT ('N') NULL,
    [IsSeedList]       BIT           CONSTRAINT [DF_Groups_IsSeedList] DEFAULT (0) NULL,
    [CreatedDate]      DATETIME      CONSTRAINT [DF_Groups_CREATEDDATE] DEFAULT (getdate()) NULL,
    [CreatedUserID]    INT           NULL,
    [UpdatedDate]      DATETIME      NULL,
    [UpdatedUserID]    INT           NULL,
    [Archived] BIT NULL, 
    CONSTRAINT [PK_Groups] PRIMARY KEY CLUSTERED ([GroupID] ASC) WITH (FILLFACTOR = 80)
);


GO
CREATE NONCLUSTERED INDEX [IX_CustomerID]
    ON [dbo].[Groups]([CustomerID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_GroupName]
    ON [dbo].[Groups]([GroupName] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IDX_Groups_CustomerID_MasterSupression_GroupID]
    ON [dbo].[Groups]([CustomerID] ASC, [MasterSupression] ASC, [GroupID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE STATISTICS [_dta_stat_1099866985_1_2]
    ON [dbo].[Groups]([GroupID], [CustomerID]);


GO
CREATE STATISTICS [_dta_stat_1099866985_11_1_2]
    ON [dbo].[Groups]([AllowUDFHistory], [GroupID], [CustomerID]);


GO
GRANT DELETE
    ON OBJECT::[dbo].[Groups] TO [ecn5writer]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[Groups] TO [ecn5writer]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Groups] TO [ecn5writer]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[Groups] TO [ecn5writer]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Groups] TO [reader]
    AS [dbo];

