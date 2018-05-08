CREATE proc [dbo].[sp_Pharmalive_EarnDefrReport]
(
	@groupID varchar(1000),
	@month int,
	@year int,
	@cachereport int
)
as
Begin

	set NOCOUNT ON

	declare @startdate varchar(20),
				@enddate varchar(20),
				@PDI decimal(10,2),	
				@NA decimal(10,2),			
				@NR decimal(10,2),			
				@ADJ decimal(10,2),			
				@EI decimal(10,2),			
				@CDI decimal(10,2),
				@lmonth int,
				@lyear int

	set @PDI = 0
	set @NA  = 0
	set @NR  = 0
	set @ADJ = 0
	set @EI  = 0
	set @CDI = 0

	if exists (select * from ecn_misc..pharmalive_earneddeferred where groupID in (select items from dbo.fn_split(@groupID, ',')) and month = @month and year = @year)
	Begin

		select @PDI = sum(PreviousDeferredIncome),
			   @NA  = sum(NewAdds),
			   @NR  = sum(Renewals),
			   @ADJ = sum(Adjustments),
			   @EI  = sum(EarnedIncome),
			   @CDI = sum(CurrentDeferredIncome)
		From 
			ecn_misc..pharmalive_earneddeferred where groupID in (select items from dbo.fn_split(@groupID, ',')) and month = @month and year = @year

	End
	else
	Begin
		
		if @month = 1
		Begin
			set @lmonth = 12
			set @lyear = @year -1
		End
		else
		Begin
			set @lmonth = @month-1
			set @lyear = @year
		End
	
		select @PDI = isnull(sum(CurrentDeferredIncome),0)	From 
		ecn_misc..pharmalive_earneddeferred where groupID in (select items from dbo.fn_split(@groupID, ',')) and month = @lmonth and year = @lyear

		set @startdate =  convert(varchar(10),convert(datetime,convert(varchar(10),@month) + '/01/' + convert(varchar(10),@year)),101) + ' 00:00:00'
		set @enddate = convert(varchar(10),dateadd(dd, -1, dateadd(mm,1, convert(datetime,convert(varchar(10),@month) + '/01/' + convert(varchar(10),@year)))),101) + ' 23:59:59'

		create table #tmpreport (emailID int, groupID int, entryID varchar(100), subtype varchar(50), startdate datetime, enddate datetime, 
						amountpaid decimal(10,2), PreviousmonthAdjs decimal(10,2), CurrentmonthAdjs decimal(10,2), 
						totalmonths int, earnedmonths int, monthlyprice decimal(10,2), earnedamount decimal(10,2), Deferedamount decimal(10,2))
				
		insert into #tmpreport (emailID, groupID, entryID, subtype, startdate, enddate, amountpaid, previousmonthAdjs, currentmonthAdjs)
		select inn1.emailID, inn1.groupID, inn1.entryID, inn1.Transactiontype,  inn1.startdate, inn1.enddate,  inn1.amount, 
		sum(case when inn2.DateAdded < @startdate then isnull(inn2.amount,0) else 0 end)   as previousmonthAdjs,
		sum(case when inn2.DateAdded between @startdate and @enddate then isnull(inn2.amount,0) else 0 end)   as currentmonthAdjs
		from 
		(
			select  emailID, groupID, entryID, guid, Transactiontype as subscriptiontype, Transactiontype, startdate, enddate, amount, convert(datetime,tmpPaid.DateAdded) as DateAdded from 
				(
					select	distinct emailID, 
							gdf.groupID, 
							entryID,
							entryID as Guid,
							max(case when shortname='startdate' then datavalue end) as startdate,
							max(case when shortname='enddate' then datavalue end) as enddate,
							max(case when shortname='subtype' then datavalue end) as Transactiontype,
							max(case when shortname='amountpaid' then datavalue end) as Amount,
							max(case when shortname='subtype' then edv.modifieddate end) as DateAdded,
							(select datavalue from emaildatavalues edv1 join groupdatafields gdf1 on edv1.groupdatafieldsID  = gdf1.groupdatafieldsID  where emailID = edv.emailID and groupID = gdf.groupID and shortname ='paidorfree') as PaidOrFree
					from	
							emaildatavalues edv join 
							groupdatafields gdf on edv.groupdatafieldsID = gdf.groupdatafieldsID
					where 
							gdf.groupID in (select items from dbo.fn_split(@groupID, ',')) and 
							entryID is not null and 
							shortname in ('startdate','enddate','subtype','amountpaid')
					group by 
							emailID, gdf.groupID, entryID
					having 
							max(case when shortname='subtype' then edv.modifieddate end) <= @enddate
				)	
				tmpPaid
			where 
				isnull(convert(decimal,Amount),0) > 0 
		) inn1
		left outer join
		(
			select  emailID, groupID, entryID, guid, 'Adjustment' as subscriptiontype,  Transactiontype, amount, convert(datetime,tmpAdj.DateAdded) as DateAdded from 
			(
				select	distinct emailID, gdf.groupID, entryID,
						max(case when shortname='TransEntryID' then datavalue end) as Guid,
						max(case when shortname='AdjType' then datavalue end) as Transactiontype,
						max(case when shortname='AdjAmount' then convert(decimal(18,2),isnull(datavalue,0)) end) as Amount,
						max(case when shortname='AdjDate' then datavalue end) as DateAdded
				from	
						emaildatavalues edv join 
						groupdatafields gdf on edv.groupdatafieldsID = gdf.groupdatafieldsID
				where 
						gdf.groupID in (select items from dbo.fn_split(@groupID, ',')) and 
						entryID is not null and 
						shortname in ('TransEntryID','AdjType','AdjAmount','AdjDate')
				group by 
						emailID, gdf.groupID, entryID
				having 
						max(case when shortname='AdjDate' then datavalue end) <= @enddate
			) tmpAdj 
		where isnull(convert(decimal,Amount),0) > 0 
		) inn2 on inn1.emailID=inn2.emailID and inn1.groupID = inn2.groupID and inn1.entryID = inn2.guid
		group by inn1.emailID, inn1.groupID, inn1.entryID, inn1.Transactiontype,  inn1.startdate, inn1.enddate, inn1.amount

		update #tmpreport
		set totalmonths = DATEDIFF ( mm , startdate , enddate),
			earnedmonths = DATEDIFF ( mm , startdate , @enddate )+1
			,monthlyprice = ((amountpaid - PreviousmonthAdjs - currentmonthAdjs)/DATEDIFF ( mm , startdate , enddate ))
			--,earnedamount = ((amountpaid - PreviousmonthAdjs - currentmonthAdjs) /DATEDIFF ( mm , startdate , enddate )) * (DATEDIFF ( mm , startdate , @enddate )+1),
			--Deferedamount = amountpaid - PreviousmonthAdjs - ((amountpaid/DATEDIFF ( mm , startdate , enddate )) * (DATEDIFF ( mm , startdate , @enddate )+1))

		delete from #tmpreport where enddate < @startdate

		--select * from #tmpreport

		select @NA  = isnull(sum(amountpaid),0) from #tmpreport where subtype = 'new' and earnedmonths = 1
		select @NR  = isnull(sum(amountpaid),0) from #tmpreport where subtype = 'renew'  and earnedmonths = 1
		select @ADJ = sum(currentmonthadjs) from #tmpreport
		select @EI  = sum(monthlyprice) from #tmpreport

		select @CDI = @PDI + @NA + @NR - @ADJ - @EI

		drop table #tmpreport
	end

	select 1 as sortorder, 'Previous Deferred Income' as 'desc', @PDI as amount 
	union
	select 2, '   New Adds', @NA
	union
	select 3, '   Renewals', @NR
	union
	select 4, '   Earned Income', @EI
	union
	select 5, '   Adjustments', @ADJ
	union
	select 6, 'Total Deferred Income', @CDI

	if @cachereport = 1
	Begin
		insert into ecn_misc..pharmalive_earneddeferred (groupID,[Month],[Year],PreviousDeferredIncome,NewAdds,Renewals,EarnedIncome,Adjustments,CurrentDeferredIncome)
		select @groupID, @month, @year,isnull(@PDI,0),isnull(@NA,0),isnull(@NR,0),isnull(@EI,0),isnull(@ADJ,0),isnull(@CDI,0) 
	End

end
