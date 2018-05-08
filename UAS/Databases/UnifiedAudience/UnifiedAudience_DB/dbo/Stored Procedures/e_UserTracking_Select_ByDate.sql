create PROCEDURE [dbo].[e_UserTracking_Select_ByDate]
(
	@FromDate datetime,
	@ToDate datetime
)
AS
BEGIN
	
	SET NOCOUNT ON
	
	SELECT *
	FROM   usertracking with(nolock) 
	WHERE  activitydatetime >= @FromDate 
		   AND activitydatetime <= @ToDate + ' 23:59:59'

END