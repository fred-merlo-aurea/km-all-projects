CREATE PROCEDURE [dbo].[e_DomainTrackerUserProfile_Select_DomainTrackerID]
@DomainTrackerID int,
@CurrentPage int = null,
@PageSize int = null,
@StartDate varchar(250) = NULL,
@EndDate varchar(250) = NULL,
@Email varchar(MAX) = NULL,
@TypeFilter varchar(50),
@SortColumn varchar(50),
@SortDirection varchar(50),
@PageUrl varchar(MAX) = null
AS
DECLARE @SelectSQL varchar(4000)
IF @CurrentPage IS NOT NULL AND @PageSize IS NOT NULL
BEGIN
	
		
	SET @SelectSQL = 'WITH Results AS 
		(SELECT ROW_NUMBER() OVER (ORDER BY dtup.' + @SortColumn + ' ' + @SortDirection + ' ) AS ROWNUM,Count(dtup.ProfileID) over () AS ProfileCount,Count(dtup.ProfileID) over () AS TotalCount, dtup.* from DomainTrackerUserProfile dtup with (NOLOCK)
		join DomainTrackerActivity dta  with (NOLOCK) on dtup.ProfileID= dta.ProfileID'
END
ELSE
BEGIN
	SET @SelectSQL = 'SELECT distinct  dtup.* from DomainTrackerUserProfile dtup with (NOLOCK)
		join DomainTrackerActivity dta  with (NOLOCK) on dtup.ProfileID= dta.ProfileID'
END

IF @StartDate IS NOT NULL AND @EndDate IS NOT NULL
BEGIN
	SET @SelectSQL = @SelectSQL + ' AND TimeStamp between ''' + @startDate + ''' AND ''' + @EndDate + ''''
END		

SET @SelectSQL = @SelectSQL + ' WHERE dta.DomainTrackerID=' + convert(varchar(100),@DomainTrackerID) +' AND dtup.IsDeleted=0'

IF @Email IS NOT NULL
BEGIN
	SET @SelectSQL = @SelectSQL + ' AND EmailAddress like ''%'+@Email+'%'''
END		

if @TypeFilter = 'known'
BEGIN
	SET @SelectSQL = @SelectSQL + ' and ISNULL(IsKnown,1) = 1 '
END
else if @TypeFilter= 'unknown'
BEGIN
	SET @SelectSQL = @SelectSQL + ' and IsKnown = 0 '
END

IF @PageUrl is not null
BEGIN
	SET @SelectSQL = @SelectSQL + ' and dta.PageURL = ''' + @PageUrl + ''' '
END

IF @CurrentPage IS NOT NULL AND @PageSize IS NOT NULL
BEGIN
	SET @SelectSQL = @SelectSQL + ' group by dtup.ProfileID, dtup.BaseChannelID, dtup.CreatedDate, dtup.CreatedUserID, dtup.CustomerID, dtup.EmailAddress, dtup.IsDeleted, dtup.UpdatedDate, dtup.UpdatedUserID, dtup.ConvertedDate, dtup.IsKnown
		)
		Select distinct r.ProfileID,r.BaseChannelID, r.CreatedDate, r.CreatedUserID, r.CustomerID, r.EmailAddress, r.IsDeleted,  r.UpdatedDate, r.UpdatedUserID,r.ConvertedDate,r.ProfileCount	
		from Results r
		WHERE ROWNUM between (('+convert(varchar(100),@CurrentPage)+' ) * ' + convert(varchar(10),@PageSize)+' + 1) and ((' + convert(varchar(10),@CurrentPage)+' + 1 ) * ' + convert(varchar(10),@PageSize)+') '
END

EXEC(@SelectSQL + ' ORDER BY ' + @SortColumn + ' ' + @SortDirection )
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[e_DomainTrackerUserProfile_Select_DomainTrackerID] TO [ecn5]
    AS [dbo];

