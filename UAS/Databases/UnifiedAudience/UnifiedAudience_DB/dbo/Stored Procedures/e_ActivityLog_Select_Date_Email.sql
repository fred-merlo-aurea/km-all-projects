CREATE PROCEDURE [dbo].[e_ActivityLog_Select_Date_Email]
	@FromDate varchar(25),
	@ToDate varchar(25),
	@email varchar(50)
AS
BEGIN

	set nocount on

	SELECT a.ActivityLogID, a.Activity, a.ActivityDate, au.Email
	FROM ActivityLog a  WITH (NOLOCK)
		join ApplicationUsers au WITH (NOLOCK) on a.UserID = au.UserId
	WHERE (len(@FromDate)=0  or ActivityDate  >= @FromDate )and 
		(len(@ToDate)=0  or ActivityDate <= @ToDate + ' 23:59:59') and
		(len(@email)=0 or au.email LIKE '%' + @email + '%')

END