CREATE TABLE [dbo].[BlastSingles] (
    [BlastSingleID] INT         IDENTITY (1, 1) NOT NULL,
    [BlastID]       INT         NULL,
    [EmailID]       INT         NULL,
    [SendTime]      DATETIME    NULL,
    [Processed]     VARCHAR (1) CONSTRAINT [DF_BlastSingles_Processed] DEFAULT ('n') NULL,
    [LayoutPlanID]  INT         NULL,
    [refblastID]    INT         NULL,
    [CreatedDate]   DATETIME    CONSTRAINT [DF_BlastSingles_CREATEDDATE] DEFAULT (getdate()) NULL,
    [CreatedUserID] INT         NULL,
    [IsDeleted]     BIT         CONSTRAINT [DF_BlastSingles_IsDeleted] DEFAULT ((0)) NULL,
    [UpdatedDate]   DATETIME    NULL,
    [UpdatedUserID] INT         NULL,
    [StartTime] DATETIME NULL, 
    [EndTime] DATETIME NULL, 
    CONSTRAINT [PK_BlastSingles] PRIMARY KEY CLUSTERED ([BlastSingleID] ASC) WITH (FILLFACTOR = 80)
);




GO
CREATE NONCLUSTERED INDEX [IX_BlastSingles_EmailID]
    ON [dbo].[BlastSingles]([EmailID] ASC) WITH (FILLFACTOR = 80);


GO
GRANT DELETE
    ON OBJECT::[dbo].[BlastSingles] TO [ecn5writer]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[BlastSingles] TO [ecn5writer]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[BlastSingles] TO [ecn5writer]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[BlastSingles] TO [ecn5writer]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[BlastSingles] TO [reader]
    AS [dbo];


GO
CREATE NONCLUSTERED INDEX [IX_BlastSingles_ProcessedSendTime]
    ON [dbo].[BlastSingles]([Processed] ASC, [SendTime] ASC);

