CREATE TABLE [dbo].[FilterGroup]
(
	FilterGroupId int identity(1,1) not null Primary Key,
	FilterId int not null,
	SortOrder int not null,
	DateCreated datetime not null,
	DateUpdated datetime null,
	CreatedByUserID int not null,
	UpdatedByUserID int null
)
