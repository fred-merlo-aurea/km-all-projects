
CREATE PROCEDURE [dbo].[e_DomainTrackerActivity_HeatMapStats]
	@DomainTrackerID int,
	@startDate DateTime  = NULL,
	@endDate DateTime = NULL,
	@Filter  nvarchar(50) = NULL,
	@TypeFilter varchar(20)
AS

SET NOCOUNT ON

IF @Filter IS NOT NULL 
BEGIN
	if @TypeFilter = 'known'
	BEGIN
		SELECT 
			COUNT(dta.IPAddress) AS HitCount ,
			Region,
			Country,
			State,
			City
		FROM 
			DomainTrackerActivity dta WITH(NOLOCK)
			JOIN IPLocationDetailed ipd WITH(NOLOCK) ON dta.ipaddress = ipd.ipaddress
			JOIN DomainTrackerUserProfile dtup With(nolock) on dta.ProfileID = dtup.ProfileID
		WHERE 
			DomainTrackerID = @DomainTrackerID
			AND TimeStamp >= @startDate
			AND TimeStamp <= @endDate
			AND pageURL LIKE '%' + @Filter + '%' 
			AND ISNULL(dtup.IsKnown,1) = 1
		GROUP BY 
			Region,
			Country,
			State,
			City
		ORDER BY 
			COUNT(dta.IPAddress) DESC
	END
	else if @TypeFilter = 'unknown'
	BEGIN
			SELECT 
			COUNT(dta.IPAddress) AS HitCount ,
			Region,
			Country,
			State,
			City
		FROM 
			DomainTrackerActivity dta WITH(NOLOCK)
			JOIN IPLocationDetailed ipd WITH(NOLOCK) ON dta.ipaddress = ipd.ipaddress
			JOIN DomainTrackerUserProfile dtup With(nolock) on dta.ProfileID = dtup.ProfileID
		WHERE 
			DomainTrackerID = @DomainTrackerID
			AND TimeStamp >= @startDate
			AND TimeStamp <= @endDate
			AND pageURL LIKE '%' + @Filter + '%' 
			AND dtup.IsKnown = 0
		GROUP BY 
			Region,
			Country,
			State,
			City
		ORDER BY 
			COUNT(dta.IPAddress) DESC
	END
	else
	BEGIN
		SELECT 
			COUNT(dta.IPAddress) AS HitCount ,
			Region,
			Country,
			State,
			City
		FROM 
			DomainTrackerActivity dta WITH(NOLOCK)
			JOIN IPLocationDetailed ipd WITH(NOLOCK) ON dta.ipaddress = ipd.ipaddress
		WHERE 
			DomainTrackerID = @DomainTrackerID
			AND TimeStamp >= @startDate
			AND TimeStamp <= @endDate
			AND pageURL LIKE '%' + @Filter + '%' 
		GROUP BY 
			Region,
			Country,
			State,
			City
		ORDER BY 
			COUNT(dta.IPAddress) DESC
	END
END
ELSE
BEGIN
	if @TypeFilter = 'known'
	BEGIN
		SELECT 
			COUNT(dta.IPAddress) AS HitCount ,
			Region,
			Country,
			State,
			City
		FROM 
			DomainTrackerActivity dta WITH(NOLOCK)
			JOIN IPLocationDetailed ipd WITH(NOLOCK) ON dta.ipaddress = ipd.ipaddress
			JOIN DomainTrackerUserProfile dtup with(nolock) on dta.ProfileID = dtup.ProfileID
		WHERE 
			DomainTrackerID = @DomainTrackerID
			AND TimeStamp >= @startDate
			AND TimeStamp <= @endDate
			AND ISNULL(dtup.IsKnown,1) = 1
		GROUP BY 
			Region,
			Country,
			State,
			City,
			Lat,
			Long,
			Zip,
			TimeZone
		ORDER BY 
			COUNT(dta.IPAddress) DESC
	END
	else if @TypeFilter = 'unknown'
	BEGIN
		SELECT 
			COUNT(dta.IPAddress) AS HitCount ,
			Region,
			Country,
			State,
			City
		FROM 
			DomainTrackerActivity dta WITH(NOLOCK)
			JOIN IPLocationDetailed ipd WITH(NOLOCK) ON dta.ipaddress = ipd.ipaddress
			JOIN DomainTrackerUserProfile dtup with(nolock) on dta.ProfileID = dtup.ProfileID
		WHERE 
			DomainTrackerID = @DomainTrackerID
			AND TimeStamp >= @startDate
			AND TimeStamp <= @endDate
			AND dtup.IsKnown = 0
		GROUP BY 
			Region,
			Country,
			State,
			City,
			Lat,
			Long,
			Zip,
			TimeZone
		ORDER BY 
			COUNT(dta.IPAddress) DESC
	END
	else 
	BEGIN

		SELECT 
			COUNT(dta.IPAddress) AS HitCount ,
			Region,
			Country,
			State,
			City
		FROM 
			DomainTrackerActivity dta WITH(NOLOCK)
			JOIN IPLocationDetailed ipd WITH(NOLOCK) ON dta.ipaddress = ipd.ipaddress
		WHERE 
			DomainTrackerID = @DomainTrackerID
			AND TimeStamp >= @startDate
			AND TimeStamp <= @endDate
		GROUP BY 
			Region,
			Country,
			State,
			City,
			Lat,
			Long,
			Zip,
			TimeZone
		ORDER BY 
			COUNT(dta.IPAddress) DESC
	END
END
