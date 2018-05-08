CREATE TABLE [dbo].[IssueComp]
(
	IssueCompId int identity(1,1) not null,
	IssueId int not null,
	ImportedDate datetime not null,
	IssueCompCount int not null,
	[DateCreated]     DATETIME      NOT NULL,
    [DateUpdated]     DATETIME      NULL,
    [CreatedByUserID] INT           NOT NULL,
    [UpdatedByUserID] INT           NULL, 
    [IsActive] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [PK_IssueComp] PRIMARY KEY ([IssueCompId])
)
