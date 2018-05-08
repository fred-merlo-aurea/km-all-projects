CREATE PROCEDURE [dbo].[e_BillingContact_Save]    
(
@BillingContactID int, 			
@CustomerID int, 
@Salutation varchar(10),
@ContactName varchar(50),
@FirstName varchar(50),
@LastName varchar(50),
@ContactTitle varchar(50), 
@Phone varchar(50), 
@Fax varchar(50), 
@Email varchar(50), 
@StreetAddress varchar(200), 
@City varchar(50), 
@State varchar(50), 
@Country varchar(20), 
@Zip varchar(20),
	@UserID	int
)
AS  

BEGIN   
	SET NOCOUNT ON;  
	
	if (@BillingContactID > 0)
	Begin
		UPDATE BillingContact
		   SET  CustomerID = @CustomerID,
				Salutation = @Salutation,
				ContactName = @ContactName,
				FirstName = @FirstName,
				LastName = @LastName,	
				ContactTitle = @ContactTitle,
				Phone = @Phone,	
				Fax = @Fax,
				Email = @Email,
				StreetAddress = @StreetAddress,
				City = @City,	
				State = @State,
				Country = @Country,	
				Zip = @Zip,
				UpdatedUserID = @UserID,
				UpdatedDate = getdate()				
		 WHERE BillingContactID = @BillingContactID
		 
		 select @BillingContactID

	End
	Else
	Begin
		
		INSERT INTO BillingContact(
			CustomerID,
			Salutation,
			ContactName,
			FirstName,
			LastName,
			ContactTitle,
			Phone,
			Fax,
			Email, 
			StreetAddress,
			City, 
			State,
			Country,
			Zip, 
			CreatedUserID,
			CreatedDate,
			IsDeleted		
			) 
		VALUES (
			@CustomerID,
			@Salutation,
			@ContactName,
			@FirstName,
			@LastName,
			@ContactTitle,
			@Phone,
			@Fax,
			@Email, 
			@StreetAddress,
			@City, 
			@State,
			@Country,
			@Zip,
			@UserID, 
			getdate(),
			0)
			SELECT @@IDENTITY;
	End
END
