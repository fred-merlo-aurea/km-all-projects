CREATE TABLE dbo.SuppressionFileDetail
(
	SuppressionFileDetailId int identity(1,1),
	SuppressionFileId int not null,
	FirstName varchar(100) null,
	LastName varchar(100) null,
	Company varchar(100) null,
	Address1 varchar(255) null,
	City varchar(50) null,
	RegionCode varchar(50) null,
	ZipCode varchar(50) null,
	Phone varchar(100) null,
	Fax varchar(100) null,
	Email varchar(100) null, 
    CONSTRAINT [PK_SuppressionFileDetail] PRIMARY KEY ([SuppressionFileDetailId]) 
)
go

CREATE INDEX IX_SuppressionFileDetail_FirstName
    ON dbo.SuppressionFileDetail
    (FirstName)
go
CREATE INDEX IX_SuppressionFileDetail_LastName
    ON dbo.SuppressionFileDetail
    (LastName)
go
CREATE INDEX IX_SuppressionFileDetail_Company
    ON dbo.SuppressionFileDetail
    (Company)
go
CREATE INDEX IX_SuppressionFileDetail_Address1
    ON dbo.SuppressionFileDetail
    (Address1)
go
CREATE INDEX IX_SuppressionFileDetail_ZipCode
    ON dbo.SuppressionFileDetail
    (ZipCode)
go
CREATE INDEX IX_SuppressionFileDetail_Phone
    ON dbo.SuppressionFileDetail
    (Phone)
go
CREATE INDEX IX_SuppressionFileDetail_Email
    ON dbo.SuppressionFileDetail
    (Email)
go
