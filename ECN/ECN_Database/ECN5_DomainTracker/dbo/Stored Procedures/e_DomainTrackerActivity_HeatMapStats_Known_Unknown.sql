CREATE PROCEDURE [dbo].[e_DomainTrackerActivity_HeatMapStats_Known_Unknown]
	@DomainTrackerID int,
	@startDate datetime = null,
	@endDate datetime = null,
	@Filter varchar(500) = null
AS
	declare @Known table([Type] varchar(10), HitCount int, Region varchar(100), Country varchar(100), [State] varchar(100), City varchar(100))
	declare @Unknown table([Type] varchar(10), HitCount int, Region varchar(100), Country varchar(100), [State] varchar(100), City varchar(100))

	--Known
IF @Filter IS NOT NULL 

insert into @Known
SELECT 
	'Known' as 'Type',
	COUNT(dta.IPAddress) AS HitCount ,
	Region,
	Country,
	State,
	City
FROM 
	DomainTrackerActivity dta WITH(NOLOCK)
	JOIN IPLocationDetailed ipd WITH(NOLOCK) ON dta.ipaddress = ipd.ipaddress
	join DomainTrackerUserProfile dtup with(nolock) on dta.ProfileID = dtup.ProfileID
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

ELSE
insert into @Known
SELECT 
	'Known' as 'Type',
	COUNT(dta.IPAddress) AS HitCount ,
	Region,
	Country,
	State,
	City
FROM 
	DomainTrackerActivity dta WITH(NOLOCK)
	JOIN IPLocationDetailed ipd WITH(NOLOCK) ON dta.ipaddress = ipd.ipaddress
	join DomainTrackerUserProfile dtup with(nolock) on dta.ProfileID = dtup.ProfileID
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


	--Unknown
	IF @Filter IS NOT NULL 
	insert into @Unknown
SELECT 
	'Unknown' as 'Type',
	COUNT(dta.IPAddress) AS HitCount ,
	Region,
	Country,
	State,
	City
FROM 
	DomainTrackerActivity dta WITH(NOLOCK)
	JOIN IPLocationDetailed ipd WITH(NOLOCK) ON dta.ipaddress = ipd.ipaddress
	join DomainTrackerUserProfile dtup with(nolock) on dta.ProfileID = dtup.ProfileID
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

ELSE
insert into @Unknown
SELECT 
	'Unknown' as 'Type',
	COUNT(dta.IPAddress) AS HitCount ,
	Region,
	Country,
	State,
	City
FROM 
	DomainTrackerActivity dta WITH(NOLOCK)
	JOIN IPLocationDetailed ipd WITH(NOLOCK) ON dta.ipaddress = ipd.ipaddress
	join DomainTrackerUserProfile dtup with(nolock) on dta.ProfileID = dtup.ProfileID
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


insert into @Known(Type, HitCount, Region, Country, State, City)
Select Type, HitCount, Region, Country, State, City
from @Unknown

declare @Final table(Region varchar(100), Country varchar(100), State varchar(100), City varchar(100), Known int, Unknown int, Total int)

insert into @Final(Region, Country, State, City, Known, Unknown, Total)
Select Region, Country, State, City, case when Type = 'Known' then SUM(hitCount) end as 'Known', case when Type = 'Unknown' then SUM(hitCOunt) end as 'Unknown', 0
FROM @Known
Group By Region, Country, State, City, Type

select Region, Country, State, City, SUM(ISNULL(Known,0)) as 'Known', SUM(ISNULL(Unknown,0)) as 'Unknown', SUM(ISNULL(Known,0)) + SUM(ISNULL(Unknown,0)) as 'Total'
FROM @Final
GROUP BY Region, Country, State, City