CREATE PROCEDURE [dbo].[e_ApplicationLog_Select_DateRange]
(
	@ApplicationID int = NULL,
	@StartDate Date,
	@EndDate Date
)

AS
IF @ApplicationID is NULL
BEGIN
	SELECT *
	FROM [ApplicationLog] WITH(NOLOCK)
	WHERE LogAddedDate Between @StartDate and @EndDate
END
ELSE
BEGIN
	SELECT *
	FROM [ApplicationLog] WITH(NOLOCK)
	WHERE LogAddedDate Between @StartDate and @EndDate and ApplicationID = @ApplicationID
END
