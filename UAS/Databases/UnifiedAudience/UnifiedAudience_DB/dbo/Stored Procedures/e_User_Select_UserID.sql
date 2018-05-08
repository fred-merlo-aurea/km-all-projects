CREATE PROCEDURE dbo.e_User_Select_UserID
@UserID int
AS
BEGIN
	
	SET NOCOUNT ON
	
	SELECT *
	FROM Users With(NoLock)
	WHERE UserID = @UserID

END