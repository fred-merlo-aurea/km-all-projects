CREATE PROCEDURE [dbo].[e_User_Select_UserID]
@UserID int
AS
	SELECT * FROM [User] With(NoLock) WHERE UserID = @UserID
