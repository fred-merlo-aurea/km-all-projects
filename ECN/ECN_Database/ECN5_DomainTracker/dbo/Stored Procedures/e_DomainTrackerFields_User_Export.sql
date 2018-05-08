CREATE PROCEDURE [dbo].[e_DomainTrackerFields_User_Export]
@DomainTrackerID int,
@EmailAddress VARCHAR(500),
@StartDate varchar(MAX),
@EndDate varchar(MAX)   ,
@TypeFilter varchar(50)
AS
Begin
	declare @SQL VARCHAR(MAX)
	declare @FieldNames VARCHAR(MAX)

	SET @FieldNames = (SELECT 
                              ISNULL(STUFF((
                              SELECT  ',[' + Fieldname +']'
                              FROM DomainTrackerFields with (nolock)
                              WHERE DomainTrackerID = @DomainTrackerID AND IsDeleted = 0
                              FOR XML PATH('')) ,1,1,''),''))
	
	if @FieldNames = '' set @FieldNames = '[PivotEmpty]' --Pivot on something when @FieldsNames are empty
	
	declare @TypeSql varchar(MAX)
	if @TypeFilter = 'known'
	BEGIN
		set @TypeSql = ' and ISNULL(dtup.IsKnown,1) = 1 '
	END
	else if @TypeFilter = 'unknown'
	BEGIN
		set @TypeSql = ' and dtup.IsKnown = 0 '
	END

	SET @SQL = 
	'                            
	 select
		dta.timestamp,
		dta.DomainTrackerActivityId,
		EmailAddress, 
		PageURL, 
		IPAddress, 
		OS, 
		Browser, 
		pvt.* 
	from 
		DomainTrackerActivity  dta with (nolock)
		join DomainTrackerUserProfile dtup with (nolock) on dta.ProfileID = dtup.ProfileID and dtup.IsDeleted = 0
		LEFT JOIN 
		(
			select 
				dta.DomainTrackerActivityId, FieldName, dtv.Value
			from 
				DomainTrackerActivity  dta with (nolock)
				join DomainTrackerUserProfile dtup with (nolock) on dta.ProfileID = dtup.ProfileID and dtup.IsDeleted = 0
				join DomainTrackerFields dtf with (nolock) on dta.DomainTrackerID = dtf.DomainTrackerID and dtf.IsDeleted = 0
				join DomainTrackerValue dtv with (nolock) on dtf.DomainTrackerFieldsID = dtv.DomainTrackerFieldsID and dta.DomainTrackerActivityID = dtv.DomainTrackerActivityID and dtv.IsDeleted = 0
			where
				dta.DomainTrackerID = ' + Convert(varchar(10),@DomainTrackerID) + ' and
				dtup.EmailAddress like ''%' + @EmailAddress + '%'' and
				dta.timestamp between ''' + @StartDate + ''' and ''' + @EndDate + '''
		) p 
		PIVOT (MIN(Value)
		FOR 
		   FieldName IN (' + @FieldNames + ')) AS PVT on DTA.DomainTrackerActivityId = pvt.DomainTrackerActivityId--testing only
	where 
		DomainTrackerID = ' + Convert(varchar(10),@DomainTrackerID) + ' and
		dtup.EmailAddress like ''%' + @EmailAddress + '%'' and
		dta.timestamp between ''' + @StartDate + ''' and ''' + @EndDate + '''' + @TypeSql + '
	order by 
		dtup.EmailAddress asc                          
	' 
	--PRINT @SQL                            
	EXEC (@SQL)
End
GO


