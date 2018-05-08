CREATE TABLE [dbo].[Suffix] (
    [SuffixID]         INT           IDENTITY (1, 1) NOT NULL,
    [SuffixCodeTypeID] INT           NOT NULL,
    [SuffixName]       VARCHAR (100) NULL,
    [IsActive]         BIT           NULL,
    [DateCreated]      DATETIME      NULL,
    [DateUpdated]      DATETIME      NULL,
    [CreatedByUserID]  INT           NULL,
    [UpdatedByUserID]  INT           NULL,
    CONSTRAINT [PK_Suffix] PRIMARY KEY CLUSTERED ([SuffixID] ASC)
);


