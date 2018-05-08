CREATE PROCEDURE [dbo].[e_DomainTrackerActivity_GetBrowserStats_Known_Unknown]
	@DomainTrackerID int,
@startDate DateTime  = NULL,
@endDate DateTime = NULL,
@Filter  nvarchar(50) = NULL
AS

SET NOCOUNT ON 
	declare @Known table(Browser varchar(100), HitCount int, Type varchar(10))
	declare @Unknown table(Browser varchar(100), HitCount int,Type varchar(10))


-----------Known
IF @Filter IS NOT NULL 
	INSERT INTO @Known
	SELECT 
		Browser, 
		COUNT(DomainTrackerActivityID) ,'Known'
	FROM 
		DomainTrackerActivity dta WITH(NOLOCK)
		join DomainTrackerUserProfile dtup with(nolock) on dta.ProfileID = dtup.ProfileID
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
ELSE
	INSERT INTO @Known
	SELECT 
		Browser, 
		COUNT(DomainTrackerActivityID),'Known'
	FROM 
		DomainTrackerActivity dta WITH(NOLOCK)
		join DomainTrackerUserProfile dtup with(nolock) on dta.ProfileID = dtup.ProfileID
	WHERE
		DomainTrackerID = @DomainTrackerID
		AND TimeStamp >= @startDate
		AND TimeStamp <= @endDate
		AND ISNULL(dtup.IsKnown,1) = 1
	GROUP BY 
		Browser
	ORDER BY 
		COUNT(DomainTrackerActivityID) DESC

-----------UNKnown
IF @Filter IS NOT NULL 
	INSERT INTO @Unknown
	SELECT 
		Browser, 
		COUNT(DomainTrackerActivityID) ,'Unknown'
	FROM 
		DomainTrackerActivity dta WITH(NOLOCK)
		join DomainTrackerUserProfile dtup with(nolock) on dta.ProfileID = dtup.ProfileID
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
ELSE
	INSERT INTO @Unknown
	SELECT 
		Browser, 
		COUNT(DomainTrackerActivityID),'Unknown'
	FROM 
		DomainTrackerActivity dta WITH(NOLOCK)
		join DomainTrackerUserProfile dtup with(nolock) on dta.ProfileID = dtup.ProfileID
	WHERE
		DomainTrackerID = @DomainTrackerID
		AND TimeStamp >= @startDate
		AND TimeStamp <= @endDate
		AND dtup.IsKnown = 0
	GROUP BY 
		Browser
	ORDER BY 
		COUNT(DomainTrackerActivityID) DESC


insert into @Known(Browser, HitCount, Type)
Select Browser, HitCount, Type
from @Unknown

declare @Final table(Browser varchar(100), Known int, Unknown int, Total int)

insert into @Final(Browser, Known, Unknown, Total)
Select Browser, case when Type = 'Known' then SUM(hitCount) end as 'Known', case when Type = 'Unknown' then SUM(hitCOunt) end as 'Unknown', 0
FROM @Known
Group By Browser, Type

select Browser, SUM(ISNULL(Known,0)) as 'Known', SUM(ISNULL(Unknown,0)) as 'Unknown', SUM(ISNULL(Known,0)) + SUM(ISNULL(Unknown,0)) as 'Total'
FROM @Final
GROUP BY Browser