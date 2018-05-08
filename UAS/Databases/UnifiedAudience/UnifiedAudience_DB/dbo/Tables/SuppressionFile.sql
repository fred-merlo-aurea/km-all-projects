CREATE TABLE [dbo].[SuppressionFile]
(
	SuppressionFileId INT Identity NOT NULL PRIMARY KEY,
	FileName varchar(50) not null,
	FileDateModified datetime not null,
	DateCreated datetime not null,
	DateUpdated datetime null
)
