CREATE PROCEDURE [dbo].[e_DomainTrackerActivity_GetPageViews_Known]
	@DomainTrackerID int,
	@startDate DateTime  = NULL,
	@endDate DateTime = NULL,
	@Filter  nvarchar(50) = ''

AS

SET NOCOUNT ON
	declare @Known table([Date] DATE, [Views] int)
	declare @Unknown table([Date] DATE, [Views] int)
	declare @Total table([Date] DATE, [Views] int)
IF @Filter IS NOT NULL 
BEGIN
	insert into @Total
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
-------------KNOWN
	insert into @Known
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
-----------UNKNOWN
	insert into @Unknown
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
	insert into @Total
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

-------------KNOWN
	insert into @Known
	SELECT
		CONVERT(DATE, [Timestamp]) AS [Date], 
		COUNT(*) AS [Views] 
	FROM 
		DomainTrackerActivity dta WITH(NOLOCK)
		join DomainTrackerUserProfile dtup with(nolock) on dta.ProfileID = dtup.ProfileID
	WHERE
		DomainTrackerID= @DomainTrackerID 		
		AND TimeStamp >= @startDate
		AND TimeStamp <= @endDate
		AND ISNULL(dtup.IsKnown,1) = 1
	GROUP BY 
		CONVERT(DATE, [Timestamp])
	ORDER BY 
		CONVERT(DATE, [Timestamp])  
-----------UNKNOWN
	insert into @Unknown
	SELECT
		CONVERT(DATE, [Timestamp]) AS [Date], 
		COUNT(*) AS [Views] 
	FROM 
		DomainTrackerActivity dta WITH(NOLOCK)
		join DomainTrackerUserProfile dtup with(nolock) on dta.ProfileID = dtup.ProfileID
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

select [Date], [Views], 'Total' as 'Type'
from @Total
UNION
select [Date], [Views], 'Known' as 'Type'
from @Known
UNION
select [Date], [Views], 'Unknown' as 'Type'
from @Unknown
