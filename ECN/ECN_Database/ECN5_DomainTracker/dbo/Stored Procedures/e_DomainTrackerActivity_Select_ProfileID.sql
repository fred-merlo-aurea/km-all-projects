CREATE PROCEDURE [dbo].[e_DomainTrackerActivity_Select_ProfileID]
@ProfileID int,
@DomainTrackerID int,
@StartDate varchar(MAX) = NULL,
@EndDate varchar(MAX) = NULL,
@PageUrl varchar(MAX) = null
AS

DECLARE @SelectSQL varchar(4000)
SET @SelectSQL = 'SELECT * from DomainTrackerActivity with (NOLOCK)
	WHERE ProfileID=' + convert(varchar(100),@ProfileID)+' and DomainTrackerID=' + convert(varchar(100),@DomainTrackerID)+''

IF @StartDate IS NOT NULL AND @EndDate IS NOT NULL
BEGIN
	SET @SelectSQL = @SelectSQL + ' AND TimeStamp between ''' + @StartDate + ''' AND ''' + @EndDate + ''''
END		

IF @PageUrl is not null
BEGIN
	SET @SelectSQL = @SelectSQL + ' AND PageURL = ''' + @PageUrl + ''' '
END

SET @SelectSQL = @SelectSQL + ' order by DomainTrackerActivityID desc'

EXEC(@SelectSQL)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[e_DomainTrackerActivity_Select_ProfileID] TO [ecn5]
    AS [dbo];

