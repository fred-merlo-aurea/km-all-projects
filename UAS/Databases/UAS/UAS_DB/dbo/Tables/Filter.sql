CREATE TABLE [dbo].[Filter]
(
	FilterId int identity(1,1) not null Primary Key,
	FilterName varchar(50) not null,
	ProductId int null,
	IsActive bit default('true') not null,
	BrandId int null,
	FilterGroupID int null,
	DateCreated datetime not null,
	DateUpdated datetime null,
	CreatedByUserID int not null,
	UpdatedByUserID int null, 
    [ClientID] INT NULL
)
