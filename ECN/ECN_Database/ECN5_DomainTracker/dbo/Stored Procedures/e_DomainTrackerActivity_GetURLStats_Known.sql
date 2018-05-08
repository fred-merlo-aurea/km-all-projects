CREATE PROCEDURE [dbo].[e_DomainTrackerActivity_GetURLStats_Known]
	@DomainTrackerID INT,
@StartDate DATETIME,
@EndDate DATETIME,
@Filter  NVARCHAR(MAX) = '',
@TopRow INTEGER = 5

AS


SET NOCOUNT ON 
	declare @Known table(PageURL varchar(MAX), [Count] int)
	declare @Unknown table(PageURL varchar(MAX), [Count] int)
	declare @Total table(PageURL varchar(MAX), [Count] int)

IF ISNULL(@Filter,'') = ''
BEGIN
    IF @TopRow = 0
	BEGIN
		insert into @Total
		SELECT   
			PageURL, 
			COUNT(DomainTrackerActivityID)  AS 'Counts' 
		FROM 
			DomainTrackerActivity WITH (NOLOCK)
		WHERE 
			DomainTrackerID=@DomainTrackerID 
			AND TimeStamp BETWEEN @StartDate AND @EndDate
		GROUP BY 
			PageURL
		ORDER BY 
			COUNT(DomainTrackerActivityID) DESC

		--KNOWN
		insert into @Known
		SELECT   
			PageURL, 
			COUNT(DomainTrackerActivityID)  AS 'Counts' 
		FROM 
			DomainTrackerActivity dta WITH (NOLOCK)
			join DomainTrackerUserProfile dtup with(nolock) on dta.ProfileID = dtup.ProfileID
		WHERE 
			DomainTrackerID=@DomainTrackerID 
			AND TimeStamp BETWEEN @StartDate AND @EndDate
			AND ISNULL(dtup.IsKnown,1) = 1
		GROUP BY 
			PageURL
		ORDER BY 
			COUNT(DomainTrackerActivityID) DESC

			--UNKNOWN
			insert into @Unknown
		SELECT   
			PageURL, 
			COUNT(DomainTrackerActivityID)  AS 'Counts' 
		FROM 
			DomainTrackerActivity dta WITH (NOLOCK)
			join DomainTrackerUserProfile dtup with(nolock) on dta.ProfileID = dtup.ProfileID
		WHERE 
			DomainTrackerID=@DomainTrackerID 
			AND TimeStamp BETWEEN @StartDate AND @EndDate
			AND dtup.IsKnown = 0
		GROUP BY 
			PageURL
		ORDER BY 
			COUNT(DomainTrackerActivityID) DESC

		END
	ELSE
	BEGIN
		insert into @Total
		SELECT 
			TOP (@TopRow)  
			PageURL, 
			COUNT(DomainTrackerActivityID)  AS 'Counts' 
		FROM 
			DomainTrackerActivity WITH (NOLOCK)
		WHERE 
			DomainTrackerID=@DomainTrackerID 
			AND TimeStamp BETWEEN @StartDate AND @EndDate
		GROUP BY 
			PageURL
		ORDER BY
			COUNT(DomainTrackerActivityID) DESC 

--KNOWN
		insert into @Known
		SELECT   
		TOP (@TopRow)  
			PageURL, 
			COUNT(DomainTrackerActivityID)  AS 'Counts' 
		FROM 
			DomainTrackerActivity dta WITH (NOLOCK)
			join DomainTrackerUserProfile dtup with(nolock) on dta.ProfileID = dtup.ProfileID
		WHERE 
			DomainTrackerID=@DomainTrackerID 
			AND TimeStamp BETWEEN @StartDate AND @EndDate
			AND ISNULL(dtup.IsKnown,1) = 1
		GROUP BY 
			PageURL
		ORDER BY 
			COUNT(DomainTrackerActivityID) DESC

			--UNKNOWN
			insert into @Unknown
		SELECT   
		TOP (@TopRow)  
			PageURL, 
			COUNT(DomainTrackerActivityID)  AS 'Counts' 
		FROM 
			DomainTrackerActivity dta WITH (NOLOCK)
			join DomainTrackerUserProfile dtup with(nolock) on dta.ProfileID = dtup.ProfileID
		WHERE 
			DomainTrackerID=@DomainTrackerID 
			AND TimeStamp BETWEEN @StartDate AND @EndDate
			AND dtup.IsKnown = 0
		GROUP BY 
			PageURL
		ORDER BY 
			COUNT(DomainTrackerActivityID) DESC

	END	
END
ELSE
BEGIN 
    IF @TopRow = 0
	BEGIN

		SELECT
			PageURL, COUNT(DomainTrackerActivityID)  AS 'Counts' 
		FROM 
			DomainTrackerActivity WITH (NOLOCK)
		WHERE 
			DomainTrackerID=@DomainTrackerID 
			AND TimeStamp BETWEEN @StartDate AND @EndDate
			AND pageURL LIKE '%' + @Filter + '%' 
		GROUP BY 
			PageURL
		ORDER BY 
			COUNT(DomainTrackerActivityID) DESC

		--KNOWN
		insert into @Known
		SELECT   
			PageURL, 
			COUNT(DomainTrackerActivityID)  AS 'Counts' 
		FROM 
			DomainTrackerActivity dta WITH (NOLOCK)
			join DomainTrackerUserProfile dtup with(nolock) on dta.ProfileID = dtup.ProfileID
		WHERE 
			DomainTrackerID=@DomainTrackerID 
			AND TimeStamp BETWEEN @StartDate AND @EndDate
			AND pageURL LIKE '%' + @Filter + '%' 
			AND ISNULL(dtup.IsKnown,1) = 1
		GROUP BY 
			PageURL
		ORDER BY 
			COUNT(DomainTrackerActivityID) DESC

			--UNKNOWN
		insert into @Unknown
		SELECT   
			PageURL, 
			COUNT(DomainTrackerActivityID)  AS 'Counts' 
		FROM 
			DomainTrackerActivity dta WITH (NOLOCK)
			join DomainTrackerUserProfile dtup with(nolock) on dta.ProfileID = dtup.ProfileID
		WHERE 
			DomainTrackerID=@DomainTrackerID 
			AND TimeStamp BETWEEN @StartDate AND @EndDate
			AND pageURL LIKE '%' + @Filter + '%' 
			AND dtup.IsKnown = 0
		GROUP BY 
			PageURL
		ORDER BY 
			COUNT(DomainTrackerActivityID) DESC
	END
	ELSE 
	BEGIN
		SELECT 
			TOP (@TopRow)  
			PageURL, 
			COUNT(DomainTrackerActivityID)  AS 'Counts' 
		FROM 
			DomainTrackerActivity WITH (NOLOCK)
		WHERE 
			DomainTrackerID = @DomainTrackerID 
			AND TimeStamp BETWEEN @StartDate AND @EndDate
			AND pageURL LIKE '%' + @Filter + '%' 
		GROUP BY 
			PageURL
		ORDER BY 
			COUNT(DomainTrackerActivityID) DESC

		--KNOWN
		insert into @Known
		SELECT   
		TOP (@TopRow) 
			PageURL, 
			COUNT(DomainTrackerActivityID)  AS 'Counts' 
		FROM 
			DomainTrackerActivity dta WITH (NOLOCK)
			join DomainTrackerUserProfile dtup with(nolock) on dta.ProfileID = dtup.ProfileID
		WHERE 
			DomainTrackerID=@DomainTrackerID 
			AND TimeStamp BETWEEN @StartDate AND @EndDate
			AND pageURL LIKE '%' + @Filter + '%' 
			AND ISNULL(dtup.IsKnown,1) = 1
		GROUP BY 
			PageURL
		ORDER BY 
			COUNT(DomainTrackerActivityID) DESC

			--UNKNOWN
		insert into @Unknown
		SELECT   
		TOP (@TopRow) 
			PageURL, 
			COUNT(DomainTrackerActivityID)  AS 'Counts' 
		FROM 
			DomainTrackerActivity dta WITH (NOLOCK)
			join DomainTrackerUserProfile dtup with(nolock) on dta.ProfileID = dtup.ProfileID
		WHERE 
			DomainTrackerID=@DomainTrackerID 
			AND TimeStamp BETWEEN @StartDate AND @EndDate
			AND pageURL LIKE '%' + @Filter + '%' 
			AND dtup.IsKnown = 0
		GROUP BY 
			PageURL
		ORDER BY 
			COUNT(DomainTrackerActivityID) DESC
	END
END

select PageURL, [Count], 'Total' as 'Type'
from @Total
UNION
select PageURL, [Count], 'Known' as 'Type' from @Known
UNION
select PageURL, [Count], 'Unknown' as 'Type' from @Unknown
order by [Type], [Count] desc
