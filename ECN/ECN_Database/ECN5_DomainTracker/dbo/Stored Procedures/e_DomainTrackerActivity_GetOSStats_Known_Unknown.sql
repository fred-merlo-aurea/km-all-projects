CREATE PROCEDURE [dbo].[e_DomainTrackerActivity_GetOSStats_Known_Unknown]
	@DomainTrackerID int,
@startDate DateTime  = NULL,
@endDate DateTime = NULL,
@Filter  nvarchar(50) = NULL 
AS

SET NOCOUNT ON

	declare @Known table(OS varchar(100), HitCount int, Type varchar(10))
	declare @Unknown table(OS varchar(100), HitCount int, Type varchar(10))

	----Known
IF @Filter IS NOT NULL 
	insert into @Known
	SELECT
		OS, 
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
		OS
	ORDER BY 
		COUNT(DomainTrackerActivityID) DESC
ELSE 
	insert into @Known
	SELECT
		OS, 
		COUNT(DomainTrackerActivityID)  ,'Known'
	FROM
		DomainTrackerActivity dta WITH(NOLOCK)
		join DomainTrackerUserProfile dtup with(nolock) on dta.ProfileID = dtup.ProfileID
	WHERE
		DomainTrackerID = @DomainTrackerID
		AND TimeStamp >= @startDate
		AND TimeStamp <= @endDate
		AND ISNULL(dtup.IsKnown,1) = 1
	GROUP BY 
		OS
	ORDER BY 
		COUNT(DomainTrackerActivityID) DESC
--------Unknown

		IF @Filter IS NOT NULL 
	insert into @Unknown
	SELECT
		OS, 
		COUNT(DomainTrackerActivityID)  ,'Unknown'
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
		OS
	ORDER BY 
		COUNT(DomainTrackerActivityID) DESC
ELSE 
	insert into @Unknown
	SELECT
		OS, 
		COUNT(DomainTrackerActivityID) ,'Unknown'
	FROM
		DomainTrackerActivity dta WITH(NOLOCK)
		join DomainTrackerUserProfile dtup with(nolock) on dta.ProfileID = dtup.ProfileID
	WHERE
		DomainTrackerID = @DomainTrackerID
		AND TimeStamp >= @startDate
		AND TimeStamp <= @endDate
		AND dtup.IsKnown = 0
	GROUP BY 
		OS
	ORDER BY 
		COUNT(DomainTrackerActivityID) DESC


insert into @Known(OS, HitCount, Type)
Select OS, HitCount, Type
from @Unknown

declare @Final table(OS varchar(100), Known int, Unknown int, Total int)

insert into @Final(OS, Known, Unknown, Total)
Select OS, case when Type = 'Known' then SUM(hitCount) end as 'Known', case when Type = 'Unknown' then SUM(hitCOunt) end as 'Unknown', 0
FROM @Known
Group By OS, Type

select OS, SUM(ISNULL(Known,0)) as 'Known', SUM(ISNULL(Unknown,0)) as 'Unknown', SUM(ISNULL(Known,0)) + SUM(ISNULL(Unknown,0)) as 'Total'
FROM @Final
GROUP BY OS
