CREATE PROCEDURE [dbo].[e_DomainTrackerActivity_GetPageViews]
@DomainTrackerID int,
@startDate DateTime  = NULL,
@endDate DateTime = NULL,
@Filter  nvarchar(50) = '',
@TypeFilter varchar(50)

AS

SET NOCOUNT ON

IF @Filter IS NOT NULL 
	if @TypeFilter = 'known'
	BEGIN
		SELECT
			CONVERT(DATE, [Timestamp]) AS [Date], 
			COUNT(*) AS [Views] 
		FROM 
			DomainTrackerActivity dta WITH(NOLOCK)
			join DomainTrackerUserProfile dtup with(nolock) on dta.ProfileID = dtup.ProfileID
		WHERE
			DomainTrackerID= @DomainTrackerID 
			AND pageURL LIKE '%' + @Filter + '%' 
			AND TimeStamp >= @startDate
			AND TimeStamp <= @endDate
			AND ISNULL(dtup.IsKnown,1) = 1
		GROUP BY 
			CONVERT(DATE, [Timestamp])
		ORDER BY 
			CONVERT(DATE, [Timestamp])  
	END
	ELSE if @TypeFilter = 'unknown'
	BEGIN
		SELECT
			CONVERT(DATE, [Timestamp]) AS [Date], 
			COUNT(*) AS [Views] 
		FROM 
			DomainTrackerActivity dta WITH(NOLOCK)
			join DomainTrackerUserProfile dtup with(nolock) on dta.ProfileID = dtup.ProfileID
		WHERE
			DomainTrackerID= @DomainTrackerID 
			AND pageURL LIKE '%' + @Filter + '%' 
			AND TimeStamp >= @startDate
			AND TimeStamp <= @endDate
			AND dtup.IsKnown = 0
		GROUP BY 
			CONVERT(DATE, [Timestamp])
		ORDER BY 
			CONVERT(DATE, [Timestamp])  
	END
	ELSE
	BEGIN
		SELECT
			CONVERT(DATE, [Timestamp]) AS [Date], 
			COUNT(*) AS [Views] 
		FROM 
			DomainTrackerActivity WITH(NOLOCK)
		WHERE
			DomainTrackerID= @DomainTrackerID 
			AND pageURL LIKE '%' + @Filter + '%' 
			AND TimeStamp >= @startDate
			AND TimeStamp <= @endDate
		GROUP BY 
			CONVERT(DATE, [Timestamp])
		ORDER BY 
			CONVERT(DATE, [Timestamp])  
	END
ELSE
	if @TypeFilter = 'known'
	BEGIN
		SELECT
			CONVERT(DATE, [Timestamp]) AS [Date], 
			COUNT(*) AS [Views] 
		FROM 
			DomainTrackerActivity dta WITH(NOLOCK)
			JOIN DomainTrackerUserProfile dtup with(nolock) on dta.ProfileID = dtup.ProfileID
		WHERE
			DomainTrackerID= @DomainTrackerID 
			AND TimeStamp >= @startDate
			AND TimeStamp <= @endDate
			AND ISNULL(dtup.IsKnown,1) = 1
		GROUP BY 
			CONVERT(DATE, [Timestamp])
		ORDER BY 
			CONVERT(DATE, [Timestamp])  
	END
	else if @TypeFilter = 'unknown'
	BEGIN
		SELECT
			CONVERT(DATE, [Timestamp]) AS [Date], 
			COUNT(*) AS [Views] 
		FROM 
			DomainTrackerActivity dta WITH(NOLOCK)
			JOIN DomainTrackerUserProfile dtup with(nolock) on dta.ProfileID = dtup.ProfileID
		WHERE
			DomainTrackerID= @DomainTrackerID 
			AND TimeStamp >= @startDate
			AND TimeStamp <= @endDate
			AND dtup.IsKnown = 0
		GROUP BY 
			CONVERT(DATE, [Timestamp])
		ORDER BY 
			CONVERT(DATE, [Timestamp])  
	END
	ELSE
	BEGIN
		SELECT
			CONVERT(DATE, [Timestamp]) AS [Date], 
			COUNT(*) AS [Views] 
		FROM 
			DomainTrackerActivity WITH(NOLOCK)
		WHERE
			DomainTrackerID= @DomainTrackerID 
			AND TimeStamp >= @startDate
			AND TimeStamp <= @endDate
		GROUP BY 
			CONVERT(DATE, [Timestamp])
		ORDER BY 
			CONVERT(DATE, [Timestamp])  
	END
GO

GRANT EXECUTE
    ON OBJECT::[dbo].[e_DomainTrackerActivity_GetPageViews] TO [ecn5]
    AS [dbo];

