CREATE TABLE [dbo].[RuleSet_File_Map]
(
	RuleSetId int not null,
	SourceFileId int not null,
	FileTypeId int not null,
	IsSystem bit default((0)) not null,
	IsGlobal bit default ((0)) not null,
	IsActive bit default((1)) not null,
	ExecutionPointId int not null,
	ExecutionOrder int default(1) not null,
	WhereClause varchar(max) null,
	DateCreated datetime default(getdate()) not null,
	DateUpdated datetime null,
	CreatedByUserId int not null,
	UpdatedByUserId int null, 
    CONSTRAINT [PK_RuleSet_File_Map] PRIMARY KEY ([RuleSetId], [FileTypeId], [SourceFileId]) WITH (FILLFACTOR = 90),
	CONSTRAINT [FK_RuleSet_File_Map_RuleSet] FOREIGN KEY ([RuleSetId]) REFERENCES [dbo].[RuleSet] ([RuleSetId]),
    CONSTRAINT [FK_RuleSet_File_Map_SourceFile] FOREIGN KEY ([SourceFileId]) REFERENCES [dbo].[SourceFile] ([SourceFileID])
)
