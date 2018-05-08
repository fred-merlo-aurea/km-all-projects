﻿CREATE PROCEDURE [dbo].[e_DomainTrackerActivity_GetBrowserStats]
@DomainTrackerID int,
@startDate DateTime  = NULL,
@endDate DateTime = NULL,
@Filter  nvarchar(50) = NULL,
@TypeFilter varchar(50)
AS

SET NOCOUNT ON 

IF @Filter IS NOT NULL 
	if @TypeFilter = 'known'
	BEGIN
		SELECT 
			Browser, 
			COUNT(DomainTrackerActivityID) AS 'Counts' 
		FROM 
			DomainTrackerActivity dta WITH(NOLOCK)
			JOIN DomainTrackerUserProfile dtup with(nolock) on dta.ProfileID = dtup.ProfileID
		WHERE
			DomainTrackerID = @DomainTrackerID
			AND TimeStamp >= @startDate
			AND TimeStamp <= @endDate
			AND pageURL LIKE '%' + @Filter + '%' 
			AND ISNULL(dtup.IsKnown,1) = 1
		GROUP BY 
			Browser
		ORDER BY
			COUNT(DomainTrackerActivityID) DESC
	END
	else if @TypeFilter = 'unknown'
	BEGIN
		SELECT 
			Browser, 
			COUNT(DomainTrackerActivityID) AS 'Counts' 
		FROM 
			DomainTrackerActivity dta WITH(NOLOCK)
			JOIN DomainTrackerUserProfile dtup with(nolock) on dta.ProfileID = dtup.ProfileID
		WHERE
			DomainTrackerID = @DomainTrackerID
			AND TimeStamp >= @startDate
			AND TimeStamp <= @endDate
			AND pageURL LIKE '%' + @Filter + '%' 
			AND dtup.IsKnown = 0
		GROUP BY 
			Browser
		ORDER BY
			COUNT(DomainTrackerActivityID) DESC
	END
	else 
	BEGIN
		SELECT 
			Browser, 
			COUNT(DomainTrackerActivityID) AS 'Counts' 
		FROM 
			DomainTrackerActivity WITH(NOLOCK)
		WHERE
			DomainTrackerID = @DomainTrackerID
			AND TimeStamp >= @startDate
			AND TimeStamp <= @endDate
			AND pageURL LIKE '%' + @Filter + '%' 
		GROUP BY 
			Browser
		ORDER BY
			COUNT(DomainTrackerActivityID) DESC
	END
ELSE
	if @TypeFilter = 'known'
	BEGIN	
		SELECT 
			Browser, 
			COUNT(DomainTrackerActivityID) AS 'Counts' 
		FROM 
			DomainTrackerActivity dta WITH(NOLOCK)
			JOIN DomainTrackerUserProfile dtup with(nolock) on dta.ProfileID = dtup.ProfileID
		WHERE
			DomainTrackerID = @DomainTrackerID
			AND TimeStamp >= @startDate
			AND TimeStamp <= @endDate
			AND ISNULL(dtup.IsKnown,1) = 1
		GROUP BY 
			Browser
		ORDER BY 
			COUNT(DomainTrackerActivityID) DESC
	END
	else if @TypeFilter = 'unknown'
	BEGIN
	
		SELECT 
			Browser, 
			COUNT(DomainTrackerActivityID) AS 'Counts' 
		FROM 
			DomainTrackerActivity dta WITH(NOLOCK)
			JOIN DomainTrackerUserProfile dtup with(nolock) on dta.ProfileID = dtup.ProfileID
		WHERE
			DomainTrackerID = @DomainTrackerID
			AND TimeStamp >= @startDate
			AND TimeStamp <= @endDate
			AND dtup.IsKnown = 0
		GROUP BY 
			Browser
		ORDER BY 
			COUNT(DomainTrackerActivityID) DESC
	END
	else 
	BEGIN

		SELECT 
			Browser, 
			COUNT(DomainTrackerActivityID) AS 'Counts' 
		FROM 
			DomainTrackerActivity WITH(NOLOCK)
		WHERE
			DomainTrackerID = @DomainTrackerID
			AND TimeStamp >= @startDate
			AND TimeStamp <= @endDate
		GROUP BY 
			Browser
		ORDER BY 
			COUNT(DomainTrackerActivityID) DESC
	END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[e_DomainTrackerActivity_GetBrowserStats] TO [ecn5]
    AS [dbo];

