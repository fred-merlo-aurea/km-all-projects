Create  proc [dbo].[v_ABSummaryReport]  
(  
  @customerID int,  
  @startdate date,  
  @enddate date
)
  
as 

Begin  


set nocount on
  

Create table #blasts (blastID int, sendtime datetime, EmailSubject varchar(500), CampaignItemName varchar(500), sampleID int, CampaignItemType varchar(500), layoutName  varchar(500),ABWinnerType varchar(10))

insert into #blasts
select b.blastID,  b.SendTime, b.EmailSubject, ci.CampaignItemName, ci.SampleID, CampaignItemType, l.LayoutName, ABWinnerType 
from 
	ecn5_communicator..blast b with (NOLOCK) 
	join ecn5_communicator..CampaignItemBlast cib on cib.BlastID = b.BlastID
	join ecn5_communicator..CampaignItem ci on ci.CampaignItemID = cib.CampaignItemID 
	join ecn5_communicator..Campaign c on c.CampaignID = ci.CampaignID
	join ECN5_COMMUNICATOR..Sample s on b.SampleID = s.SampleID
	left outer join ecn5_communicator..groups g on b.groupID = g.groupID  
	left outer join ecn5_communicator..filter f on b.FilterID = f.FilterID and f.IsDeleted=0
	left outer join ecn5_communicator..Layout l on l.LayoutID = b.LayoutID
where 
	b.customerID = @customerID 
	and CAST(b.sendtime as date) between @startdate and @enddate 
	and statuscode = 'sent' 
	and b.testblast='N'
	and c.IsDeleted=0 
	and CampaignItemType in ('ab', 'Champion')

Create Table #BlastActivity (BlastID int, tcounts int, ucounts int, actiontypecode varchar(100), unique clustered (BlastID, ActionTypeCode))

	insert into #BlastActivity
	SELECT bao.BlastID, COUNT(bao.OpenID), COUNT(DISTINCT bao.EmailID),'open'
		FROM BlastActivityOpens bao WITH (NOLOCK) join #blasts b on bao.BlastID = b.blastID
		GROUP BY bao.BlastID   
	UNION 
		SELECT bas.BlastID, COUNT(bas.SendID), COUNT(DISTINCT bas.EmailID), 'send'
		FROM BlastActivitySends bas WITH (NOLOCK) join #blasts b on bas.BlastID = b.blastID
		GROUP BY bas.BlastID   
	UNION 
		SELECT	bab.BlastID, COUNT(bab.BounceID), COUNT(DISTINCT bab.EmailID), 'bounce'
		FROM BlastActivityBounces bab WITH (NOLOCK) JOIN BounceCodes bc WITH (NOLOCK) ON bab.BounceCodeID = bc.BounceCodeID join #blasts b on bab.BlastID = b.blastID
		GROUP BY bab.BlastID
	UNION 
		SELECT bau.BlastID, COUNT(bau.UnsubscribeID), COUNT(DISTINCT bau.EmailID), 'subscribe' 
		FROM BlastActivityUnSubscribes bau WITH (NOLOCK) JOIN UnsubscribeCodes uc WITH (NOLOCK) on bau.UnsubscribeCodeID = uc.UnsubscribeCodeID join #blasts b on bau.BlastID = b.blastID
		WHERE uc.UnsubscribeCode = 'subscribe'
		GROUP BY bau.BlastID
	UNION
		SELECT bac.BlastID, COUNT(bac.ClickID), COUNT(DISTINCT bac.EmailID), 'click' 
		FROM BlastActivityClicks bac WITH (NOLOCK) join #blasts b on bac.BlastID = b.blastID 
		GROUP BY bac.BlastID
	UNION
		SELECT bar.BlastID,  COUNT(bar.ReferID), ISNULL(COUNT( DISTINCT EmailID),0), 'forward'
		FROM BlastActivityRefer bar WITH (NOLOCK) join #blasts b on bar.BlastID = b.blastID 
		GROUP BY bar.BlastID    

		Create table #AB_Winner (BlastID int, SendTime Datetime, CampaignItemName varchar(500), EmailSubject varchar(500), SampleID int, CampaignItemType varchar(500), LayoutName varchar(500), SendTotal int, Opens int, Clicks int, Bounce int, Unsubscribe int, Forward int, Delivered int, ABWinnerType varchar(10), WinningPercentage decimal(12,4), Winner int)

		Insert into #AB_Winner
			 select  b1.BlastID, b1.SendTime, b1.CampaignItemName, b1.EmailSubject, SampleID, CampaignItemType, b1.layoutName,
				sum(case when  actiontypecode='send' then ucounts else 0 end) as SendTotal,  
				sum(case when  actiontypecode='open' then ucounts else 0 end) as Opens,
				sum(case when  actiontypecode='click' then ucounts else 0 end) as Clicks,  
				sum(case when  actiontypecode='bounce' then ucounts else 0 end) as Bounce,  
				sum(case when  actiontypecode='subscribe' then ucounts else 0 end) as Unsubscribe,
				sum(case when  actiontypecode='forward' then ucounts else 0 end) as Forward,
				sum(case when  actiontypecode='send' then ucounts else 0 end) - 
				sum(case when  actiontypecode='bounce' then ucounts else 0 end) as  Delivered,
				ABWinnerType,
				0,
				0
			 from  
				 #BlastActivity ba 
				 join #blasts b1 on ba.blastID = b1.blastID 
			 group by 
				b1.BlastID, 
				b1.sendtime, 
				b1.CampaignItemName, 
				b1.EmailSubject, 
				b1.SampleID, 
				b1.CampaignItemType, 
				b1.layoutName, 
				ABWinnerType
		Update 
			#AB_Winner 
		set 
			WinningPercentage = 
			Case 
				when 
					Delivered > 0 and ABWinnerType ='opens' and Opens > 0 then  ((CONVERT(DECIMAL(12,4),Opens) / Delivered)) --Winning percentage if user selected opens
				when 
					Delivered > 0 and ABWinnerType ='clicks' and Clicks > 0 then  ((CONVERT(DECIMAL(12,4),Clicks) / Delivered))-- Winning percentage if user selected clicks
				when 
					Delivered > 0 and ABWinnerType is null and Opens >= Clicks then ((CONVERT(DECIMAL(12,4),Opens) / Delivered))--Winning percentage if choice null
				when 
					Delivered > 0 and ABWinnerType is null and Clicks > Opens then ((CONVERT(DECIMAL(12,4),Clicks) / Delivered))--Winning percentage if choice null
				else
					0 end
--select * from #AB_Winner
		select 
			BlastID, 
			SendTime, 
			CampaignItemName, 
			EmailSubject, 
			SampleID, 
			CampaignItemType, 
			LayoutName, 
			SendTotal, 
			Opens, 
			Clicks, 
			Bounce, 
			Unsubscribe, 
			Forward, 
			Delivered,
			ABWinnerType,
			WinningPercentage,
			Winner -- = 0; Logic now in BusinessLayer
		from
			#AB_Winner
		order by SampleID, sendtime


drop table #Blasts
Drop table #blastactivity
drop table #AB_Winner
--select * from #blasts
--select * from #BlastActivity
end