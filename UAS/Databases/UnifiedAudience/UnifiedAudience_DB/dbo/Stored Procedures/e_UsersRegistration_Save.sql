CREATE PROCEDURE [dbo].[e_UsersRegistration_Save]
	@RegID int,
	@UserID	uniqueidentifier,
	@Fname	varchar(20),
	@Lname	varchar(25),
	@Phone	varchar(22),
	@Address1	varchar(45),
	@Address2	varchar(45),
	@City	varchar(30),
	@State	varchar(2),
	@Zip	varchar(5),
	@CountryID	int,
	@CardHolderName	varchar(50),
	@CardType	varchar(25),
	@CardNo	varchar(4),
	@CardExpirationMonth	int,
	@CardExpirationYear	int,
	@CardCVV	varchar(10),
	@BillingAddress1	varchar(45),
	@BillingAddress2	varchar(45),
	@BillingCity	varchar(30),
	@BillingState	varchar(2),
	@BillingZip	varchar(5),
	@BillingCountryID	int,	
	@IsProcessed bit,
	@PaymentTransactionID varchar(50),
	@SubscriptionFee decimal(10,2),
	@TrailtoPaid bit
AS
BEGIN
	
	SET NOCOUNT ON
	
	if @RegID = 0
		begin
	
			--OPEN SYMMETRIC KEY CDMKey DECRYPTION BY CERTIFICATE CDMCert
		
			INSERT INTO [UsersRegistration]
				([UserID]
				,[Fname]
				,[Lname]
				,[Phone]
				,[Address1]
				,[Address2]
				,[City]
				,[State]
				,[Zip]
				,[CountryID]	  
				,[CardHolderName]
				,[CardType]
				,[CardNo]
				,[CardExpirationMonth]
				,[CardExpirationYear]
				,[CardCVV]
				,[BillingAddress1]
				,[BillingAddress2]
				,[BillingCity]
				,[BillingState]
				,[BillingZip]
				,[BillingCountryID]	  			  
				,[IsProcessed]
				,[PaymentTransactionID]
				,[SubscriptionFee]
				,[DateAdded]
				,[TRAILTOPAID])
			  			  
			VALUES
				(@UserID, @Fname, @Lname, @Phone, @Address1, @Address2, @City, @State, @Zip, @CountryID, @CardHolderName,
				@CardType, @CardNo, @CardExpirationMonth, @CardExpirationYear, @CardCVV, @BillingAddress1, @BillingAddress2, @BillingCity, 
				@BillingState, @BillingZip, @BillingCountryID, @IsProcessed,	@PaymentTransactionID, @SubscriptionFee, GETDATE(), @TrailtoPaid)		
		end	
	else 
		begin
			UPDATE UsersRegistration set
				[UserID] = @UserID
				,[Fname] = @Fname
				,[Lname] = @Lname
				,[Phone] = @Phone
				,[Address1] = @Address1
				,[Address2] = @Address2
				,[City] = @City
				,[State] = @State
				,[Zip] = @Zip
				,[CountryID] = @CountryID
			where RegID = @RegID
		end

END