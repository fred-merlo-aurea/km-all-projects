CREATE TABLE [dbo].[FilterDetail]
(
	FilterDetailId int identity(1,1) not null,
	FilterId int not null,
	FilterTypeId  int not null,
	FilterField varchar(50) not null,
	AdHocFromField varchar(50) null,
	AdHocToField varchar(50) null,
	AdHocFieldValue varchar(50) null,
	SearchCondition varchar(50) null,
	[FilterObjectType] varchar(50) null,
	FilterGroupID int null,
	DateCreated datetime not null,
	DateUpdated datetime null,
	CreatedByUserID int not null,
	UpdatedByUserID int null,
    CONSTRAINT [PK_FilterDetail] PRIMARY KEY CLUSTERED ([FilterDetailId] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_FilterDetail_FilterDetail] FOREIGN KEY ([FilterId]) REFERENCES [dbo].[Filter] ([FilterId])
)
