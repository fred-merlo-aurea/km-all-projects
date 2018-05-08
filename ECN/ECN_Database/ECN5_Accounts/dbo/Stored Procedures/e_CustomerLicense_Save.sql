create proc [dbo].[e_CustomerLicense_Save] 
(
	@CLID 		int,
	@CustomerID 	int,
	@QuoteItemID	int,
	@LicenseTypeCode	varchar(50),
	@LicenseLevel	char(4),
	@Quantity	int,
	@Used	int,	
	@ExpirationDate	datetime,
	@AddDate	datetime,
	@IsActive	bit,
	@UserID	int
)
as
Begin
			if @CLID = 0 
			Begin
				INSERT INTO CustomerLicense
				( 
					CustomerID,
					QuoteItemID,
					LicenseTypeCode,
					LicenseLevel,
					Quantity,
					Used,	
					ExpirationDate,
					AddDate,
					IsActive,
					[CreatedUserID],[CreatedDate],[IsDeleted]
				)
				VALUES
				(
					@CustomerID, 
					@QuoteItemID,
					@LicenseTypeCode,
					@LicenseLevel,
					@Quantity,
					@Used,	
					@ExpirationDate,
					@AddDate,
					@IsActive,					
					@UserID, getdate(),0
				)
				set @CLID = @@IDENTITY
			End
			Else
			Begin
				Update CustomerLicense
				Set [CustomerID] = @CustomerID, 
					QuoteItemID = @QuoteItemID,
					LicenseTypeCode = @LicenseTypeCode,
					LicenseLevel = @LicenseLevel,
					Quantity = @Quantity,
					Used = @Used,	
					ExpirationDate = @ExpirationDate,
					AddDate = @AddDate,
					IsActive = @IsActive,
					[UpdatedUserID] = @UserID,
					[UpdatedDate] = getdate()
				where
					CLID = @CLID
			End
				select @CLID as ID
End
