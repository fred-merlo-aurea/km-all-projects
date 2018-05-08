
CREATE PROCEDURE [dbo].[e_DomainTrackerActivity_GetURLStats]

@DomainTrackerID INT,
@StartDate DATETIME,
@EndDate DATETIME,
@Filter  NVARCHAR(MAX) = '',
@TopRow INTEGER = 5,
@TypeFilter varchar(50)

AS

SET NOCOUNT ON 

IF ISNULL(@Filter,'') = ''
    IF @TopRow = 0
		if @TypeFilter = 'Known'
		BEGIN
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
		END
		Else if @TypeFilter = 'Unknown'
		BEGIN
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
		else
		BEGIN
			SELECT   
				PageURL, 
				COUNT(DomainTrackerActivityID)  AS 'Counts' 
			FROM 
				DomainTrackerActivity dta WITH (NOLOCK)			
			WHERE 
				DomainTrackerID=@DomainTrackerID 
				AND TimeStamp BETWEEN @StartDate AND @EndDate			
			GROUP BY 
				PageURL
			ORDER BY 
				COUNT(DomainTrackerActivityID) DESC
		END
	ELSE
		if @TypeFilter = 'known'
		BEGIN
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
		END
		else if @TypeFilter = 'unknown'
		BEGIN
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
		else
		BEGIN
			SELECT 
				TOP (@TopRow)  
				PageURL, 
				COUNT(DomainTrackerActivityID)  AS 'Counts' 
			FROM 
				DomainTrackerActivity dta WITH (NOLOCK)				
			WHERE 
				DomainTrackerID=@DomainTrackerID 
				AND TimeStamp BETWEEN @StartDate AND @EndDate				
			GROUP BY 
				PageURL
			ORDER BY
				COUNT(DomainTrackerActivityID) DESC 
		END
ELSE 
    IF @TopRow = 0
		if @TypeFilter = 'known'
		BEGIN
			SELECT
				PageURL, COUNT(DomainTrackerActivityID)  AS 'Counts' 
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
		END
		else if @TypeFilter = 'unknown'
		BEGIN
			SELECT
				PageURL, COUNT(DomainTrackerActivityID)  AS 'Counts' 
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
		else
		BEGIN
			SELECT
				PageURL, COUNT(DomainTrackerActivityID)  AS 'Counts' 
			FROM 
				DomainTrackerActivity dta WITH (NOLOCK)				
			WHERE 
				DomainTrackerID=@DomainTrackerID 
				AND TimeStamp BETWEEN @StartDate AND @EndDate
				AND pageURL LIKE '%' + @Filter + '%' 				
			GROUP BY 
				PageURL
			ORDER BY 
				COUNT(DomainTrackerActivityID) DESC
		END
	ELSE 
		if @TypeFilter = 'known'
		BEGIN
			SELECT 
				TOP (@TopRow)  
				PageURL, 
				COUNT(DomainTrackerActivityID)  AS 'Counts' 
			FROM 
				DomainTrackerActivity dta WITH (NOLOCK)
				join DomainTrackerUserProfile dtup with(nolock) on dta.ProfileID = dtup.ProfileID
			WHERE 
				DomainTrackerID = @DomainTrackerID 
				AND TimeStamp BETWEEN @StartDate AND @EndDate
				AND pageURL LIKE '%' + @Filter + '%' 
				AND ISNULL(dtup.IsKnown,1) = 1
			GROUP BY 
				PageURL
			ORDER BY 
				COUNT(DomainTrackerActivityID) DESC
		END
		else if @TypeFilter = 'unknown'
		BEGIN
			SELECT 
				TOP (@TopRow)  
				PageURL, 
				COUNT(DomainTrackerActivityID)  AS 'Counts' 
			FROM 
				DomainTrackerActivity dta WITH (NOLOCK)
				join DomainTrackerUserProfile dtup with(nolock) on dta.ProfileID = dtup.ProfileID
			WHERE 
				DomainTrackerID = @DomainTrackerID 
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
				DomainTrackerActivity dta WITH (NOLOCK)
				
			WHERE 
				DomainTrackerID = @DomainTrackerID 
				AND TimeStamp BETWEEN @StartDate AND @EndDate
				AND pageURL LIKE '%' + @Filter + '%' 
				
			GROUP BY 
				PageURL
			ORDER BY 
				COUNT(DomainTrackerActivityID) DESC
		END