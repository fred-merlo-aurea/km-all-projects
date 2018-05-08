------------------------------------------------------
-- 2014-01-28 MK Changed EmailDataValueId to BIGINT
--
--
--
------------------------------------------------------

CREATE proc [dbo].[sp_Pharmalive_Reset_PAIDTRIAL_to_Free]
as
Begin
	set NOCOUNT ON

	declare @enddate varchar(20)
	set @enddate = convert(varchar(10),dateadd(dd, -1, dateadd(mm,1, convert(datetime,convert(varchar(10),month(getdate())) + '/01/' + convert(varchar(10),year(getdate()))))),101) + ' 23:59:59'

	create table #tmpreport (
		edvID  BIGINT, --int, 
		emailID int, 
		groupID int, 
		subscriptiontype varchar(100), 
		entryID varchar(40), 
		SubType varchar(20), 
		startdate datetime, 
		enddate datetime, 
		amountpaid decimal(10,2), 
		totalmonths int, 
		earnedmonths int, 
		monthlyprice decimal(10,2), 
		earnedamount decimal(10,2), 
		Deferedamount decimal(10,2))

	insert into #tmpreport  (edvID, emailID, groupID, subscriptiontype, entryID, subtype, startdate, enddate, amountpaid)
	select	emaildatavaluesID, emailID, groupID, subscriptiontype, entryID, 
			max(case when shortname='subtype' then datavalue end)  as subtype , 
			max(case when shortname='startdate' then CONVERT(DATETIME,datavalue) end) as startdate, 
			max(case when shortname='enddate' then CONVERT(DATETIME,datavalue) end) as enddate,
			max(case when shortname='amountpaid' then datavalue end) as amountpaid  
	from 
	(
		select  inn.emaildatavaluesID, inn.emailID, inn.groupID, inn.subscriptiontype, entryID, shortname, edv.datavalue 
		from	groups g  join
				groupdatafields gdf on g.groupID = gdf.groupID join
				emaildatavalues edv on edv.groupdatafieldsID = gdf.groupdatafieldsID join 
				(
					select emailID, groupID, emaildatavaluesID, datavalue as subscriptiontype from emaildatavalues edv1 join groupdatafields gdf1 on edv1.groupdatafieldsID = gdf1.groupdatafieldsID
					where	groupID in (14092,14093,14095,14096,14098) and shortname ='PaidOrFree'
							and datavalue in ('trial','paid')
				) inn on edv.emailID = inn.emailID and inn.groupID = g.groupID
		where gdf.shortname in ('startdate','enddate','subtype', 'amountpaid')
	) inn2
	group by inn2.emaildatavaluesID, emailID, groupID, subscriptiontype, entryID
	
	delete from #tmpreport where (isnull(amountpaid,0) = 0 and subscriptiontype = 'PAID') or enddate > getdate()

	if exists (select edvID from #tmpreport where enddate < getdate())
	Begin
		update emaildatavalues
		set		datavalue = 'FREE', ModifiedDate = getdate()
		where emaildatavaluesID in (select edvID from #tmpreport where subscriptiontype = 'TRIAL' and enddate < getdate())

		update #tmpreport
		set totalmonths = DATEDIFF ( mm , startdate , enddate ),
			earnedmonths = DATEDIFF ( mm , startdate , @enddate )+1,
			monthlyprice = (amountpaid/DATEDIFF ( mm , startdate , enddate )),
			earnedamount = (amountpaid/DATEDIFF ( mm , startdate , enddate )) * (DATEDIFF ( mm , startdate , @enddate )+1),
			Deferedamount = amountpaid - ((amountpaid/DATEDIFF ( mm , startdate , enddate )) * (DATEDIFF ( mm , startdate , @enddate )+1))
		where edvID in (select edvID from #tmpreport t where subscriptiontype = 'PAID' and enddate < getdate() and not exists (select emailID from #tmpreport t1 where t1.emailID = t.emailID and t1.groupID = t.groupID and startdate > getdate()))
		
		update emaildatavalues
		set		datavalue = 'FREE', ModifiedDate = getdate()
		where emaildatavaluesID in (select edvID from #tmpreport t where subscriptiontype = 'PAID' and enddate < getdate() and not exists (select emailID from #tmpreport t1 where t1.emailID = t.emailID and t1.groupID = t.groupID and startdate > getdate()))

		update emaildatavalues
		set datavalue = t.earnedmonths, ModifiedDate = getdate()
		from emaildatavalues join  #tmpreport t on 	emaildatavalues.emailID = t.emailID and emaildatavalues.EntryID = t.entryID 
		and groupdatafieldsID = (select groupdatafieldsID from groupdatafields where groupID = t.groupID and shortname ='totalsent')
		where t.subscriptiontype = 'PAID' 		

		update emaildatavalues
		set datavalue = t.earnedamount, ModifiedDate = getdate()
		from emaildatavalues join  #tmpreport t on 	emaildatavalues.emailID = t.emailID and emaildatavalues.EntryID = t.entryID
		and groupdatafieldsID = (select groupdatafieldsID from groupdatafields where groupID = t.groupID and shortname ='earnedamount')
		where t.subscriptiontype = 'PAID' 

		update emaildatavalues
		set datavalue = t.Deferedamount, ModifiedDate = getdate()
		from emaildatavalues join  #tmpreport t on 	emaildatavalues.emailID = t.emailID and emaildatavalues.EntryID = t.entryID
		and groupdatafieldsID = (select groupdatafieldsID from groupdatafields where groupID = t.groupID and shortname ='Deferredamount')
		where t.subscriptiontype = 'PAID' 

	End

	select * from #tmpreport

	drop table #tmpreport
End