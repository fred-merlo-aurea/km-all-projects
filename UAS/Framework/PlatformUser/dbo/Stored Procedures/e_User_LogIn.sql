CREATE PROCEDURE [dbo].[e_User_LogIn]
@UserName varchar(50),
@Password varchar(250)
AS
	SELECT *
	FROM [User] With(NoLock)
	WHERE UserName = @UserName
	AND Password = @Password COLLATE Latin1_General_CS_AS
	AND IsActive = 'true'
