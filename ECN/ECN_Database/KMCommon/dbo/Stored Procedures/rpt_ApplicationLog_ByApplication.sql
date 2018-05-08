CREATE PROCEDURE [dbo].[rpt_ApplicationLog_ByApplication]
@ApplicationID int = NULL,
@StartDate date = NULL,
@EndDate date = NULL
AS
BEGIN
	SELECT 
		a.ApplicationID, a.ApplicationName, al.SourceMethod, al.LogAddedDate, al.LogAddedTime, s.SeverityName, 
		al.NotificationSent, al.Exception, al.LogNote
	FROM 
		ApplicationLog al WITH (NOLOCK)
		JOIN [Application] a WITH (NOLOCK) ON al.ApplicationID = a.ApplicationID
		JOIN Severity s WITH (NOLOCK) ON al.SeverityID = s.SeverityID
	WHERE 
		al.ApplicationID = @ApplicationID AND
		al.LogAddedDate BETWEEN @StartDate AND @EndDate
END
