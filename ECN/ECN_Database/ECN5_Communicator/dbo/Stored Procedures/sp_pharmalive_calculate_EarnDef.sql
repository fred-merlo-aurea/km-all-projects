CREATE proc [dbo].[sp_pharmalive_calculate_EarnDef]
as	
Begin
	declare 
			@startdate varchar(20),
			@enddate varchar(20)
		
	set @startdate = convert(varchar(10), dateadd(mm, -1, dateadd(dd, -1 * (day(getdate())-1), getdate())), 101) + ' 00:00:00'
	set @enddate = convert(varchar(10), dateadd(dd, -1 * (day(getdate())), getdate()), 101) + ' 23:59:59'

	create table #tmpreport (emailID int, groupID int, entryID varchar(40), startdate datetime, enddate datetime, amountpaid decimal(10,2), earnedamount decimal(10,2), Deferredamount decimal(10,2), Totalsent int, EarnedThisMonth decimal(10,2), Adjustments int, totalmonths int , Remainingmonths int)

	insert into #tmpreport (emailID, groupID,  entryID,  startdate, enddate, amountpaid, earnedamount, Deferredamount, Totalsent, totalmonths, Remainingmonths, adjustments)
	select emailID, groupID,  entryID,  startdate, enddate, isnull(amountpaid,0), isnull(earnedamount,0), isnull(Deferredamount,0), isnull(Totalsent,0), datediff(mm, startdate,enddate), datediff(mm, startdate,enddate)- datediff(mm, startdate,@enddate), 0 from 
	(select	distinct emailID, gdf.groupID, entryID,
			max(case when shortname='startdate' then datavalue end) as startdate,
			max(case when shortname='enddate' then datavalue end) as enddate,
			max(case when shortname='amountpaid' then datavalue end) as amountpaid,
			max(case when shortname='earnedamount' then datavalue end) as earnedamount,
			max(case when shortname='Deferredamount' then datavalue end) as Deferredamount,
			max(case when shortname='TotalSent' then datavalue end) as TotalSent,
			(select datavalue from emaildatavalues edv1 join groupdatafields gdf1 on edv1.groupdatafieldsID  = gdf1.groupdatafieldsID  where emailID = edv.emailID and groupID = gdf.groupID and shortname ='paidorfree') as PaidOrFree
	from	emaildatavalues edv join 
			groupdatafields gdf on edv.groupdatafieldsID = gdf.groupdatafieldsID
	where 
			gdf.groupID in (14092,14093,14095,14096,14098,14104,14105,16201,16585) --) and 
			and entryID is not null and 
			shortname in ('startdate','enddate','amountpaid','earnedamount','Deferredamount','TotalSent')
	group by emailID, gdf.groupID, entryID
	) inn
	where isnull(convert(decimal,amountpaid),0) > 0
			 and paidorfree = 'PAID' 	

	--delete from #tmpreport where entryid is null or (dateadd(m, 1, enddate) < @enddate  or startdate >@enddate)
	delete from #tmpreport where entryid is null or (enddate < @enddate  or startdate >@enddate)

/*
	update #tmpreport
	set earnedAmount = 0,
		deferredamount = 0,	
		TotalSent = 0
*/

	update #tmpreport
	set		Adjustments = AdjAmount
	from #tmpreport t join
	(
		select emailID, groupID,  TransEntryID, sum(convert(decimal(10,2), isnull(adjamount,0))) as adjamount from
		(
		select	edv.emailID, gdf.groupID, edv.entryID, 
				max(case when shortname='TransEntryID' then datavalue end) as TransEntryID,
				max(case when shortname='AdjDate' then datavalue end) as AdjDate,
				max(case when shortname='AdjType' then datavalue end) as AdjType,
				max(case when shortname='AdjAmount' then datavalue end) as AdjAmount,
				max(case when shortname='AdjExpDate' then datavalue end) as AdjExpDate,
				max(case when shortname='AdjDesc' then datavalue end) as AdjDesc
		from	emaildatavalues edv join 
				groupdatafields gdf on edv.groupdatafieldsID = gdf.groupdatafieldsID join
				#tmpreport t on t.emailID = edv.emailID and t.groupID = gdf.groupID 
		where 
				gdf.groupID in (14092,14093,14095,14096,14098,14104,14105,16201,16585) --) and 
				and edv.entryID is not null and 
				shortname in ('TransEntryID','AdjDate','AdjType','AdjAmount','AdjExpDate','AdjDesc')
		group by edv.emailID, gdf.groupID, edv.entryID
		having convert(datetime,max(case when shortname='AdjDate' then datavalue end)) between  @startdate and @enddate and
				max(case when shortname='TransEntryID' then datavalue end) in (select entryID from #tmpreport)
		) inn
		where isnull(convert(decimal,AdjAmount),0) > 0 and AdjType = 'DISCOUNT' 
		group by emailID, groupID,  TransEntryID
	) inn2
	on t.emailID = inn2.emailID and t.groupID = inn2.groupID and t.entryID = inn2.TransEntryID
	
	update #tmpreport
	set EarnedThisMonth = ((case when deferredamount = 0 then amountpaid else deferredamount end) - isnull(Adjustments,0)) / RemainingMonths

	update #tmpreport
	set earnedAmount = earnedAmount + EarnedThisMonth,
		deferredamount = (case when deferredamount = 0 then amountpaid else deferredamount end) - isnull(Adjustments,0) - EarnedThisMonth,	
		TotalSent = totalmonths - remainingmonths + 1

	update emaildatavalues
	set datavalue = t.TotalSent, ModifiedDate = getdate()
	from emaildatavalues join  #tmpreport t on 	emaildatavalues.emailID = t.emailID and emaildatavalues.EntryID = t.entryID
	and groupdatafieldsID = (select groupdatafieldsID from groupdatafields where groupID = t.groupID and shortname ='totalsent')
	
	print ('update totalsent' + convert(varchar,@@ROWCOUNT))

	update emaildatavalues
	set datavalue = t.earnedamount, ModifiedDate = getdate()
	from emaildatavalues join  #tmpreport t on 	emaildatavalues.emailID = t.emailID and emaildatavalues.EntryID = t.entryID
	and groupdatafieldsID = (select groupdatafieldsID from groupdatafields where groupID = t.groupID and shortname ='earnedamount')

	print ('update earnedamount' + convert(varchar,@@ROWCOUNT))

	update emaildatavalues
	set datavalue = t.Deferredamount, ModifiedDate = getdate()
	from emaildatavalues join  #tmpreport t on 	emaildatavalues.emailID = t.emailID and emaildatavalues.EntryID = t.entryID
	and groupdatafieldsID = (select groupdatafieldsID from groupdatafields where groupID = t.groupID and shortname ='Deferredamount')

	print ('update Deferredamount' + convert(varchar,@@ROWCOUNT))

	insert into emaildatavalues
	select t.emailID, (select groupdatafieldsID from groupdatafields where groupID = t.groupID and shortname ='totalsent'),  t.TotalSent, getdate(), -1, t.entryID
	from #tmpreport t left outer join emaildatavalues on emaildatavalues.emailID = t.emailID and emaildatavalues.EntryID = t.entryID
	and groupdatafieldsID = (select groupdatafieldsID from groupdatafields where groupID = t.groupID and shortname ='totalsent')
	where emaildatavalues.EmailDataValuesID is null

	print ('insert totalsent' + convert(varchar,@@ROWCOUNT))

	insert into emaildatavalues
	select t.emailID, (select groupdatafieldsID from groupdatafields where groupID = t.groupID and shortname ='earnedamount'),  t.earnedamount, getdate(), -1, t.entryID
	from #tmpreport t left outer join emaildatavalues on 	emaildatavalues.emailID = t.emailID and emaildatavalues.EntryID = t.entryID
	and groupdatafieldsID = (select groupdatafieldsID from groupdatafields where groupID = t.groupID and shortname ='earnedamount')
	where emaildatavalues.EmailDataValuesID is null

	print ('insert earnedamount' + convert(varchar,@@ROWCOUNT))

	insert into emaildatavalues
	select t.emailID, (select groupdatafieldsID from groupdatafields where groupID = t.groupID and shortname ='Deferredamount'),  t.Deferredamount, getdate(), -1, t.entryID
	from #tmpreport t left outer join emaildatavalues on 	emaildatavalues.emailID = t.emailID and emaildatavalues.EntryID = t.entryID
	and groupdatafieldsID = (select groupdatafieldsID from groupdatafields where groupID = t.groupID and shortname ='Deferredamount')
	where emaildatavalues.EmailDataValuesID is null

	print ('insert Deferredamount' + convert(varchar,@@ROWCOUNT))

	select * from #tmpreport

	drop table #tmpreport
End
