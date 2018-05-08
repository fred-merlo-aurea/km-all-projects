CREATE PROCEDURE [dbo].[e_DomainTrackerFields_Select_FieldValuePairs]
@DomainTrackerID int,
@ProfileID int,
@StartDate varchar(MAX) = NULL,
@EndDate varchar(MAX) = NULL,
@PageUrl varchar(MAX) = null
AS

DECLARE @SelectSQL varchar(4000)
SET @SelectSQL = 'SELECT dta.DomainTrackerActivityID, DTF.FieldName, DTV.Value, dta.ReferralURL, DTV.CreatedDate
	FROM
		DomainTrackerActivity dta with (nolock)
		join DomainTrackerFields dtf with (nolock) on dta.DomainTrackerID = dtf.DomainTrackerID and dtf.IsDeleted = 0
		join DomainTrackerValue dtv with (nolock) on dtf.DomainTrackerFieldsID = dtv.DomainTrackerFieldsID and dta.DomainTrackerActivityID = dtv.DomainTrackerActivityID and dtv.IsDeleted = 0
	WHERE
		dta.DomainTrackerID ='+convert(varchar(100), @DomainTrackerID)+' and
		dta.ProfileID ='+convert(varchar(100), @ProfileID)+''
		
IF @StartDate IS NOT NULL AND @EndDate IS NOT NULL
BEGIN
	SET @SelectSQL = @SelectSQL + ' AND DTV.CreatedDate between ''' + @StartDate + ''' AND ''' + @EndDate + ''''
END			

if @PageUrl is not null
BEGIN
	Set @SelectSQL = @SelectSQL + ' AND dta.PageURL = ''' + @PageUrl + ''' '
END

SET @SelectSQL = @SelectSQL + ' ORDER BY dta.DomainTrackerActivityID DESC '

EXEC(@SelectSQL)
