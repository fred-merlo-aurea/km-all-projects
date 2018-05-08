CREATE PROCEDURE [dbo].[e_DomainTrackerActivity_GetTotalViews]
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
		COUNT(DomainTrackerID) 
	FROM
		DomainTrackerActivity dta WITH(NOLOCK)
		join DomainTrackerUserProfile dtup with(nolock) on dta.ProfileID = dtup.ProfileID
	WHERE 
		DomainTrackerID = @DomainTrackerID
		AND PageURL LIKE '%' + @Filter + '%' 
		AND TimeStamp >=  @startDate
		AND TimeStamp <=  @endDate
		AND ISNULL(dtup.IsKnown,1) = 1
	END
	else if @TypeFilter = 'unknown'
	BEGIN
		SELECT 
			COUNT(DomainTrackerID) 
		FROM
			DomainTrackerActivity dta WITH(NOLOCK)
			join DomainTrackerUserProfile dtup with(nolock) on dta.ProfileID = dtup.ProfileID
		WHERE 
			DomainTrackerID = @DomainTrackerID
			AND PageURL LIKE '%' + @Filter + '%' 
			AND TimeStamp >=  @startDate
			AND TimeStamp <=  @endDate
			AND dtup.IsKnown = 0
	END
	else
	BEGIN
		SELECT 
			COUNT(DomainTrackerID) 
		FROM
			DomainTrackerActivity dta WITH(NOLOCK)
			
		WHERE 
			DomainTrackerID = @DomainTrackerID
			AND PageURL LIKE '%' + @Filter + '%' 
			AND TimeStamp >=  @startDate
			AND TimeStamp <=  @endDate
			
	END
ELSE
	if @TypeFilter = 'known'
	BEGIN
		SELECT 
			COUNT(DomainTrackerID) 
		FROM
			DomainTrackerActivity dta WITH(NOLOCK)
			join DomainTrackerUserProfile dtup with(nolock) on dta.ProfileID = dtup.ProfileID
		WHERE 
			DomainTrackerID=@DomainTrackerID
			AND TimeStamp >= @startDate
			AND TimeStamp <=  @endDate
			AND ISNULL(dtup.IsKnown,1) = 1
	END
	else if @TypeFilter = 'unknown'
	BEGIN
		SELECT 
			COUNT(DomainTrackerID) 
		FROM
			DomainTrackerActivity dta WITH(NOLOCK)
			join DomainTrackerUserProfile dtup with(nolock) on dta.ProfileID = dtup.ProfileID
		WHERE 
			DomainTrackerID=@DomainTrackerID
			AND TimeStamp >= @startDate
			AND TimeStamp <=  @endDate
			AND dtup.IsKnown = 0
	END
	else
	BEGIN
		SELECT 
		COUNT(DomainTrackerID) 
		FROM
			DomainTrackerActivity WITH(NOLOCK)
		WHERE 
			DomainTrackerID=@DomainTrackerID
			AND TimeStamp >= @startDate
			AND TimeStamp <=  @endDate
	END
GO

GRANT EXECUTE
    ON OBJECT::[dbo].[e_DomainTrackerActivity_GetTotalViews] TO [ecn5]
    AS [dbo];

