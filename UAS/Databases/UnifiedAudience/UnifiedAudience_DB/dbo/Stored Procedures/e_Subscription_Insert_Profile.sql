CREATE PROCEDURE [dbo].[e_Subscription_Insert_Profile]
@FirstName varchar(100),
@LastName varchar(100),
@EmailAddress varchar(100),
@Address varchar(255),
@City varchar(50),
@State varchar(50),
@Zip varchar(10)
AS
BEGIN

	SET NOCOUNT ON

	INSERT INTO Subscriptions (SEQUENCE,FNAME,LNAME,EMAIL,EmailExists,ADDRESS,CITY,STATE,ZIP)
	VALUES(-1,@FirstName,@LastName,@EmailAddress,'true',@Address,@City,@State,@Zip);SELECT @@IDENTITY;

END