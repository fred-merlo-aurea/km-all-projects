CREATE  proc [dbo].[sp_PromoCodeReport]
(
	@groupID varchar(1000),
	@PromoIDs varchar(500),
	@startdate varchar(20) ,
	@enddate varchar(20)
)
as
Begin
/*
set @groupID = '14092' --'14092,14093,14094,14095,14096,14097,14098,14099,14100,14101,14102,14103,14104,14105,16201,16585'
set @PromoIDs = ''
set @startdate = '01/01/2009'
set @enddate = '03/31/2009'
*/
	
	set NOCOUNT ON

	set @startdate =  @startdate + ' 00:00:00'
	set @enddate = @enddate + ' 23:59:59'

	declare @promocodes TABLE (code varchar(100))
	
	if len(ltrim(rtrim(@PromoIDs))) > 0
	Begin
		insert into @promocodes
		select Code from ecn_misc..canon_paidpub_promotions where PromotionID in (select items from dbo.fn_split(@PromoIDs, ','))

		select 'PAID' as PAIDorFREE, PromoCode, subscriptiontype as subtype, count(distinct emailID) as subscount, sum(amount) as amountpaid from 
			(
				select	distinct emailID, 
						gdf.groupID, 
						entryID,
						max(case when shortname='subtype' then datavalue end) as subscriptiontype,
						max(case when shortname='PromoCode' then datavalue end) as PromoCode,
						convert(decimal(10,2), isnull(max(case when shortname='amountpaid' then datavalue end),0)) as Amount,
						max(case when shortname='subtype' then edv.modifieddate end) as DateAdded,
						(select top 1 datavalue from emaildatavalues edv1 join groupdatafields gdf1 on edv1.groupdatafieldsID  = gdf1.groupdatafieldsID  where emailID = edv.emailID and groupID = gdf.groupID and shortname ='paidorfree') as PaidOrFree
				from	
						emaildatavalues edv join 
						groupdatafields gdf on edv.groupdatafieldsID = gdf.groupdatafieldsID
				where 
						gdf.groupID in (select items from dbo.fn_split(@groupID, ',')) and 
						entryID is not null and 
						shortname in ('subtype','amountpaid','PromoCode')
				group by 
						emailID, gdf.groupID, entryID
				having 
						max(case when shortname='subtype' then edv.modifieddate end) between @startdate and @enddate
			)	
			inn 
		where 
			promocode in (select code from @promocodes) and 
			isnull(convert(decimal,Amount),0) > 0 
			--and PromoCode in (select Code from ecn_misc..canon_paidpub_promotions where PromotionID in (' + @PromoIDs + '))
		group by PromoCode, subscriptiontype
		union all
		select PAIDorFREE as PAIDorFREE, PromoCode as PromoCode, '' as subtype, count(distinct emailID) as subscount, 0 as amountpaid from 
			(
				select	distinct emailID, 
						gdf.groupID, 
						(select top 1 datavalue from emaildatavalues edv1 join groupdatafields gdf1 on edv1.groupdatafieldsID  = gdf1.groupdatafieldsID  where emailID = edv.emailID and groupID = gdf.groupID and shortname ='paidorfree') as PaidOrFree,
						max(case when shortname='Effort_Code' then datavalue end) as PromoCode,
						max(case when shortname='Effort_Code' then edv.modifieddate end) as DateAdded
				from	
						emaildatavalues edv join 
						groupdatafields gdf on edv.groupdatafieldsID = gdf.groupdatafieldsID
				where 
						gdf.groupID in (select items from dbo.fn_split(@groupID, ',')) and 
						shortname in ('Effort_Code')
				group by 
						emailID, gdf.groupID, entryID
				having 
						--max(case when shortname='PromoCode' then datavalue end) not in ('PAID') and 
						max(case when shortname='Effort_Code' then edv.modifieddate end) between @startdate and @enddate
			)	
			inn 
		where promocode in (select code from @promocodes) and  PAIDorFREE not in ('PAID')
		group by PAIDorFREE, PromoCode

	End
	Else
	Begin
		select 'PAID' as PAIDorFREE, PromoCode, subscriptiontype as subtype, count(distinct emailID) as subscount, sum(amount) as amountpaid from 
			(
				select	distinct emailID, 
						gdf.groupID, 
						entryID,
						max(case when shortname='subtype' then datavalue end) as subscriptiontype,
						max(case when shortname='PromoCode' then datavalue end) as PromoCode,
						convert(decimal(10,2), isnull(max(case when shortname='amountpaid' then datavalue end),0)) as Amount,
						max(case when shortname='subtype' then edv.modifieddate end) as DateAdded,
						(select top 1 datavalue from emaildatavalues edv1 join groupdatafields gdf1 on edv1.groupdatafieldsID  = gdf1.groupdatafieldsID  where emailID = edv.emailID and groupID = gdf.groupID and shortname ='paidorfree') as PaidOrFree
				from	
						emaildatavalues edv join 
						groupdatafields gdf on edv.groupdatafieldsID = gdf.groupdatafieldsID
				where 
						gdf.groupID in (select items from dbo.fn_split(@groupID, ',')) and 
						entryID is not null and 
						shortname in ('subtype','amountpaid','PromoCode')
				group by 
						emailID, gdf.groupID, entryID
				having 
						max(case when shortname='subtype' then edv.modifieddate end) between @startdate and @enddate
			)	
			inn 
		where 
			isnull(convert(decimal,Amount),0) > 0 
			--and PromoCode in (select Code from ecn_misc..canon_paidpub_promotions where PromotionID in (' + @PromoIDs + '))
		group by PromoCode, subscriptiontype
		union all
		select PAIDorFREE as PAIDorFREE, PromoCode as PromoCode, '' as subtype, count(distinct emailID) as subscount, 0 as amountpaid from 
			(
				select	distinct emailID, 
						gdf.groupID, 
						(select top 1 datavalue from emaildatavalues edv1 join groupdatafields gdf1 on edv1.groupdatafieldsID  = gdf1.groupdatafieldsID  where emailID = edv.emailID and groupID = gdf.groupID and shortname ='paidorfree') as PaidOrFree,
						max(case when shortname='Effort_Code' then datavalue end) as PromoCode,
						max(case when shortname='Effort_Code' then edv.modifieddate end) as DateAdded
				from	
						emaildatavalues edv join 
						groupdatafields gdf on edv.groupdatafieldsID = gdf.groupdatafieldsID
				where 
						gdf.groupID in (select items from dbo.fn_split(@groupID, ',')) and 
						shortname in ('Effort_Code')
				group by 
						emailID, gdf.groupID, entryID
				having 
						--max(case when shortname='PromoCode' then datavalue end) not in ('PAID') and 
						max(case when shortname='Effort_Code' then edv.modifieddate end) between @startdate and @enddate
			)	
			inn 
		where PAIDorFREE not in ('PAID')
		group by PAIDorFREE, PromoCode
	end
/*
	declare @ColumnSet1 varchar(8000),
		@ColumnSet2 varchar(8000),
		@SqlString  varchar(8000),
		@SqlString1  varchar(8000)	

	set @ColumnSet1 = ''
	set @ColumnSet2 = ''
	set @SqlString = ''

	select  @ColumnSet1  = @ColumnSet1 + coalesce('max([' + RTRIM(ShortName)  + ']) as ''' + RTRIM(ShortName)  + ''',',''),
			@ColumnSet2 = @ColumnSet2 + coalesce('case when GroupDatafields.groupDataFieldsID = ' + convert(varchar(10),GroupDatafields.groupDataFieldsID) + ' then EmailDataValues.DataValue else null end as [' + ShortName  + '],', '')
	from	GroupDatafields
	where	GroupDatafields.groupID = @GroupID and shortname in ('amountpaid','SubType','PromoCode')

	set @SqlString1 = '(select emaildatavalues.emailID from emaildatavalues where groupdatafieldsID in ' 
					 + '(select groupdatafieldsID from	GroupDatafields where	GroupDatafields.groupID = ' + convert(varchar(10),@GroupID) + ' and shortname = ''subtype'')'
					 + ' and datavalue in (''new'',''renew'') and emaildatavalues.ModifiedDate between ''' + @startdate + ''' and ''' + @enddate + ' 23:59:59'')'

	set @SqlString = ' select InnerTable1.EmailID, InnerTable1.EntryID, ' + substring(@ColumnSet1,0,len(@ColumnSet1)) + ' from (  ' +
	+ ' select Emails.EmailID, EmailDataValues.EntryID, ' + substring(@ColumnSet2,0,len(@ColumnSet2))
	+ ' from Groups join EmailGroups on Groups.groupID = EmailGroups.groupID join  Emails on Emails.EmailID = EmailGroups.EmailID join '+
	+ ' EmailDataValues on Emails.EmailID = EmailDataValues.EmailID join '+
	+ ' GroupDataFields on EmailDataValues.groupDataFieldsID = GroupDataFields.groupDataFieldsID AND GroupDataFields.GroupID = Groups.GroupID '
	+ ' where entryid is not null and emaildatavalues.ModifiedDate <= ''' + @enddate + ''' and Groups.groupID = ' + convert(varchar(10),@GroupID) + ' and emails.emailID in ' + @SqlString1 + ') as InnerTable1 '

	set @SqlString = @SqlString + ' where entryID is not null Group by InnerTable1.EmailID, InnerTable1.EntryID'
	set @SqlString = 'select Promocode, SubType, count(emailID) as subscount,  SUM(CONVERT(decimal(10, 2), case when len(ltrim(rtrim(amountpaid)))<> 0 then amountpaid end)) as amountpaid from (' + @SqlString + ') inn where len(ltrim(rtrim(subtype))) <> 0 '

	if len(rtrim(ltrim(@PromoIDs))) > 0 
		set @SqlString = @SqlString + ' and PromoCode in (select Code from ecn_misc..canon_paidpub_promotions where PromotionID in (' + @PromoIDs + '))'
	
	set @SqlString = @SqlString  + ' group by Promocode,SubType '

	exec (@SqlString)
*/

end
