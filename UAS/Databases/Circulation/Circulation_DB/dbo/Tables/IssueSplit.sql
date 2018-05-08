CREATE TABLE [dbo].[IssueSplit]
(
	IssueSplitId int identity(1,1) not null,
	IssueId int not null,
	IssueSplitCode varchar(250) not null,
	IssueSplitName varchar(250) not null,
	IssueSplitCount int not null,
	FilterId int null,
	[DateCreated]              		 DATETIME     NOT NULL,
    [DateUpdated]              		 DATETIME     NULL,
    [CreatedByUserID]          		 INT          NOT NULL,
    [UpdatedByUserID]          		 INT          NULL, 
    [IsActive] BIT NOT NULL DEFAULT 1, 
    [KeyCode] VARCHAR(250) NULL, 
    CONSTRAINT [PK_IssueSplit] PRIMARY KEY ([IssueSplitId])
)
