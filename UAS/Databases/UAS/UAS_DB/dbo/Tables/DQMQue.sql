CREATE TABLE [dbo].[DQMQue]
(
	ProcessCode   varchar(50) Not Null,
	ClientID	  int Not null,
	[IsDemo] BIT NOT NULL DEFAULT ('true'),
	[IsADMS] BIT NOT NULL,
	DateCreated   datetime Not null,
	IsQued	      bit not null default('false'),
	DateQued	  datetime Null,
	IsCompleted   bit not null default('false'),
	DateCompleted datetime null,
	SourceFileId	int null
    
)
GO

CREATE NONCLUSTERED INDEX [NCI_DQMQue]
    ON [dbo].[DQMQue]([ClientID] ASC, [IsDemo] ASC, [IsADMS] ASC, [IsQued] ASC)
    INCLUDE([ProcessCode], [DateCreated], [DateQued], [IsCompleted], [DateCompleted], [SourceFileId]);
GO