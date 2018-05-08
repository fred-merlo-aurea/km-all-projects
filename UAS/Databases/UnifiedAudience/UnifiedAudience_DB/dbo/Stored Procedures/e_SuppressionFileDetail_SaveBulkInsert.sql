CREATE PROCEDURE [dbo].[e_SuppressionFileDetail_SaveBulkInsert]
	@xml xml,
	@suppFileId int
AS
BEGIN
	
	SET NOCOUNT ON
		
	DECLARE @docHandle int
	DECLARE @import TABLE    
	(  
		Address1 varchar(255) null,
		City varchar(50) null,
		Company varchar(100) null,
		Email varchar(100) null,
		Fax varchar(100) null,
		FirstName varchar(100) null,
		LastName varchar(100) null,		
		RegionCode varchar(50) null,
		ZipCode varchar(50) null,
		Phone varchar(100) null	
	)

	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml  
		
	INSERT INTO @import (FirstName,LastName,Company,Address1,City,RegionCode,ZipCode,Phone,Fax,Email) 
	SELECT FirstName,LastName,Company,Address1,City,RegionCode,ZipCode,Phone,Fax,Email
	FROM OPENXML(@docHandle, N'/XML/SuppressionFileDetail')
	WITH
	(
		FirstName  varchar(100) 'FirstName',
		LastName varchar(100) 'LastName',
		Company varchar(100) 'Company',
		Address1 varchar(255) 'Address1',
		City varchar(50) 'City',
		RegionCode varchar(50) 'RegionCode',
		ZipCode varchar(50) 'ZipCode',
		Phone varchar(100) 'Phone',
		Fax varchar(100) 'Fax',
		Email varchar(100) 'Email',
		SuppressionFileid int 'SuppressionFileId'
	)
	
	
	EXEC sp_xml_removedocument @docHandle
	
	insert into SuppressionFileDetail (SuppressionFileId,FirstName,LastName,Company,Address1,City,RegionCode,ZipCode,Phone,Fax,Email)
	select @suppFileId,FirstName,LastName,Company,Address1,City,RegionCode,ZipCode,Phone,Fax,Email
	from @import

END