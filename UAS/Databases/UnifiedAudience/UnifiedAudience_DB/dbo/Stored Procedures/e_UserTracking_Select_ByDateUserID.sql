CREATE PROCEDURE e_UserTracking_Select_ByDateUserID
(
	@FromDate datetime,
	@ToDate datetime,
	@UserID varchar(50)
)
AS
BEGIN
	
	SET NOCOUNT ON
	
	SELECT ut.*, u.Username
	FROM UserTracking ut with(nolock) 
		join ecn5_accounts.dbo.Users u with(nolock) on u.UserID = ut.UserID
	WHERE ActivityDateTime >= @FromDate and ActivityDateTime <= @ToDate + ' 23:59:59' and
		(@UserID=0 or ut.UserID  = @UserID)

END
GO