CREATE proc [dbo].[e_CustomerContact_Save] 
(
	@ContactID 		int,
	@CustomerID 	int,
	@FirstName	varchar(50),
	@LastName	varchar(50),
	@Address	varchar(100),
	@Address2	varchar(50),
	@City 	varchar(50),
	@State	varchar(2),
	@Zip	varchar(10),
	@Phone	varchar(20),
	@Mobile varchar(20),
	@Email 	varchar(50),
	@UserID	int
)
as
Begin
			if @ContactID <= 0 
			Begin
				INSERT INTO CustomerContact
				( 
					[CustomerID], [FirstName], [LastName], [Address], [Address2],
					[City],[State],[Zip],[Phone], [Mobile],[Email],
					[CreatedUserID],[CreatedDate],[IsDeleted]
				)
				VALUES
				(
					@CustomerID, @FirstName, @LastName, @Address,  @Address2,
					@City, @State, @Zip, @Phone, @Mobile, @Email,
					@UserID, getdate(),0
				)
				set @ContactID = @@IDENTITY
			End
			Else
			Begin
				Update CustomerContact
				Set [CustomerID] = @CustomerID, 
					[FirstName] = @FirstName, 
					[LastName] = @LastName, 
					[Address] = @Address, 
					[Address2] = @Address2,
					[City] = @City,
					[State] = @State,
					[Zip] = @Zip,
					[Phone] = @Phone, 
					[Mobile] = @Mobile,
					[Email] = @Email,
					[UpdatedUserID] = @UserID,
					[UpdatedDate] = getdate()
				where
					ContactID = @ContactID
			End
				select @ContactID as ID
End
