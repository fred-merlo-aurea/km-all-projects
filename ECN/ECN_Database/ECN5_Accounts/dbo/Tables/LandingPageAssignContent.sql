CREATE TABLE [dbo].[LandingPageAssignContent] (
    [LPACID]        INT           IDENTITY (1, 1) NOT NULL,
    [LPAID]         INT           NULL,
    [LPOID]         INT           NULL,
    [Display]       VARCHAR (MAX) NULL,
    [CreatedUserID] INT           NULL,
    [CreatedDate]   DATETIME      NULL,
    [UpdatedUserID] INT           NULL,
    [UpdatedDate]   DATETIME      NULL,
    [IsDeleted]     BIT           NULL,
    [SortOrder] INT NULL, 
    CONSTRAINT [LandingPageAssignContent_PK] PRIMARY KEY CLUSTERED ([LPACID] ASC) WITH (FILLFACTOR = 80)
);

