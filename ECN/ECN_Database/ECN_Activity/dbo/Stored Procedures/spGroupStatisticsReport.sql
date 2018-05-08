-- [spGroupStatisticsReport] 160782, "04/01/2014", "06/01/2014"
CREATE procedure [dbo].[spGroupStatisticsReport]
(
	@groupID int,
	@startdate date,
	@enddate date
)
as

Begin
	--DECLARE @groupID int = 237884,
	--@startdate varchar(20) = '3/23/2015',
	--@enddate varchar(20) = '3/24/2015'
	set nocount on

-- print '0 - start '+ ' / ' + convert(varchar(30), getdate() ,113 )

	declare @blasts Table (BlastID int, EmailSubject varchar(1000), sendtime datetime,  CampaignName varchar(250), UNIQUE CLUSTERED (BlastID), Filter varchar(500))
	
	insert into @blasts
	SELECT  
			b.BlastID, b.EmailSubject, b.sendtime, c.CampaignName,''
	FROM	
			ecn5_communicator.dbo.[BLAST] b 
			join ecn5_communicator.dbo.CampaignItemBlast cib on b.BlastID = cib.BlastId 
			--LEFT OUTER JOIN ECN5_COMMUNICATOR..CampaignItemBlastFilter cibf with(nolock) on cib.CampaignItemBlastID = cibf.CampaignItemBlastID and cibf.IsDeleted = 0
			--LEFT OUTER JOIN ECN5_COMMUNICATOR..SmartSegment ss with(nolock) on cibf.SmartSegmentID = ss.SmartSegmentID
			join ecn5_communicator.dbo.CampaignItem ci on cib.CampaignItemID = ci.CampaignItemID 
			join Ecn5_communicator..Campaign c on c.CampaignID = ci.CampaignID
			--left outer join ECN5_COMMUNICATOR..Filter f WITH (NOLOCK) on cibf.FilterID = f.FilterID 
	WHERE	
			b.groupID = @groupID and 
			statuscode = 'sent' and 
			b.testblast='N' and 
			CAST(b.sendtime as date) between @startdate and @enddate; 
			
	DECLARE @Filters Table(BlastID int, Filter varchar(1000))
	INSERT INTO @Filters (BlastID, Filter)
	SELECT cib.BlastID,STUFF( Case WHEN cibf.SmartSegmentID is not null THEN ss.SmartSegmentName + ' for blasts(' + cibf.refBlastIDs + '), '
									WHEN cibf.FilterID is not null THEN f.FilterName + ', ' END,1,0,'')
	FROM
	ECN5_COMMUNICATOR..Blast b with(nolock) 
	join ecn5_communicator..CampaignItemBlast cib with(nolock) on b.BlastID = cib.BlastID
	LEFT OUTER JOIN ecn5_communicator..CampaignItemBlastFilter cibf with(nolock) on cib.CampaignItemBlastID = cibf.CampaignItemBlastID and cibf.IsDeleted = 0
	LEFT OUTER JOIN ecn5_communicator..SmartSegment ss with(nolock) on cibf.SmartSegmentID = ss.SmartSegmentID
	LEFT OUTER JOIN ecn5_communicator..Filter f with(nolock) on cibf.FilterID = f.FilterID
	WHERE cib.GroupID = @groupID and
			statuscode = 'sent' and 
			b.testblast='N' and 
			CAST(b.sendtime as date) between @startdate and @enddate 			

-- print '1 - after ins blast '+ ' / ' + convert(varchar(30), getdate() ,113 )
			
	declare @cte table (BlastID int, actiontype varchar(20), UniqueCount int, TotalCount int, UNIQUE CLUSTERED (BlastID, actiontype))

	insert into @cte
	SELECT b.BlastID, 'send' as ActionTypeCode, ISNULL(COUNT( DISTINCT EmailID),0) AS UniqueCount, ISNULL(COUNT(SendID),0) AS TotalCount
	FROM BlastActivitySends bas WITH (NOLOCK) JOIN @blasts b on b.blastID = bas.blastID 
	GROUP BY  b.BlastID 

-- print '1.1 - after send '+ ' / ' + convert(varchar(30), getdate() ,113 )
	
	insert into @cte
	SELECT b.BlastID, 'bounce' as ActionTypeCode, ISNULL(COUNT( DISTINCT EmailID),0) AS UniqueCount, ISNULL(COUNT(BounceID),0) AS TotalCount
	FROM BlastActivityBounces bab WITH (NOLOCK) JOIN @blasts b on b.blastID = bab.blastID
	GROUP BY  b.BlastID   

-- print '1.2 - after bounce '+ ' / ' + convert(varchar(30), getdate() ,113 )
	
	insert into @cte
	SELECT b.BlastID, 'open' as ActionTypeCode, ISNULL(COUNT( DISTINCT EmailID),0) AS UniqueCount, ISNULL(COUNT(OpenID),0) AS TotalCount
	FROM BlastActivityOpens bao WITH (NOLOCK) JOIN @blasts b on b.blastID = bao.blastID
	GROUP BY  b.BlastID

-- print '1.3 - after open '+ ' / ' + convert(varchar(30), getdate() ,113 )
	
	insert into @cte
	SELECT b.BlastID, 'subscribe' as ActionTypeCode, ISNULL(COUNT( DISTINCT EmailID),0) AS UniqueCount, ISNULL(COUNT(UnsubscribeID),0) AS TotalCount
	FROM BlastActivityUnSubscribes bau WITH (NOLOCK) JOIN UnsubscribeCodes uc WITH (NOLOCK) on bau.UnsubscribeCodeID = uc.UnsubscribeCodeID JOIN @blasts b on b.blastID = bau.blastID
	WHERE uc.UnsubscribeCode = 'subscribe'
	GROUP BY  b.BlastID 
	
-- print '1.4 - after subscribe '+ ' / ' + convert(varchar(30), getdate() ,113 )
	
	insert into @cte
	SELECT b.BlastID, 'refer' as ActionTypeCode, ISNULL(COUNT( DISTINCT EmailID),0) AS UniqueCount, ISNULL(COUNT(ReferID),0) AS TotalCount
	FROM BlastActivityRefer bar WITH (NOLOCK) JOIN @blasts b on b.blastID = bar.blastID
	GROUP BY  b.BlastID

	insert into @cte
	SELECT b.BlastID, 'suppression' as ActionTypeCode, ISNULL(COUNT( DISTINCT EmailID),0) AS UniqueCount, ISNULL(COUNT(SuppressID),0) AS TotalCount
	FROM BlastActivitySuppressed bas WITH (NOLOCK) JOIN @blasts b on b.blastID = bas.blastID 
	GROUP BY  b.BlastID 

-- print '1.5 - after refer '+ ' / ' + convert(varchar(30), getdate() ,113 )
	
	insert into @cte
	SELECT  
			inn.BlastID,
			'click' as ActionTypeCode,
			ISNULL(SUM(DistinctCount),0) AS UniqueCount, 
			ISNULL(SUM(total),0) AS TotalCount       
	FROM (        
			SELECT  b.BlastID, COUNT(distinct URL) AS DistinctCount, COUNT(ClickID) AS total         
			FROM   BlastActivityClicks bac WITH (NOLOCK) JOIN @blasts b on b.blastID = bac.blastID
			GROUP BY  b.BlastID, URL, EmailID        
		) AS inn   
	GROUP BY BlastID

	insert into @cte
	SELECT 
		b.BlastID,
		'clickthrough' as ActionTypeCode,
		ISNULL(COUNT(distinct bac.EmailID),0) as UniqueCount,
		0
	FROM BlastActivityClicks bac with(nolock)
	JOIN @blasts b on bac.BlastID = b.BlastID
	GROUP BY b.BlastID

-- print '1.6 - after click '+ ' / ' + convert(varchar(30), getdate() ,113 )
	
	select	r.blastID, EmailSubject, sendtime, CampaignName, SUBSTRING(f.Filter,0, LEN(f.Filter)) as 'Filter',
			isnull(max(case when actiontype='send' then uniquecount end),0) as usend,
			isnull(max(case when actiontype='send' then TotalCount end),0) as tsend,
			isnull(max(case when actiontype='bounce' then uniquecount end),0) as ubounce,
			isnull(max(case when actiontype='bounce' then TotalCount end),0) as tbounce,
			isnull(max(case when actiontype='open' then uniquecount end),0) as uopen,
			isnull(max(case when actiontype='open' then TotalCount end),0) as topen,
			isnull(max(case when actiontype='click' then uniquecount end),0) as uClick,
			isnull(max(case when actiontype='click' then TotalCount end),0) as tClick,
			isnull(max(case when actiontype='subscribe' then uniquecount end),0) as uUnsubscribe,
			isnull(max(case when actiontype='subscribe' then TotalCount end),0) as tUnsubscribe,
			isnull(max(case when actiontype='refer' then uniquecount end),0) as uRefer,
			isnull(max(case when actiontype='refer' then TotalCount end),0) as tRefer,
			isnull(max(case when actiontype='suppression' then TotalCount end),0) as Suppressed,
			isnull(max(case when actiontype='clickthrough' then UniqueCount end),0) as ClickThrough
	FROM
			@cte r 
			join @blasts b on r.blastID = b.BlastID
			left outer join @Filters f on r.BlastID = f.BlastID
	group by 
			r.blastID, 
			EmailSubject, 
			sendtime,  
			CampaignName,
			f.Filter
	order by 
			sendtime asc;		

-- print '3 - END '+ ' / ' + convert(varchar(30), getdate() ,113 )				
			
	/* report CTE to improve performance - 3/14/2012 */

	--WITH Report_CTE (BlastID, actiontype, UniqueCount , TotalCount)
	--AS
	--(
	--	SELECT b.BlastID, 'send' as ActionTypeCode, ISNULL(COUNT( DISTINCT EmailID),0) AS UniqueCount, ISNULL(COUNT(SendID),0) AS TotalCount
	--	FROM BlastActivitySends bas WITH (NOLOCK) JOIN @blasts b on b.blastID = bas.blastID 
	--	GROUP BY  b.BlastID 
	--	UNION 
	--	SELECT b.BlastID, 'bounce' as ActionTypeCode, ISNULL(COUNT( DISTINCT EmailID),0) AS UniqueCount, ISNULL(COUNT(BounceID),0) AS TotalCount
	--	FROM BlastActivityBounces bab WITH (NOLOCK) JOIN @blasts b on b.blastID = bab.blastID
	--	GROUP BY  b.BlastID   
	--	UNION 
	--	SELECT b.BlastID, 'open' as ActionTypeCode, ISNULL(COUNT( DISTINCT EmailID),0) AS UniqueCount, ISNULL(COUNT(OpenID),0) AS TotalCount
	--	FROM BlastActivityOpens bao WITH (NOLOCK) JOIN @blasts b on b.blastID = bao.blastID
	--	GROUP BY  b.BlastID
	--	UNION 
	--	SELECT b.BlastID, 'subscribe' as ActionTypeCode, ISNULL(COUNT( DISTINCT EmailID),0) AS UniqueCount, ISNULL(COUNT(UnsubscribeID),0) AS TotalCount
	--	FROM BlastActivityUnSubscribes bau WITH (NOLOCK) JOIN UnsubscribeCodes uc WITH (NOLOCK) on bau.UnsubscribeCodeID = uc.UnsubscribeCodeID JOIN @blasts b on b.blastID = bau.blastID
	--	WHERE uc.UnsubscribeCode = 'subscribe'
	--	GROUP BY  b.BlastID 
	--	UNION 
	--	SELECT b.BlastID, 'refer' as ActionTypeCode, ISNULL(COUNT( DISTINCT EmailID),0) AS UniqueCount, ISNULL(COUNT(ReferID),0) AS TotalCount
	--	FROM BlastActivityRefer bar WITH (NOLOCK) JOIN @blasts b on b.blastID = bar.blastID
	--	GROUP BY  b.BlastID
	--	--UNION
	--	--SELECT  
	--	--		inn.BlastID,
	--	--		'click' as ActionTypeCode,
	--	--		ISNULL(SUM(DistinctCount),0) AS UniqueCount, 
	--	--		ISNULL(SUM(total),0) AS TotalCount       
	--	--FROM (        
	--	--		SELECT  b.BlastID, COUNT(distinct URL) AS DistinctCount, COUNT(ClickID) AS total         
	--	--		FROM   BlastActivityClicks bac WITH (NOLOCK) JOIN @blasts b on b.blastID = bac.blastID
	--	--		GROUP BY  b.BlastID, URL, EmailID        
	--	--	) AS inn   
	--	--GROUP BY BlastID
	--) 
	--select	r.blastID, EmailSubject, sendtime, BlastCategory,
	--		isnull(max(case when actiontype='send' then uniquecount end),0) as usend,
	--		isnull(max(case when actiontype='send' then TotalCount end),0) as tsend,
	--		isnull(max(case when actiontype='bounce' then uniquecount end),0) as ubounce,
	--		isnull(max(case when actiontype='bounce' then TotalCount end),0) as tbounce,
	--		isnull(max(case when actiontype='open' then uniquecount end),0) as uopen,
	--		isnull(max(case when actiontype='open' then TotalCount end),0) as topen,
	--		isnull(max(case when actiontype='click' then uniquecount end),0) as uClick,
	--		isnull(max(case when actiontype='click' then TotalCount end),0) as tClick,
	--		isnull(max(case when actiontype='subscribe' then uniquecount end),0) as uUnsubscribe,
	--		isnull(max(case when actiontype='subscribe' then TotalCount end),0) as tUnsubscribe,
	--		isnull(max(case when actiontype='refer' then uniquecount end),0) as uRefer,
	--		isnull(max(case when actiontype='refer' then TotalCount end),0) as tRefer
	--FROM
	--		Report_CTE r join @blasts b on r.blastID = b.BlastID
	--group by 
	--		r.blastID, 
	--		EmailSubject, 
	--		sendtime,  
	--		BlastCategory
	--order by 
	--		sendtime asc;
End
