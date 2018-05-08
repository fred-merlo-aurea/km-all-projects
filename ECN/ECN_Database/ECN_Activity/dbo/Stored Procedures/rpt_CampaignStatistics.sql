CREATE PROCEDURE [dbo].[rpt_CampaignStatistics]
	@CampaignID int,
	@StartDate date,
	@EndDate date,
	@GroupID int = null
AS
	
--Check to see if campaign is deleted
IF (SELECT Isdeleted FROM ECN5_Communicator.dbo.Campaign WHERE CampaignId = @CampaignID) = 1
BEGIN
	PRINT 'Campaign is Deleted'
	RETURN 1
END

DECLARE @CurrentDate DATETIME = GETDATE()
 
 
CREATE table #blasts(BlastID int, SendTime DateTime, EmailSubject varchar(255), 
					GroupName varchar(255), FilterName varchar(255), BlastCategory varchar(255), 
					FromEmail varchar(255), CampaignItemName varchar(255), CustomerName varchar(255),MessageName varchar(255))




if(@GroupID is null)
BEGIN
INSERT INTO #blasts
	SELECT 
		b.blastID,  
		b.SendTime,  
		b.EmailSubject, 
		GroupName, 
		'' as 'FilterName', 
		c.CampaignName AS BlastCategory,
		ci.FromEmail, 
		ci.CampaignItemName, 
		cu.CustomerName,
		l.LayoutName as MessageName
	FROM 
		ecn5_communicator..blast b WITH (NOLOCK) 
		JOIN ecn5_communicator..CampaignItemBlast cib ON cib.BlastID = b.BlastID
		JOIN ecn5_communicator..CampaignItem ci ON ci.CampaignItemID = cib.CampaignItemID 
		JOIN ecn5_communicator..Campaign c ON c.CampaignID = ci.CampaignID
		LEFT OUTER JOIN ecn5_communicator..groups g ON b.groupID = g.groupID  
		JOIN ecn5_accounts..Customer cu with(nolock) ON c.CustomerID = cu.CustomerID
		JOIN ECN5_COMMUNICATOR..Layout l with(nolock) on b.LayoutID = l.LayoutID
	WHERE 
		c.CampaignID = @CampaignID
		AND	CAST(b.sendtime as date) between @StartDate and @EndDate 
		AND statuscode = 'sent' 
		AND b.testblast='N'
		AND c.IsDeleted=0
END
ELSE
BEGIN
INSERT INTO #blasts
	SELECT 
		b.blastID,  
		b.SendTime,  
		b.EmailSubject, 
		GroupName, 
		'' as 'FilterName', 
		c.CampaignName AS BlastCategory,
		ci.FromEmail, 
		ci.CampaignItemName, 
		cu.CustomerName,
		l.LayoutName as MessageName
	FROM 
		ecn5_communicator..blast b WITH (NOLOCK) 
		JOIN ecn5_communicator..CampaignItemBlast cib ON cib.BlastID = b.BlastID
		JOIN ecn5_communicator..CampaignItem ci ON ci.CampaignItemID = cib.CampaignItemID 
		JOIN ecn5_communicator..Campaign c ON c.CampaignID = ci.CampaignID
		LEFT OUTER JOIN ecn5_communicator..groups g ON b.groupID = g.groupID  
		JOIN ecn5_accounts..Customer cu with(nolock) ON c.CustomerID = cu.CustomerID
		JOIN ecn5_communicator..Layout l with(nolock) on b.LayoutID = l.LayoutID
	WHERE 
		c.CampaignID = @CampaignID
		AND	CAST(b.sendtime as date) between @StartDate and @EndDate  
		AND statuscode = 'sent' 
		AND b.testblast='N'
		AND c.IsDeleted=0
		AND b.GroupID = @GroupID
END
CREATE UNIQUE CLUSTERED INDEX idx_tmpblast ON #Blasts (BlastId)

--DROP TABLE #FilterTemp 
CREATE TABLE #FilterTemp (BlastID int, ssFilter varchar(MAX),Filter varchar(MAX))

INSERT INTO #FilterTemp(
	BlastID, 
	ssFilter ,
	Filter
	)
SELECT DISTINCT 
	b.BlastId,
	 ss.SmartSegmentName + ' for blasts(' + STUFF (( SELECT DISTINCT ',' + refBlastIDs FROM ECN5_COMMUNICATOR.dbo.CampaignItemBlastFilter cibfINN WHERE cibfINN.campaignitemblastid = cibf.campaignitemblastid ORDER BY ',' + refBlastIDs FOR XML PATH('') ), 1, 1, '')+')'	 ssFilter
	, STUFF(( SELECT DISTINCT ',' + FilterName  FROM ECN5_COMMUNICATOR.dbo.Filter fINN WHERE fINN.Filterid = f.FilterId ORDER BY ',' + FilterName FOR XML PATH('') ), 1, 1, '')Filter
FROM
	ecn5_communicator..blast b WITH (NOLOCK) 
	JOIN ecn5_communicator..CampaignItemBlast cib ON cib.BlastID = b.BlastID
	JOIN ecn5_communicator..CampaignItem ci ON ci.CampaignItemID = cib.CampaignItemID 
	LEFT OUTER JOIN ECN5_COMMUNICATOR..CampaignItemBlastFilter cibf with(nolock) on cib.CampaignItemBlastID = cibf.CampaignItemBlastID AND cibf.IsDeleted=0
	LEFT OUTER JOIN ECN5_COMMUNICATOR..SmartSegment ss with(nolock) on cibf.SmartSegmentID = ss.SmartSegmentID	AND ss.IsDeleted=0
	LEFT OUTER JOIN ECN5_COMMUNICATOR..Filter f with(nolock) on cibf.FilterID = f.FilterID	AND f.IsDeleted=0
WHERE 
	ci.CampaignID = @CampaignID 
	AND CAST(b.sendtime as date) between @StartDate and @EndDate  
	AND statuscode = 'sent' 
	AND b.testblast='N'
	AND cibf.IsDeleted=0
	AND b.GroupID = case when @GroupID is not null then  @GroupID else b.GroupID END
--GROUP BY 
--	b.BlastID,
--	cibf.SmartSegmentID, 
--	ss.SmartSegmentName,
--	cibf.RefBlastIDs, 
--	cibf.FilterID, 
--	f.FilterName
--	cib.CampaignItemBlastID = 547537

DECLARE @Filters table(BlastID int, FilterName varchar(1000))
INSERT INTO @Filters(BlastID, FilterName)

SELECT 
	BlastID,
	STUFF(( SELECT DISTINCT ',' + 
		CASE WHEN ssFilter IS NOT NULL THEN ssFilter WHEN Filter IS NOT NULL THEN Filter END FROM #FilterTemp fIn WHERE FIn.BlastId = f.BlastId  ORDER BY ',' 
		+ CASE WHEN ssFilter IS NOT NULL THEN ssFilter WHEN Filter IS NOT NULL THEN Filter END 
	FOR XML PATH('') ), 1, 1, '')
FROM
	#FilterTemp f
GROUP BY 
	BlastId

CREATE TABLE #BlastActivity  (
	BlastID INT, 
	tcounts INT, 
	ucounts INT, 
	actiontypecode VARCHAR(50), 
UNIQUE CLUSTERED (BlastID, ActionTypeCode))

---------------------------------------
--Historical Data
---------------------------------------

INSERT INTO #BlastActivity
	SELECT 
		bao.BlastID, 
		TotalOpens,
		UniqueOpens,
		'open'
	FROM 
		ECN5_Warehouse.dbo.BlastOpenCounts bao
		INNER JOIN  #blasts b ON bao.BlastID = b.blastID
	WHERE 
		DATEDIFF(DAY,b.Sendtime,GETDATE()) > 0
	ORDER BY
		bao.BlastId

INSERT INTO #BlastActivity
	SELECT 
		bas.BlastID, 
		TotalSends,
		UniqueSends,
		'send'
	FROM 
		ECN5_Warehouse.dbo.BlastSendCounts bas
		INNER JOIN #blasts b ON bas.BlastID = b.blastID
	WHERE 
		DATEDIFF(DAY,b.Sendtime,GETDATE()) > 0
	ORDER BY
		bas.BlastId

INSERT INTO #BlastActivity
	SELECT	
		bab.BlastID, 
		SUM(TotalBounces),
		SUM(UniqueBounces),
	CASE 
			WHEN BounceCode='hardbounce' or BounceCode='hard' THEN 'hardbounce' 
			WHEN BounceCode='softbounce' THEN 'softbounce' 
			ELSE 'bounce' END
	FROM 
		ECN5_Warehouse.dbo.BlastBounceCounts bab
		JOIN #blasts b ON bab.BlastID = b.blastID
	WHERE 
		DATEDIFF(DAY,b.Sendtime,GETDATE()) > 0
	GROUP BY 
		bab.BlastID, 
		CASE 
			WHEN BounceCode='hardbounce' or BounceCode='hard' THEN 'hardbounce' 
			WHEN BounceCode='softbounce' THEN 'softbounce' 
			ELSE 'bounce' END
	ORDER BY
		bab.BlastId,
		4

INSERT INTO #BlastActivity
	SELECT 
		bau.BlastID, 
		TotalUnsubscribed,
		UniqueUnsubscribed,
		'subscribe' 
	FROM 
		ECN5_Warehouse.dbo.BlastUnsubscribeCounts bau
		JOIN #blasts b ON bau.BlastID = b.blastID
	WHERE 
		UnsubscribeCode IN ('unsubscribe', 'subscribe')
		AND	DATEDIFF(DAY,b.Sendtime,GETDATE()) > 0
	ORDER BY
		bau.BlastId

INSERT INTO #BlastActivity
	SELECT 
		bau.BlastID,
	    TotalUnsubscribed,
		UniqueUnsubscribed,
		bau.UnsubscribeCode
	FROM
		ECN5_Warehouse.dbo.BlastUnsubscribeCounts bau
		JOIN #blasts b ON bau.BlastID = b.blastID
	WHERE
		UnsubscribeCode IN ('MASTSUP_UNSUB', 'FEEDBACK_UNSUB', 'ABUSERPT_UNSUB')
		AND	DATEDIFF(DAY,b.Sendtime,GETDATE()) > 0
	ORDER BY 
		bau.BlastID

INSERT INTO #BlastActivity
	SELECT 
		basu.BlastID, 
		SUM(TotalSuppressed),
		SUM(UniqueSuppressed), 
		'suppressed' 
	FROM 
		ECN5_Warehouse.dbo.BlastSuppressCounts basu
		JOIN #blasts b ON basu.BlastID = b.blastID
	WHERE 
		DATEDIFF(DAY,b.Sendtime,GETDATE()) > 0
	GROUP BY
		basu.BlastId

INSERT INTO #BlastActivity
	SELECT
		bac.BlastId,
		SUM(TotalClicks),
		SUM(UniqueClicks),
		'click'
	FROM
		ECN5_Warehouse.dbo.BlastClickCounts bac
		JOIN #blasts b ON bac.BlastID = b.blastID
	WHERE 
		DATEDIFF(DAY,b.Sendtime,GETDATE()) > 0
	GROUP BY
		bac.BlastId
	ORDER BY 
		bac.BlastId


INSERT INTO #BlastActivity
	SELECT 
		bao.BlastID, 
		ISNULL(MobileOpens,0), 
		ISNULL(MobileUniqueOpens,0), 
		'mobileopens' 
	FROM
		ECN5_Warehouse.dbo.BlastOpenCounts bao
		JOIN #blasts b ON bao.BlastID = b.blastID
	WHERE 
		DATEDIFF(DAY,b.Sendtime,GETDATE()) > 0
	ORDER BY
		bao.BlastId


INSERT INTO #BlastActivity
	SELECT 
		bac.BlastID,
		0 ,
		COUNT(distinct bac.EmailID),
		'clickthrough'
	FROM
		BlastActivityClicks bac with(nolock)		
		JOIN #blasts b on bac.BlastID = b.BlastID
	GROUP BY bac.BlastID

IF @CurrentDate BETWEEN @StartDate AND @EndDate  
BEGIN 
---------------------------------------
--Today's Data
---------------------------------------
INSERT INTO #BlastActivity
	SELECT 
		bao.BlastID, 
		COUNT(OpenId) AS TotalOpens,
		COUNT(DISTINCT(EmailID)) AS UniqueOpens,
		'open'
	FROM 
		ECN_Activity.dbo.BlastActivityOpens bao WITH (NOLOCK) 
		INNER JOIN  #blasts b ON bao.BlastID = b.blastID
	WHERE
		DATEDIFF(DAY,b.Sendtime,GETDATE()) = 0
	GROUP BY
		bao.BlastId
	ORDER BY
		bao.BlastId

INSERT INTO #BlastActivity
	SELECT 
		bas.BlastID, 
		COUNT(SendId) AS TotalSends,
		COUNT(DISTINCT(EmailId)) AS UniqueSends,
		'send'
	FROM 
		ECN_Activity.dbo.BlastActivitySends bas WITH (NOLOCK)
		INNER JOIN #blasts b ON bas.BlastID = b.blastID
	WHERE
		DATEDIFF(DAY,b.Sendtime,GETDATE()) = 0
	GROUP BY
		bas.BlastId
	ORDER BY
		bas.BlastId


INSERT INTO #BlastActivity
	SELECT	
		bab.BlastID, 
		COUNT(BounceID),
		COUNT(Distinct(EmailID)),
	CASE 
			WHEN BounceCode='hardbounce' or BounceCode='hard' THEN 'hardbounce' 
			WHEN BounceCode='softbounce' THEN 'softbounce' 
			ELSE 'bounce' END
	FROM 
		ECN_Activity.dbo.BlastActivityBounces bab WITH (NOLOCK)
		JOIN ECN_Activity.dbo.BounceCodes bc WITH (NOLOCK) ON bab.BounceCodeId = bc.BounceCodeId
		JOIN #blasts b ON bab.BlastID = b.blastID
	WHERE
		DATEDIFF(DAY,b.Sendtime,GETDATE()) = 0
	GROUP BY
		bab.BlastId,
	CASE 
			WHEN BounceCode='hardbounce' or BounceCode='hard' THEN 'hardbounce' 
			WHEN BounceCode='softbounce' THEN 'softbounce' 
			ELSE 'bounce' END
	ORDER BY
		bab.BlastId,
		4

INSERT INTO #BlastActivity
	SELECT 
		bau.BlastID, 
		COUNT(UnsubscribeId) TotalUnsubscribed,
		COUNT(DISTINCT(EMAILId)) UniqueUnsubscribed,
		'subscribe' 
	FROM 
		ECN_Activity.dbo.BlastActivityUnsubscribes bau WITH (NOLOCK)
		JOIN ECN_Activity.dbo.UnsubscribeCodes uc  WITH (NOLOCK) ON bau.UnsubscribeCodeId = uc.UnsubscribeCodeID
		JOIN #blasts b ON bau.BlastID = b.blastID
	WHERE 
		UnsubscribeCode IN ('unsubscribe', 'subscribe')
		AND DATEDIFF(DAY,b.Sendtime,GETDATE()) = 0
	GROUP BY
		bau.BlastId
	ORDER BY
		bau.BlastId

INSERT INTO #BlastActivity
	SELECT 
		bau.BlastID,
		COUNT(UnsubscribeID) TotalUnsubscribed,
		COUNT(DISTINCT(EmailID)) UniqueUnsubscribed,
		case
			WHEN bau.UnsubscribeCodeID = 1 then 'FEEDBACK_UNSUB'
			WHEN bau.UnsubscribeCodeID = 2 then 'MASTSUP_UNSUB'
			WHEN bau.UnsubscribeCodeID = 4 then 'ABUSERPT_UNSUB'
			END
	FROM
		ECN_Activity.dbo.BlastActivityUnSubscribes bau WITH(NOLOCK)
		JOIN ECN_Activity.dbo.UnsubscribeCodes uc WITH(NOLOCK) on bau.UnsubscribeCodeID = uc.UnsubscribeCodeID
		JOIN #blasts b on bau.BlastID = b.BlastID
	WHERE
		bau.UnsubscribeCodeID in (1,2,4)
		AND DATEDIFF(DAY, b.Sendtime, GETDATE()) = 0
	GROUP BY 
		bau.BlastID, bau.UnsubscribeCodeID
	ORDER BY 
		bau.BlastID

INSERT INTO #BlastActivity
	SELECT 
		basu.BlastID, 
		COUNT(SuppressID),
		COUNT(DISTINCT(EMAILID)), 
		'suppressed' 
	FROM 
		ECN_Activity.dbo.BlastActivitySuppressed basu WITH (NOLOCK)
		JOIN #blasts b ON basu.BlastID = b.blastID
	WHERE
		DATEDIFF(DAY,b.Sendtime,GETDATE()) = 0
	GROUP BY
		basu.BlastId;

WITH CTE (BlastId,Url,TotalClicks,UniqueClicks)
AS (
	SELECT 
		bac.BlastID, 
		LEFT(URL,896) as URL,
		COUNT(bac.ClickID) TotalClicks,
		COUNT(DISTINCT bac.EmailID) UniqueClicks		
	FROM
		ECN_Activity.dbo.BlastActivityClicks bac WITH (NOLOCK) 
		INNER JOIN #blasts b on bac.BlastId = b.BlastId
	WHERE
		DATEDIFF(DAY,b.Sendtime,GETDATE()) = 0
	GROUP BY 
		bac.BlastID,
		LEFT(URL,896))

INSERT INTO #BlastActivity
	SELECT
		BlastId,
		SUM(TotalClicks),
		SUM(UniqueClicks),
		'click'
	FROM
		CTE
	GROUP BY
		BlastId
	ORDER BY 
		BlastId


INSERT INTO #BlastActivity
	SELECT 
		bao.BlastID, 
		COUNT(OpenId) AS TotalOpens,
		COUNT(DISTINCT(EmailID)) AS UniqueOpens,
		'mobileopens' 
	FROM 
		ECN_Activity.dbo.BlastActivityOpens bao WITH (NOLOCK) 
		INNER JOIN  #blasts b ON bao.BlastID = b.blastID
	WHERE
		DATEDIFF(DAY,b.Sendtime,GETDATE()) = 0
		AND PlatformId = 2
	GROUP BY
		bao.BlastId
	ORDER BY
		bao.BlastId

END

/**********************/
/* Return Data */
/**********************/

 SELECT  
	b1.blastID, 
	b1.sendtime, 
	b1.emailsubject, 
	groupname, 
--	ISNULL(SUBSTRING(f.FilterName,0,LEN(f.FilterName)),'') as 'FilterName', 
	ISNULL(f.FilterName,'') as 'FilterName', 
	b1.BlastCategory, 
	SUM(CASE WHEN  actiontypecode='send' THEN ucounts ELSE 0 END) AS sendtotal,  
	SUM(CASE WHEN  actiontypecode='send' THEN ucounts ELSE 0 END) -  
		(SUM(CASE WHEN  actiontypecode='softbounce' THEN ucounts ELSE 0 END) + 
		SUM(CASE WHEN  actiontypecode='bounce' THEN ucounts ELSE 0 END) +
		SUM(CASE WHEN  actiontypecode='hardbounce' THEN ucounts ELSE 0 END)) AS  Delivered,  

   SUM(CASE WHEN  actiontypecode='softbounce' THEN ucounts ELSE 0 END) + 
		SUM(CASE WHEN  actiontypecode='bounce' THEN ucounts ELSE 0 END) +
		SUM(CASE WHEN  actiontypecode='hardbounce' THEN ucounts ELSE 0 END) AS bouncetotal,  
   SUM(CASE WHEN  actiontypecode='open' THEN ucounts ELSE 0 END) AS UniqueOpens,  
   SUM(CASE WHEN  actiontypecode='open' THEN tcounts ELSE 0 END) AS TotalOpens,  
   SUM(CASE WHEN  actiontypecode='click' THEN ucounts ELSE 0 END) AS UniqueClicks,  
   SUM(CASE WHEN  actiontypecode='click' THEN tcounts ELSE 0 END) AS TotalClicks,  
   SUM(CASE WHEN actiontypecode = 'clickthrough' THEN ucounts ELSE 0 END) as ClickThrough,
   SUM(CASE WHEN  actiontypecode='subscribe' THEN ucounts ELSE 0 END) AS UnsubscribeTotal,
   SUM(CASE WHEN  actiontypecode='suppressed' THEN ucounts ELSE 0 END) AS suppressedtotal ,
   SUM(CASE WHEN actiontypecode='mobileopens' THEN ucounts ELSE 0 END) AS MobileOpens,
   SUM(CASE WHEN actiontypecode='ABUSERPT_UNSUB' THEN ucounts ELSE 0 END) AS TotalAbuseComplaints,
   SUM(CASE WHEN actiontypecode='FEEDBACK_UNSUB' THEN ucounts ELSE 0 END) AS TotalISPFeedbackLoops,
   SUM(CASE WHEN actiontypecode='MASTSUP_UNSUB' THEN ucounts ELSE 0 END) AS MasterSuppressed,
   b1.FromEmail,
   b1.CampaignItemName,
   b1.CustomerName,
   b1.MessageName
FROM  
	#BlastActivity ba 
	JOIN #blasts b1 ON ba.blastID = b1.blastID 
	LEFT OUTER JOIN @Filters f on ba.BlastID = f.BlastID
GROUP BY 
	b1.BlastID,
	b1.CustomerName, 
	b1.CampaignItemName, 
	b1.sendtime, 
	b1.emailsubject, 
	groupname, 
	f.filtername, 
	b1.BlastCategory,
	b1.FromEmail,
	b1.MessageName
ORDER BY 
	b1.sendtime ASC
 
DROP TABLE #blasts
DROP TABLE #blastActivity
DROP TABLE #FilterTemp
