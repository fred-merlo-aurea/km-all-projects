CREATE proc [dbo].[e_CustomerLicense_Select_CustomerID_LicenseTypeCode]
(
	@CustomerID int,
	@licensetypecode varchar(20)
)
as
Begin
	if exists (select top 1 * from CustomerLicense 
				where CustomerID = @CustomerID and Quantity = -1 and LicenseTypeCode = @licensetypecode and ExpirationDate>GetDate() AND AddDate<GetDate()
				and isnull(IsActive,0) = 1 and ISNULL(IsDeleted,0) = 0
				)
	Begin
		SELECT -1 as Allowed, SUM(Isnull(Used,0)) as Used, -1 as Available
        FROM dbo.CustomerLicense 
        WHERE 
			CustomerID= @CustomerID AND 
			LicenseTypeCode = @licensetypecode AND 
			ExpirationDate>GetDate() AND 
			AddDate<GetDate() and isnull(IsActive,0) = 1 and ISNULL(IsDeleted,0) = 0
	End
	Else
	Begin
		SELECT SUM(Isnull(Quantity,0)) as Allowed, SUM(Isnull(Used,0)) as Used, SUM(Quantity- Used) as Available
        FROM dbo.CustomerLicense 
        WHERE	
				CustomerID= @CustomerID AND 
				LicenseTypeCode = @licensetypecode AND 
				ExpirationDate>GetDate() AND 
				AddDate<GetDate()	 and isnull(IsActive,0) = 1 and ISNULL(IsDeleted,0) = 0
	End
End
