CREATE PROCEDURE [dbo].[e_User_Search_Email]
@Email varchar(250)
AS
	SELECT *
	FROM [User] With(NoLock)
	WHERE EmailAddress = @Email
