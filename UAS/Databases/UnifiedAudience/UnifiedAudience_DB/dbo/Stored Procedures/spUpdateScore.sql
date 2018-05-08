CREATE proc [dbo].[spUpdateScore]
as
BEGIN
	
	SET NOCOUNT ON

	update Pubs 
		set Score = 1 
		where ISNULL(score, 0) = 0
	
	create table #tmpscore (subscriptionID int primary key , score int)

	if (DB_NAME() = 'ANTHEMMASTERDB')
	Begin
		--custom - unique click activity
		insert into #tmpscore
		select sq1.SubscriptionID, isnull(c1,0) + isnull(c2,0) + isnull(c3,0)
		from
		(
			select  ps.subscriptionID, isnull(SUM(p.Score), 0) as c1
			from 
					PubSubscriptions ps with (NOLOCK) join			
					Pubs p with (NOLOCK) on p.PubID = ps.PubID
			group by ps.subscriptionID
		) as sq1
		left outer join 
		(
			select  ps.subscriptionID, isnull(COUNT(distinct CONVERT(VARCHAR(100),sc.pubSubscriptionID) + CONVERT(VARCHAR(100),link) + CONVERT(VARCHAR(100),sc.ActivityDate)), 0) as c2
			from 			
					PubSubscriptions ps with (NOLOCK)  join 
					SubscriberClickActivity sc with (NOLOCK) on ps.PubSubscriptionID = sc.PubSubscriptionID and ISNULL(link,'') <> ''
			group by ps.subscriptionID
		) as sq2
		on sq1.SubscriptionID=sq2.SubscriptionID
		left outer join 
		(
			select  ps.subscriptionID, isnull(COUNT(distinct OpenActivityID), 0) as c3
			from 			
					PubSubscriptions ps with (NOLOCK)  join 
					SubscriberOpenActivity so with (NOLOCK) on ps.PubSubscriptionID = so.PubSubscriptionID
			group by ps.subscriptionID
		) as sq3
		on sq1.SubscriptionID=sq3.SubscriptionID
	End
	--else if (DB_NAME() = 'CanonMasterDB')
	--Begin

	--	Update Subscriptions 
	--	set Score = isnull(inn.PB_RCP,0) + (isnull(inn.EP_RCP,0)*2) +((isnull(inn.CF_ATD,0))*5)  + ((isnull(inn.CF_RGT,0))*4) +
	--				(isnull(inn.WC_RGT,0) * 3) + ((isnull(inn.WC_ATD,0))*4)  + ((isnull(inn.SH_RGT,0))*4) +
	--				(isnull(inn.SH_ATD,0) * 5) + ((isnull(inn.CLCK_ACT,0))*3)  + ((isnull(inn.OPN_ACT,0))*3) + (isnull(inn.swipe_ACT,0)*5)
	--	from subscriptions s1 join 
	--	(
	--		select sq0.SubscriptionID, PB_RCP, EP_RCP,CF_ATD, CF_RGT, WC_RGT, WC_ATD, SH_RGT, SH_ATD, CLCK_ACT, OPN_ACT, swipe_ACT  from 
	--		(
	--		(
	--			select distinct s.SubscriptionID from Subscriptions s with (NOLOCK)
	--			left outer join PubSubscriptions ps with (NOLOCK) 
	--			on s.SubscriptionID=ps.SubscriptionID
	--			join Pubs p with (NOLOCK)
	--			on ps.PubID=p.PubID join (select pubID from CanonDataMine..pubs where PriceGroupID = 1) p1 on p.PubID = p1.PubID  
	--			) as sq0
	--			left outer join 
	--			(
	--			select s.SubscriptionID, isnull(COUNT(p.PubID),0) as PB_RCP  
	--			from Subscriptions s with (NOLOCK)
	--			left outer join PubSubscriptions ps  with (NOLOCK)on s.SubscriptionID=ps.SubscriptionID
	--			join Pubs p with (NOLOCK) on ps.PubID=p.PubID 
	--			JOIN PubTypes pt with (NOLOCK) on pt.PubTypeID=p.PubTypeID 
	--			join (select pubID from CanonDataMine..pubs where PriceGroupID = 1) p1 on p.PubID = p1.PubID  
	--			where pt.PubTypeDisplayName ='PUB'
	--			group by s.subscriptionID
	--			) as sq1 on sq0.SubscriptionID=sq1.SubscriptionID
	--			left outer join 
	--			(
	--			select s.SubscriptionID, isnull(COUNT(p.PubID),0) as EP_RCP   
	--			from Subscriptions s with (NOLOCK)
	--			join PubSubscriptions ps  with (NOLOCK)on s.SubscriptionID=ps.SubscriptionID
	--			join Pubs p with (NOLOCK)	on ps.PubID=p.PubID 
	--			JOIN PubTypes pt with (NOLOCK) on pt.PubTypeID=p.PubTypeID 
	--			join (select pubID from CanonDataMine..pubs where PriceGroupID = 1) p1 on p.PubID = p1.PubID  
	--			where pt.PubTypeDisplayName = 'EPROD'--'ENEWS'
	--			group by s.subscriptionID
	--			) as sq2
	--			on sq0.SubscriptionID=sq2.SubscriptionID
	--			left outer join 
	--			(
	--			select s.SubscriptionID, COUNT(case when ps.PubTransactionID=38 then p.PubID end) as CF_RGT , COUNT(case when ps.PubTransactionID=10 then p.PubID end) as CF_ATD  
	--			from Subscriptions s with (NOLOCK)
	--			join PubSubscriptions ps  with (NOLOCK)	on s.SubscriptionID=ps.SubscriptionID
	--			join  Pubs p with (NOLOCK)on ps.PubID=p.PubID 
	--			JOIN PubTypes pt with (NOLOCK) on pt.PubTypeID=p.PubTypeID 
	--			join (select pubID from CanonDataMine..pubs where PriceGroupID = 1) p1 on p.PubID = p1.PubID
	--			where CategoryID=10
	--			and pt.PubTypeDisplayName = 'CONF' --='CONFERENCE'
	--			group by s.subscriptionID
	--			) as sq3
	--			on sq0.SubscriptionID=sq3.SubscriptionID
	--			left outer join 
	--			(
	--			select s.SubscriptionID, COUNT(case when ps.PubTransactionID=38 then p.PubID end) as WC_RGT , COUNT(case when ps.PubTransactionID=10 then p.PubID end) as WC_ATD  
	--			from Subscriptions s with (NOLOCK)
	--			join PubSubscriptions ps  with (NOLOCK)on s.SubscriptionID=ps.SubscriptionID
	--			join  Pubs p with (NOLOCK)on ps.PubID=p.PubID 
	--			JOIN PubTypes pt with (NOLOCK) on pt.PubTypeID=p.PubTypeID 
	--			join (select pubID from CanonDataMine..pubs where PriceGroupID = 1) p1 on p.PubID = p1.PubID
	--			where CategoryID=10
	--			and pt.PubTypeDisplayName = 'Webinar' --='WEBCAST'
	--			group by s.subscriptionID
	--			) as sq4
	--			on sq0.SubscriptionID=sq4.SubscriptionID
	--			left outer join 
	--			(
	--			select s.SubscriptionID, COUNT(case when ps.PubTransactionID=38 then p.PubID end) as SH_RGT , COUNT(case when ps.PubTransactionID=10 then p.PubID end) as SH_ATD  
	--			from Subscriptions s with (NOLOCK)
	--			join PubSubscriptions ps  with (NOLOCK) on s.SubscriptionID=ps.SubscriptionID
	--			join  Pubs p with (NOLOCK)on ps.PubID=p.PubID 
	--			JOIN PubTypes pt with (NOLOCK) on pt.PubTypeID=p.PubTypeID 
	--			join (select pubID from CanonDataMine..pubs where PriceGroupID = 1) p1 on p.PubID = p1.PubID
	--			where CategoryID=10
	--			and pt.PubTypeDisplayName='SHOW'
	--			group by s.subscriptionID
	--			) as sq5
	--			on sq0.SubscriptionID=sq5.SubscriptionID
	--			left outer join 
	--			(
	--			select  ps.subscriptionID, COUNT(distinct ClickActivityID) as CLCK_ACT
	--			from PubSubscriptions ps  with (NOLOCK) join 
	--			SubscriberClickActivity sc  with (NOLOCK) on ps.PubSubscriptionID = sc.PubSubscriptionID 
	--			where ISNULL(link,'') <> '' 
	--			group by ps.subscriptionID
	--			) as sq6
	--			on sq0.SubscriptionID=sq6.SubscriptionID
	--			left outer join
	--			(
	--				select s.SubscriptionID , COUNT(sdID) as Swipe_Act from SubscriptionDetails sd  with (NOLOCK) join subscriptions s  with (NOLOCK) on sd.SubscriptionID = s.SubscriptionID 
	--				where MasterID in (select MasterID from Mastercodesheet where MasterGroupID = 17)
	--				group by s.subscriptionID
	--			) as sq7
	--			on sq0.SubscriptionID=sq7.SubscriptionID
	--			left outer join
	--			(
	--				select  ps.subscriptionID, COUNT(distinct OpenActivityID) as OPN_ACT
	--				from PubSubscriptions ps  with (NOLOCK)  join 
	--				SubscriberOpenActivity so  with (NOLOCK) on ps.PubSubscriptionID = so.PubSubscriptionID
	--				group by ps.subscriptionID
	--			) sq8
	--			on sq0.SubscriptionID=sq8.SubscriptionID
	--		) 	
	--	) inn on s1.SubscriptionID = inn.SubscriptionID	
	--End
	else if (DB_NAME() = 'LEBHARMASTERDB')
	Begin
		--custom - Add adhoc count
		insert into #tmpscore
		select sq1.SubscriptionID, isnull(c1,0) + isnull(c2,0) + isnull(c3,0) + isnull(c4,0)
		from
		(
			select  ps.subscriptionID, isnull(SUM(p.Score), 0) as c1
			from 
					PubSubscriptions ps with (NOLOCK) join			
					Pubs p with (NOLOCK) on p.PubID = ps.PubID
			group by ps.subscriptionID
		) as sq1
		left outer join 
		(
			select  ps.subscriptionID, isnull(COUNT(distinct ClickActivityID), 0) as c2
			from 			
					PubSubscriptions ps with (NOLOCK)  join 
					SubscriberClickActivity sc with (NOLOCK) on ps.PubSubscriptionID = sc.PubSubscriptionID and ISNULL(link,'') <> ''
			group by ps.subscriptionID
		) as sq2
		on sq1.SubscriptionID=sq2.SubscriptionID
		left outer join 
		(
			select  ps.subscriptionID, isnull(COUNT(distinct OpenActivityID), 0) as c3
			from 			
					PubSubscriptions ps with (NOLOCK)  join 
					SubscriberOpenActivity so with (NOLOCK) on ps.PubSubscriptionID = so.PubSubscriptionID
			group by ps.subscriptionID
		) as sq3
		on sq1.SubscriptionID=sq3.SubscriptionID
		left outer join 
		(
			select  se.subscriptionID, CASE WHEN ISNULL(Field1,'') != '' THEN 1 ELSE 0 END as c4
			from 			
					SubscriptionsExtension se
		) as sq4
		on sq1.SubscriptionID=sq4.SubscriptionID
	End	
	else if (DB_NAME() = 'NORTHSTARMASTERDB')
	Begin
		insert into #tmpscore
		select sq1.SubscriptionID, isnull(c0,0) + isnull(c1,0) + isnull(c2,0) + isnull(c3,0) + isnull(c4,0)
		 FROM 
		(
			(
				select SubscriptionID, 
				(CASE WHEN EmailExists = 1 THEN 1 ELSE 0 END +
				CASE WHEN ISNULL(Fname,'') != '' AND  ISNULL(Lname,'') != '' THEN 1 ELSE 0 END +
				CASE WHEN ISNULL(Phone,'') != '' THEN 1 ELSE 0 END+
				CASE WHEN ISNULL(Mailpermission,'') = 1  THEN 1 ELSE 0 END+
				CASE WHEN ISNULL(State,'') != '' THEN 1 ELSE 0 END) as c0
				from 
					Subscriptions with (NOLOCK)
			) as sq0
			JOIN
			(
				select  ps.subscriptionID, isnull(SUM(p.Score), 0) as c1
				from 
						PubSubscriptions ps with (NOLOCK) join			
						Pubs p with (NOLOCK) on p.PubID = ps.PubID
				group by ps.subscriptionID
			) as sq1 on sq0.subscriptionID = sq1.SubscriptionID
			left outer join 
			(
				select  ps.subscriptionID, isnull(COUNT(distinct ClickActivityID), 0) as c2
				from 			
						PubSubscriptions ps with (NOLOCK) join 
						SubscriberClickActivity sc with (NOLOCK) on ps.PubSubscriptionID = sc.PubSubscriptionID and ISNULL(link,'') <> ''
				group by ps.subscriptionID
			) as sq2 on sq1.SubscriptionID=sq2.SubscriptionID
			left outer join 
			(
				select  ps.subscriptionID, isnull(COUNT(distinct OpenActivityID), 0) as c3
				from 			
						PubSubscriptions ps with (NOLOCK) join 
						SubscriberOpenActivity so with (NOLOCK) on ps.PubSubscriptionID = so.PubSubscriptionID
				group by ps.subscriptionID
			) as sq3 on sq1.SubscriptionID=sq3.SubscriptionID
			LEFT OUTER JOIN 
			(
				SELECT
					s.SUBSCRIPTIONID,
					COUNT(Distinct(rg.ResponseGroupID)) AS c4
				 FROM 
					Pubs p WITH (NOLOCK)
					JOIN ResponseGroups rg WITH (NOLOCK) ON p.PubID = rg.PubID
					JOIN CodeSheet cs  WITH (NOLOCK)  ON cs.PubID = p.PubID AND cs.ResponseGroupID = rg.ResponseGroupID
					JOIN PubSubscriptions ps  WITH (NOLOCK) ON p.PubID = ps.pubid 
					JOIN Subscriptions s  WITH (NOLOCK) ON s.SubscriptionID = ps.SubscriptionID
					JOIN PubSubscriptionDetail psd  WITH (NOLOCK) ON psd.PubSubscriptionID = ps.PubSubscriptionID and psd.CodesheetID = cs.CodeSheetID 
					
				 WHERE
					ISNULL(ResponseValue,'') NOT IN ('','Z')
					AND (
						(Pubcode = 'PRINT_MC' AND ResponseGroupName IN ('PLANNING','JOB_RESPONSIBILITIES','FACILITIES_USED','MEETINGS_OUTSIDE_US'))
						OR
						(Pubcode = 'PRINT_BTN' AND ResponseGroupName IN ('FUNCTION','SERVICES','ARRANGE_TRAVEL','TRIPS_OUTSIDE_US'))
						OR
						(Pubcode = 'PRINT_TW' AND ResponseGroupName IN ('FUNCTION','SALES','Affiliated','SERVICE_PURCHASED'))
						OR
						(Pubcode = 'PRINT_TAW' AND ResponseGroupName IN ('SALES','AFFILIATED','SERVICES_PURCHASED','ASSOCIATIONS'))
						OR
						(Pubcode = 'PRINT_INC' AND ResponseGroupName IN ('INCENTIVES','RESPONSIBILITY','EMPLOYEES','AWARDS'))
						OR
						(Pubcode = 'PRINT_SM' AND ResponseGroupName IN ('AREAS_OUTSIDE_US','STATES_MEETINGS_HELD','OFFSITE_MEETING_SIZE','TYPES_OF_FACILITIES'))
						)
				 GROUP BY s.SubscriptionID 
 			) AS Sq4 ON sq1.SubscriptionID = Sq4.SubscriptionID
		) 		
	End	
	else if (DB_NAME() = 'MEISTERMASTERDB')
	Begin
		insert into #tmpscore
		select sq1.SubscriptionID, isnull(c1,0) + (isnull(c2,0)*2) + isnull(c3,0) + isnull(c4,0)
		from
		(
			select  ps.subscriptionID, isnull(SUM(p.Score), 0) as c1,
					isnull(SUM(case when c.CodeValue in ('A','B','Q') then 3 
									when c.CodeValue in ('C','D','R','E','F','G','H','S','I','J','K','L','M','N') then 1 end), 0) as c4
			from 
					PubSubscriptions ps with (NOLOCK) join			
					Pubs p with (NOLOCK) on p.PubID = ps.PubID left outer join
					UAD_Lookup..Code c on c.CodeId = ps.PubQSourceID
			group by ps.subscriptionID
		) as sq1
		left outer join 
		(
			select  ps.subscriptionID, isnull(COUNT(distinct ClickActivityID), 0) as c2
			from 			
					PubSubscriptions ps with (NOLOCK) join 
					SubscriberClickActivity sc with (NOLOCK) on ps.PubSubscriptionID = sc.PubSubscriptionID and ISNULL(link,'') <> ''
			group by ps.subscriptionID
		) as sq2
		on sq1.SubscriptionID=sq2.SubscriptionID
		left outer join 
		(
			select  ps.subscriptionID, isnull(COUNT(distinct OpenActivityID), 0) as c3
			from 			
					PubSubscriptions ps with (NOLOCK)  join 
					SubscriberOpenActivity so with (NOLOCK) on ps.PubSubscriptionID = so.PubSubscriptionID
			group by ps.subscriptionID
		) as sq3
		on sq1.SubscriptionID=sq3.SubscriptionID
	End
	Else if (DB_NAME() = 'BRIEFMEDIAMASTERDB')
	Begin
		insert into #tmpscore
		select sq1.SubscriptionID, isnull(c1,0) + isnull(c2,0) + isnull(c3,0)
		from
		(
			select  ps.subscriptionID, isnull(SUM(p.Score), 0) as c1
			from 
					PubSubscriptions ps with (NOLOCK) join			
					Pubs p with (NOLOCK) on p.PubID = ps.PubID
			group by ps.subscriptionID
		) as sq1
		left outer join 
		(
			select  ps.subscriptionID, isnull(COUNT(distinct ClickActivityID), 0) * 2 as c2
			from 			
					PubSubscriptions ps with (NOLOCK) join 
					SubscriberClickActivity sc with (NOLOCK) on ps.PubSubscriptionID = sc.PubSubscriptionID and ISNULL(link,'') <> ''
			group by ps.subscriptionID
		) as sq2
		on sq1.SubscriptionID=sq2.SubscriptionID
		left outer join 
		(
			select  ps.subscriptionID, isnull(COUNT(distinct OpenActivityID), 0) as c3
			from 			
					PubSubscriptions ps with (NOLOCK)  join 
					SubscriberOpenActivity so with (NOLOCK) on ps.PubSubscriptionID = so.PubSubscriptionID
			group by ps.subscriptionID
		) as sq3
		on sq1.SubscriptionID=sq3.SubscriptionID
	End
	Else if (DB_NAME() = 'WATTMASTERDB' OR DB_NAME() = 'WATTMASTERDB_TEST')
		Begin
			declare @DayNumberTodayMinus90days int = master.dbo.fn_GetDateDaysFromDate(dateadd(day, -90, GETDATE())),
					@TodayMinues6Months date = dateadd(MONTH, -6, GETDATE()),
					@TodayMinues1Year date = dateadd(YEAR, -1, GETDATE())

			insert into #tmpscore
			select s.subscriptionID, isnull(SUM(x.score),0)
			from Subscriptions s left outer join
			(
				-- 1 point per opened newsletter in the past 3 months
				select  ps.subscriptionID, COUNT(distinct blastID) as score 
				from 			
						PubSubscriptions ps with (NOLOCK)  join 
						Pubs p with (NOLOCK) on ps.pubID = p.pubID join
						SubscriberOpenActivity so with (NOLOCK) on ps.PubSubscriptionID = so.PubSubscriptionID
				where
						p.PubtypeID = 1 and so.DateNumber > @DayNumberTodayMinus90days
				group by ps.subscriptionID

				union
				-- 1 point per Custom Product Subscription	
				-- 1 point per Custom Product Subscription
				-- 1 point per Registered Webinar in the past 6 months
				-- 1 point per Active Magazine Qualification within the last 12 months
				-- 1 point per Registered or Attended a live event within last 12 months
				-- 1 point per Report/Subscription purchased in last 12 months
				select  ps.subscriptionID, COUNT(PubsubscriptionID)  
				from 			
						PubSubscriptions ps with (NOLOCK)  join 
						Pubs p with (NOLOCK) on ps.pubID = p.pubID 
				where
						(p.PubtypeID = 4) or
						(p.PubtypeID in (3,8) and ps.Qualificationdate >= @TodayMinues6Months) or
						(p.PubtypeID in (5,10) and ps.Qualificationdate >= @TodayMinues1Year) or
						(p.PubtypeID = 1 and ps.Qualificationdate >= @TodayMinues1Year and PubTransactionID in (SELECT transactionCodeID FROM [UAD_Lookup].[dbo].[TransactionCode] where TransactionCodeTypeID in (1,3) and IsActive = 1))
				group by ps.subscriptionID
				union
				--	1 point per 3 page views per website in the last 3 months
				select subscriptionID, SUM(score) from
				(
					select  subscriptionID, domaintrackingID, COUNT(VisitActivityID)/3 as score 
						from 			
								SubscriberVisitActivity sv with (NOLOCK)
						where
								sv.DateNumber > @DayNumberTodayMinus90days
					group by 	subscriptionID, domaintrackingID		
					having COUNT(VisitActivityID) >= 3
				) y
				group by subscriptionID
			) x on s.subscriptionID = x.subscriptionID
			group by s.subscriptionID


		End		
	Else if (DB_NAME() = 'STAGNITOMASTERDB')
	Begin
		insert into #tmpscore
		select sq1.SubscriptionID, isnull(c1,0) + isnull(c2,0) + isnull(c3,0)
		from
		Subscriptions s with (NOLOCK) join
		(
			select  ps.subscriptionID, isnull(SUM(p.Score), 0) as c1
			from 
					PubSubscriptions ps with (NOLOCK) join			
					Pubs p with (NOLOCK) on p.PubID = ps.PubID
			group by ps.subscriptionID
		) as sq1
		on s.subscriptionID = sq1.subscriptionID
		left outer join 
		(
			select  ps.subscriptionID, isnull(COUNT(distinct ClickActivityID), 0) as c2
			from 			
					PubSubscriptions ps with (NOLOCK) join 
					SubscriberClickActivity sc with (NOLOCK) on ps.PubSubscriptionID = sc.PubSubscriptionID and ISNULL(link,'') <> ''
			group by ps.subscriptionID
		) as sq2
		on sq1.SubscriptionID=sq2.SubscriptionID
		left outer join 
		(
			select  ps.subscriptionID, isnull(COUNT(distinct OpenActivityID), 0) as c3
			from 			
					PubSubscriptions ps with (NOLOCK)  join 
					SubscriberOpenActivity so with (NOLOCK) on ps.PubSubscriptionID = so.PubSubscriptionID
			group by ps.subscriptionID
		) as sq3
		on sq1.SubscriptionID=sq3.SubscriptionID
		where
					not (isnull(Email,'') like '%@stagnitomail.com' or isnull(Email,'') like '%@ensembleiq.com' or isnull(Email,'') like '%@edgellmail.com' or 
		isnull(Company,'') like '%Stagnito%' or isnull(Company,'') like '%EIQ%' or isnull(Company,'') like '%Ensemble%' or isnull(Company,'') like '%Edgell%' )
	End
	Else
	Begin
		insert into #tmpscore
		select sq1.SubscriptionID, isnull(c1,0) + isnull(c2,0) + isnull(c3,0)
		from
		(
			select  ps.subscriptionID, isnull(SUM(p.Score), 0) as c1
			from 
					PubSubscriptions ps with (NOLOCK) join			
					Pubs p with (NOLOCK) on p.PubID = ps.PubID
			group by ps.subscriptionID
		) as sq1
		left outer join 
		(
			select  ps.subscriptionID, isnull(COUNT(distinct ClickActivityID), 0) as c2
			from 			
					PubSubscriptions ps with (NOLOCK) join 
					SubscriberClickActivity sc with (NOLOCK) on ps.PubSubscriptionID = sc.PubSubscriptionID and ISNULL(link,'') <> ''
			group by ps.subscriptionID
		) as sq2
		on sq1.SubscriptionID=sq2.SubscriptionID
		left outer join 
		(
			select  ps.subscriptionID, isnull(COUNT(distinct OpenActivityID), 0) as c3
			from 			
					PubSubscriptions ps with (NOLOCK)  join 
					SubscriberOpenActivity so with (NOLOCK) on ps.PubSubscriptionID = so.PubSubscriptionID
			group by ps.subscriptionID
		) as sq3
		on sq1.SubscriptionID=sq3.SubscriptionID
	End		
	
	Update S
	set s.Score = isnull(t.score,0)
	from 
		Subscriptions s left outer join #tmpscore t on s.subscriptionID = t.subscriptionID	
	where isnull(s.Score,0) <> isnull(t.score,0)

	drop table #tmpscore

END

GO