CREATE TABLE [dbo].[GlobalMasterSuppressionList] (
    [GSID]         INT           IDENTITY (1, 1) NOT NULL,
    [EmailAddress]  VARCHAR (100) NOT NULL,
    [CreatedDate]   DATETIME      CONSTRAINT [DF_GlobalMasterSuppressionList_DateAdded] DEFAULT (getdate()) NOT NULL,
    [CreatedUserID] INT           NULL,
    [UpdatedUserID] INT           NULL,
    [UpdatedDate]   DATETIME      NULL,
    [IsDeleted]     BIT           CONSTRAINT [DF_GlobalMasterSuppressionList_IsDeleted] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_GlobalMasterSuppressionList] PRIMARY KEY CLUSTERED ([GSID] ASC) WITH (FILLFACTOR = 80)
);


GO
CREATE NONCLUSTERED INDEX [EmailAddress]
    ON [dbo].[GlobalMasterSuppressionList]([EmailAddress] ASC) WITH (FILLFACTOR = 80);

