CREATE TABLE [dbo].[Issue]
(
	IssueId int identity(1,1),
	PublicationId int NOT NULL,
	IssueName varchar(100) NOT NULL,
	IssueCode varchar(50) NOT NULL,
	DateOpened datetime NOT NULL ,
	OpenedByUserID int NOT NULL ,
	IsClosed bit NOT NULL,
	DateClosed datetime ,
	ClosedByUserID int ,
	IsComplete bit NOT NULL,
	DateComplete datetime ,
	CompleteByUserID int ,
	DateCreated datetime NOT NULL,
    DateUpdated datetime ,
    CreatedByUserID int NOT NULL,
    UpdatedByUserID int, 
    CONSTRAINT [PK_Issue] PRIMARY KEY ([IssueId]) 
)
