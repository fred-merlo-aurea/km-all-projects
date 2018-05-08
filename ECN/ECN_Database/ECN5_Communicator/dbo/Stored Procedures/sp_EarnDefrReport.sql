CREATE proc [dbo].[sp_EarnDefrReport]
(
		@groupID int,
		@month int,
		@year int
)
as/*
declare @groupID int,
		@month int,
		@year int

set @groupID = 14092
set @month = 2
set @year = 2009
*/

Begin
	
	set NOCOUNT ON

	declare @ColumnSet1 varchar(8000),
			@ColumnSet2 varchar(8000),
			@SqlString  varchar(8000),
			@enddate varchar(20),
			@SqlString1  varchar(8000)

	set @enddate = convert(varchar(10),dateadd(dd, -1, dateadd(mm,1, convert(datetime,convert(varchar(10),@month) + '/01/' + convert(varchar(10),@year)))),101) + ' 23:59:59'

	set @ColumnSet1 = ''
	set @ColumnSet2 = ''
	set @SqlString = ''

	select  @ColumnSet1  = @ColumnSet1 + coalesce('max([' + RTRIM(ShortName)  + ']) as ''' + RTRIM(ShortName)  + ''',',''),
			@ColumnSet2 = @ColumnSet2 + coalesce('case when GroupDatafields.groupDataFieldsID = ' + convert(varchar(10),GroupDatafields.groupDataFieldsID) + ' then EmailDataValues.DataValue else null end as [' + ShortName  + '],', '')
	from	GroupDatafields
	where	GroupDatafields.groupID = @GroupID
			and shortname in ('startdate','enddate','amountpaid','SubType') --,'earnedamount','Deferedamount'

	set @SqlString1 = '(select emaildatavalues.emailID from emaildatavalues where groupdatafieldsID in ' 
					 + '(select groupdatafieldsID from	GroupDatafields where	GroupDatafields.groupID = ' + convert(varchar(10),@GroupID) + ' and shortname = ''subtype'')'
					 + ' and datavalue in (''new'',''renew'') and emaildatavalues.ModifiedDate <= ''' + @enddate + ''')'


	create table #tmpreport (emailID int, entryID varchar(40), totalmonths int, earnedmonths int, monthlyprice decimal(10,2), startdate datetime, enddate datetime, amountpaid decimal(10,2), earnedamount decimal(10,2), Deferedamount decimal(10,2), SubType varchar(20))

	set @SqlString = ' insert into #tmpreport (emailID, entryID, startdate, enddate, amountpaid, subtype) select InnerTable1.EmailID, InnerTable1.EntryID, ' + substring(@ColumnSet1,0,len(@ColumnSet1)) + ' from (  ' +
	+ ' select Emaildatavalues.EmailID, EmailDataValues.EntryID, ' + substring(@ColumnSet2,0,len(@ColumnSet2))
	+ ' from Groups join EmailGroups on Groups.groupID = EmailGroups.groupID join  '+
	+ ' EmailDataValues on EmailGroups.EmailID = EmailDataValues.EmailID join '+
	+ ' GroupDataFields on EmailDataValues.groupDataFieldsID = GroupDataFields.groupDataFieldsID AND GroupDataFields.GroupID = Groups.GroupID '
	+ ' where entryid is not null and emaildatavalues.ModifiedDate <= ''' + @enddate + ''' and emailgroups.SubscribeTypeCode = ''S'' and Groups.groupID = ' + convert(varchar(10),@GroupID) + ' and Emaildatavalues.emailID in ' + @SqlString1 + ') as InnerTable1 '
	+ ' Group by InnerTable1.EmailID, InnerTable1.EntryID'

	exec (@SqlString )

	update #tmpreport
	set totalmonths = DATEDIFF ( mm , startdate , enddate ),
		earnedmonths = DATEDIFF ( mm , startdate , @enddate )+1,
		monthlyprice = (amountpaid/DATEDIFF ( mm , startdate , enddate )),
		earnedamount = (amountpaid/DATEDIFF ( mm , startdate , enddate )) * (DATEDIFF ( mm , startdate , @enddate )+1),
		Deferedamount = amountpaid - ((amountpaid/DATEDIFF ( mm , startdate , enddate )) * (DATEDIFF ( mm , startdate , @enddate )+1))
 

	--select * from #tmpreport

	select 1 as sortorder, 'Previous Deferred Income' as 'desc',  sum(deferedamount) + sum(monthlyprice) - sum(case when earnedmonths=1 then amountpaid else 0 end) as amount from #tmpreport
	union
	select 2, '   New Adds', sum(amountpaid) from #tmpreport where subtype = 'new' and earnedmonths = 1
	union
	select 3, '   Renewals', isnull(sum(monthlyprice),0) from #tmpreport where subtype = 'renew'
	union
	select 4, '   Earned Income', sum(monthlyprice) from #tmpreport where subtype = 'new' 
	union
	select 5, 'Total Deferred Income', sum(deferedamount) from #tmpreport 

	drop table #tmpreport
end
