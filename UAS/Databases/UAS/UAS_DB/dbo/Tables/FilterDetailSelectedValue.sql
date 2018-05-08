CREATE TABLE [dbo].[FilterDetailSelectedValue]
(
	FilterDetailSelectedValueId int identity(1,1) not null Primary Key,
	FilterDetailId int not null,
	SelectedValue varchar(50) not null,
	DateCreated datetime not null,
	DateUpdated datetime null,
	CreatedByUserID int not null,
	UpdatedByUserID int null
)
