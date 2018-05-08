CREATE PROCEDURE [dbo].[e_ActivityLog_Select_Date_UserID]
	@FromDate varchar(25),
	@ToDate varchar(25),
	@UserID  uniqueidentifier
AS
BEGIN

	set nocount on

	SELECT COUNT(userID) as logincount, convert(varchar,ActivityDate,101) as ActivityDate, UserID
	FROM ActivityLog  WITH (NOLOCK)
	WHERE UserId = @UserID and 
		(len(@FromDate)=0  or ActivityDate  >= @FromDate )and 
		(len(@ToDate)=0  or ActivityDate <= @ToDate + ' 23:59:59') and
		Activity = 'login'
	group by convert(varchar,ActivityDate,101), UserID order by convert(varchar,ActivityDate,101) desc

END