CREATE PROCEDURE e_UserLog_Select_UserLogID
@UserLogID int
AS
	SELECT * FROM UserLog With(NoLock) WHERE UserLogID = @UserLogID
