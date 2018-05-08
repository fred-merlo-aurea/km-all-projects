CREATE TABLE [dbo].[blastsinglesbackup03252014] (
    [BlastSingleID] INT         IDENTITY (1, 1) NOT NULL,
    [BlastID]       INT         NULL,
    [EmailID]       INT         NULL,
    [SendTime]      DATETIME    NULL,
    [Processed]     VARCHAR (1) NULL,
    [LayoutPlanID]  INT         NULL,
    [refblastID]    INT         NULL,
    [CreatedDate]   DATETIME    NULL,
    [CreatedUserID] INT         NULL,
    [IsDeleted]     BIT         NULL,
    [UpdatedDate]   DATETIME    NULL,
    [UpdatedUserID] INT         NULL
);

