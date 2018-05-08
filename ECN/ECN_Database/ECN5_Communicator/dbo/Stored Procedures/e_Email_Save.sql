CREATE  PROC [dbo].[e_Email_Save] 
(
	@EmailID int = NULL,
    @EmailAddress varchar(255) = NULL,
    @CustomerID int = NULL,
    @Title varchar(50) = NULL,
    @FirstName varchar(50) = NULL,
    @LastName varchar(50) = NULL,
    @FullName varchar(50) = NULL,
    @Company varchar(100) = NULL,
    @Occupation varchar(50) = NULL,
    @Address varchar(255) = NULL,
    @Address2 varchar(255) = NULL,
    @City varchar(50) = NULL,
    @State varchar(50) = NULL,
    @Zip varchar(50) = NULL,
    @Country varchar(50) = NULL,
    @Voice varchar(50) = NULL,
    @Mobile varchar(50) = NULL,
    @Fax varchar(50) = NULL,
    @Website varchar(50) = NULL,
    @Age varchar(50) = NULL,
    @Income varchar(50) = NULL,
    @Gender varchar(50) = NULL,
    @User1 varchar(255) = NULL,
    @User2 varchar(255) = NULL,
    @User3 varchar(255) = NULL,
    @User4 varchar(255) = NULL,
    @User5 varchar(255) = NULL,
    @User6 varchar(255) = NULL,
    @BirthDate datetime = NULL,
    @UserEvent1 varchar(50) = NULL,
    @UserEvent1Date datetime = NULL,
    @UserEvent2 varchar(50) = NULL,
    @UserEvent2Date datetime = NULL,
    @Notes varchar(1000) = NULL,
    @Password varchar(25) = NULL,
    @BounceScore int = NULL,
    @SoftBounceScore int = NULL,
    @SMSOptIn varchar(10) = NULL,
    @CarrierCode varchar(10) = NULL
)
AS 
BEGIN
	IF @EmailID is NULL or @EmailID <= 0
	BEGIN
		INSERT INTO [Emails]
		(
			EmailAddress,Title,FirstName,LastName,FullName,Company,Occupation,[Address],Address2,City,[State],Zip,Country,
			Voice,Mobile,Fax,Website,Age,Income,Gender,User1,User2,User3,User4,User5,User6,Birthdate,UserEvent1,UserEvent1Date,
			CustomerID,Notes,BounceScore,SoftBounceScore,SMSOptIn,CarrierCode,UserEvent2,UserEvent2Date,[Password], DateAdded
		)
		VALUES
		(
			@EmailAddress,@Title,@FirstName,@LastName,@FullName,@Company,@Occupation,@Address,@Address2,@City,@State,@Zip,@Country,
			@Voice,@Mobile,@Fax,@Website,@Age,@Income,@Gender,@User1,@User2,@User3,@User4,@User5,@User6,@Birthdate,@UserEvent1,@UserEvent1Date,
			@CustomerID,@Notes,@BounceScore,@SoftBounceScore,@SMSOptIn,@CarrierCode,@UserEvent2,@UserEvent2Date,@Password, GETDATE()
		)
		SET @EmailID = @@IDENTITY
	END
	ELSE
	BEGIN
		UPDATE [Emails]
			SET EmailAddress=@emailAddress, Title = @Title, FirstName = @FirstName, LastName = @LastName, FullName = @FullName, Company = @Company,
				Occupation = @Occupation, [Address] = @Address, Address2 = @Address2, City = @City, [State] = @State, Zip = @Zip, Country = @Country,
				Voice = @Voice, Mobile = @Mobile, Fax = @Fax, Website = @Website, Age = @Age, Income = @Income, Gender = @Gender,
				User1 = @User1, User2 = @User2, User3 = @User3, User4 = @User4, User5 = @User5, User6 = @User6, Birthdate = @Birthdate,
				UserEvent1 = @UserEvent1, UserEvent1Date = @UserEvent1Date, CustomerID = @CustomerID, Notes = @Notes, BounceScore = @BounceScore, SoftBounceScore = @SoftBounceScore, SMSOptIn = @SMSOptIn, CarrierCode = @CarrierCode,
				UserEvent2 = @UserEvent2, UserEvent2Date = @UserEvent2Date, [Password]=@Password, DateUpdated = GETDATE()
		WHERE
			EmailID = @EmailID
	END

	SELECT @EmailID
END