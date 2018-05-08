CREATE PROCEDURE [dbo].[v_ChampionAuditReport] 

@StartDate date,
@EndDate date,
@CustomerID int
AS
SET NOCOUNT ON

BEGIN

declare @blasts table (blastID int, sendtime datetime, EmailSubject varchar(1000), CampaignItemName varchar(1000), sampleID int, CampaignItemType varchar(20), layoutName  varchar(1000))

insert into @blasts
select b.blastID,  b.SendTime, b.EmailSubject, ci.CampaignItemName, ci.SampleID, CampaignItemType, l.LayoutName from ecn5_communicator..blast b with (NOLOCK) 
join ecn5_communicator..CampaignItemBlast cib on cib.BlastID = b.BlastID
join ecn5_communicator..CampaignItem ci on ci.CampaignItemID = cib.CampaignItemID 
join ecn5_communicator..Campaign c on c.CampaignID = ci.CampaignID
left outer join ecn5_communicator..groups g on b.groupID = g.groupID  
left outer join ecn5_communicator..filter f on b.FilterID = f.FilterID and f.IsDeleted=0
left outer join ecn5_communicator..Layout l on l.LayoutID = b.LayoutID
where b.customerID = @customerID 
		and CAST(b.sendtime as date) between @startdate and @enddate 
		and statuscode = 'sent' and b.testblast='N'
		and c.IsDeleted=0 and CampaignItemType in ('Champion')
 
insert into @blasts
select b.blastID,  b.SendTime, b.EmailSubject, ci.CampaignItemName, ci.SampleID, CampaignItemType, l.LayoutName from ecn5_communicator..blast b with (NOLOCK) 
join ecn5_communicator..CampaignItemBlast cib on cib.BlastID = b.BlastID
join ecn5_communicator..CampaignItem ci on ci.CampaignItemID = cib.CampaignItemID 
join ecn5_communicator..Campaign c on c.CampaignID = ci.CampaignID
left outer join ecn5_communicator..groups g on b.groupID = g.groupID  
left outer join ecn5_communicator..filter f on b.FilterID = f.FilterID and f.IsDeleted=0
left outer join ecn5_communicator..Layout l on l.LayoutID = b.LayoutID
where b.customerID = @customerID 
		and b.SampleID in (select SampleID from @blasts) 
		and statuscode = 'sent' and b.testblast='N'
		and c.IsDeleted=0 and CampaignItemType in ('ab')


declare @BlastActivity Table (BlastID int, tcounts int, ucounts int, actiontypecode varchar(50))

	insert into @BlastActivity
	SELECT bao.BlastID, COUNT(bao.OpenID), COUNT(DISTINCT bao.EmailID),'open'
		FROM BlastActivityOpens bao WITH (NOLOCK) join @blasts b on bao.BlastID = b.blastID where CampaignItemType = 'Champion'
		GROUP BY bao.BlastID   
	UNION 
		SELECT bas.BlastID, COUNT(bas.SendID), COUNT(DISTINCT bas.EmailID), 'send'
		FROM BlastActivitySends bas WITH (NOLOCK) join @blasts b on bas.BlastID = b.blastID --where CampaignItemType = 'Champion'
		GROUP BY bas.BlastID   
	UNION 
		SELECT	bab.BlastID, COUNT(bab.BounceID), COUNT(DISTINCT bab.EmailID), 'bounce'
		FROM BlastActivityBounces bab WITH (NOLOCK) JOIN BounceCodes bc WITH (NOLOCK) ON bab.BounceCodeID = bc.BounceCodeID join @blasts b on bab.BlastID = b.blastID where CampaignItemType = 'Champion'
		GROUP BY bab.BlastID
	UNION 
		SELECT bau.BlastID, COUNT(bau.UnsubscribeID), COUNT(DISTINCT bau.EmailID), 'subscribe' 
		FROM BlastActivityUnSubscribes bau WITH (NOLOCK) JOIN UnsubscribeCodes uc WITH (NOLOCK) on bau.UnsubscribeCodeID = uc.UnsubscribeCodeID join @blasts b on bau.BlastID = b.blastID
		WHERE uc.UnsubscribeCode = 'subscribe' --and CampaignItemType = 'Champion'
		GROUP BY bau.BlastID
	UNION
		SELECT bac.BlastID, COUNT(bac.ClickID), COUNT(DISTINCT bac.EmailID), 'click' 
		FROM BlastActivityClicks bac WITH (NOLOCK) join @blasts b on bac.BlastID = b.blastID where CampaignItemType = 'Champion'
		GROUP BY bac.BlastID
	UNION
		SELECT bar.BlastID,  COUNT(bar.ReferID), ISNULL(COUNT( DISTINCT EmailID),0), 'forward'
		FROM BlastActivityRefer bar WITH (NOLOCK) join @blasts b on bar.BlastID = b.blastID --where CampaignItemType = 'Champion'
		GROUP BY bar.BlastID
	UNION
		Select BlastIDA, 0, OpensA, 'open'
		From ecn5_communicator.dbo.ChampionAudit ca WITH (NOLOCK) join @blasts b on b.blastID = ca.BlastIDChampion where CampaignItemType = 'champion'
	Union
		Select BlastIDB, 0, OpensB, 'open'
		From ecn5_communicator.dbo.ChampionAudit ca WITH (NOLOCK) join @blasts b on b.blastID = ca.BlastIDChampion where CampaignItemType = 'champion'
	UNION
		Select BlastIDA, 0, ClicksA, 'click'
		From ecn5_communicator.dbo.ChampionAudit ca WITH (NOLOCK) join @blasts b on b.blastID = ca.BlastIDChampion where CampaignItemType = 'champion'
	Union
		Select BlastIDB, 0, ClicksB, 'click'
		From ecn5_communicator.dbo.ChampionAudit ca WITH (NOLOCK) join @blasts b on b.blastID = ca.BlastIDChampion where CampaignItemType = 'champion'
	UNION
		Select BlastIDA, 0, BouncesA, 'bounce'
		From ecn5_communicator.dbo.ChampionAudit ca WITH (NOLOCK) join @blasts b on b.blastID = ca.BlastIDChampion where CampaignItemType = 'champion'
	Union
		Select BlastIDB, 0, BouncesB, 'bounce'
		From ecn5_communicator.dbo.ChampionAudit ca WITH (NOLOCK) join @blasts b on b.blastID = ca.BlastIDChampion where CampaignItemType = 'champion';

		WITH AB_CTE (BlastID, SendTime, CampaignItemName, EmailSubject, SampleID, CampaignItemType, LayoutName, SendTotal, Opens, Clicks, Bounce, Unsubscribe, Forward, Delivered)
		AS
		( 
			 select  
			 b1.BlastID, 
			 b1.SendTime, 
			 b1.CampaignItemName, 
			 b1.EmailSubject, 
			 SampleID, 
			 CampaignItemType, 
			 b1.layoutName,
			sum(case when  actiontypecode='send' then ucounts else 0 end) as SendTotal,  
			sum(case when  actiontypecode='open' then ucounts else 0 end) as Opens,
			sum(case when  actiontypecode='click' then ucounts else 0 end) as Clicks,  
			sum(case when  actiontypecode='bounce' then ucounts else 0 end) as Bounce,  
			sum(case when  actiontypecode='subscribe' then ucounts else 0 end) as Unsubscribe,
			sum(case when  actiontypecode='forward' then ucounts else 0 end) as Forward,
			sum(case when  actiontypecode='send' then ucounts else 0 end) - 
			sum(case when  actiontypecode='bounce' then ucounts else 0 end) as  Delivered
			 from  
			 @BlastActivity ba join @blasts b1 on ba.blastID = b1.blastID 
			 group by b1.BlastID, b1.sendtime, b1.CampaignItemName, b1.EmailSubject, b1.SampleID, b1.CampaignItemType, b1.layoutName
		)
		select BlastID, SendTime, CampaignItemName, EmailSubject, AB_CTE.SampleID, CampaignItemType, LayoutName, SendTotal, Opens, Clicks, Bounce, Unsubscribe, Forward, Delivered,
		Case when
			AB_CTE.campaignItemType = 'ab' and AB_CTE.BlastID = ca.BlastIDWinning then 1 else 0 end as 'Winner'
		from AB_CTE join ecn5_communicator.dbo.ChampionAudit ca on AB_CTE.SampleID = ca.SampleID

END