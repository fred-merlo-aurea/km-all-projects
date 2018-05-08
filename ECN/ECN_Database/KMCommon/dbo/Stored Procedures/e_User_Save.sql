CREATE PROCEDURE [dbo].[e_User_Save]
@UserID int = NULL,
@FirstName varchar(50) = NULL,
@LastName varchar(50) = NULL,
@EmailAddress varchar(100) = NULL
AS
BEGIN
	IF @UserID = NULL or @UserID <= 0
	BEGIN
		INSERT INTO [User]
		(
			FirstName, LastName, EmailAddress
		)
		VALUES
		(
			@FirstName, @LastName, @EmailAddress
		)
		SET @UserID = @@IDENTITY
	END
	ELSE
	BEGIN
		UPDATE [User]
			SET FirstName=@FirstName, LastName=@LastName, EmailAddress=@EmailAddress
		WHERE
			UserID = @UserID
	END

	SELECT @UserID
END
