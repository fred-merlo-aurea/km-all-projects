CREATE PROCEDURE [dbo].[e_User_Search_UserName]
@UserName varchar(50)
AS
	SELECT *
	FROM [User] With(NoLock)
	WHERE UserName = @UserName