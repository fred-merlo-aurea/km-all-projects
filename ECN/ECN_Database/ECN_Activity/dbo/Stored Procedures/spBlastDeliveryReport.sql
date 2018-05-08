


--2014-08-05 MK Added Code to Include current day's data if selected.
--2014-12-02 MK Added Code to prevent duplicate entries which cause errors.  
--	This happened if a blast was sent between 12 AM and 1 AM (when the aggregation runs) and the report was run on the same day the blast was sent.

CREATE PROCEDURE  [dbo].[spBlastDeliveryReport]
(  
  @customerID varchar(5000),  
  @startdate date,  
  @enddate date,
  @Unique bit = 1

)  
AS

SET NOCOUNT ON
  
BEGIN
  
/*DECLARE
  @customerID INT			= 1797, 
  @startdate VARCHAR(20)	= '2014-01-01',
  @enddate VARCHAR(20) 		= '2014-07-10',
  @Unique BIT				= 1
*/

DECLARE @CurrentDate DATETIME = GETDATE()
 
SELECT 
	b.blastID,  
	b.SendTime,  
	b.EmailSubject, 
	g.GroupName, 
	'' as FilterName, 
	c.CampaignName AS CampaignName,
	ci.FromEmail, 
	ci.CampaignItemName, 
	cu.CustomerName 
INTO #blasts
FROM 
	ecn5_communicator..blast b WITH (NOLOCK) 
	JOIN ecn5_communicator..CampaignItemBlast cib ON cib.BlastID = b.BlastID
	JOIN ecn5_communicator..CampaignItem ci ON ci.CampaignItemID = cib.CampaignItemID 
	JOIN ecn5_communicator..Campaign c ON c.CampaignID = ci.CampaignID
	LEFT OUTER JOIN ecn5_communicator..groups g ON b.groupID = g.groupID  
	JOIN ecn5_accounts..Customer cu with(nolock) ON c.CustomerID = cu.CustomerID
	join (select items as customerID from dbo.fn_Split(@customerID, ',')) tmp on tmp.customerID = cu.customerID 
WHERE 
	CAST(b.sendtime as date) between @startdate and @enddate 
	AND statuscode = 'sent' 
	AND b.testblast='N'
	AND c.IsDeleted=0

CREATE UNIQUE CLUSTERED INDEX idx_tmpblast ON #Blasts (BlastId)

CREATE TABLE #BlastActivity  (
	BlastID INT, 
	tcounts INT, 
	ucounts INT, 
	actiontypecode VARCHAR(50), 
UNIQUE CLUSTERED (BlastID, ActionTypeCode))

DECLARE @Filters table(BlastID int, FilterName varchar(1000))
INSERT INTO @Filters(BlastID, FilterName)
SELECT b.BLastID, STUFF(Case WHEN cibf.SmartSegmentID is not null THEN ss.SmartSegmentName + ' for blasts(' + cibf.refBlastIDs + '), '
									WHEN cibf.FilterID is not null THEN f.FilterName + ', ' END, 1,0,'')
FROM ecn5_communicator..blast b WITH (NOLOCK) 
	JOIN ecn5_communicator..CampaignItemBlast cib ON cib.BlastID = b.BlastID
	JOIN ecn5_communicator..CampaignItem ci ON ci.CampaignItemID = cib.CampaignItemID 
	JOIN ecn5_communicator..Campaign c ON c.CampaignID = ci.CampaignID
	left outer join ECN5_COMMUNICATOR..CampaignItemBlastFilter cibf with(nolock) on cib.CampaignItemBlastID = cibf.CampaignItemBlastID and cibf.IsDeleted = 0
	left outer join ECN5_COMMUNICATOR..SmartSegment ss with(nolock) on cibf.SmartSegmentID = ss.SmartSegmentID
	left outer join ECN5_COMMUNICATOR..Filter f with(nolock) on cibf.FilterID = f.FilterID
	join (select items as customerID from dbo.fn_Split(@customerID, ',')) tmp on tmp.customerID = b.customerID 
WHERE 
	CAST(b.sendtime as date) between @startdate and @enddate 
	AND statuscode = 'sent' 
	AND b.testblast='N'
	AND c.IsDeleted=0
Group by b.BlastID,cibf.SmartSegmentID, ss.SmartSegmentName,cibf.RefBlastIDs, cibf.FilterID, f.FilterName

---------------------------------------
--Historical Data
---------------------------------------
INSERT INTO #BlastActivity
SELECT BUC.[BlastID], SUM(BUC.UniqueUnsubscribed), '', 'AbuseHistory'      
FROM [ECN5_Warehouse].[dbo].[BlastUnsubscribeCounts] BUC
JOIN [ECN5_COMMUNICATOR].[dbo].[Blast] b (nolock)on b.BlastID = BUC.BlastID
WHERE BUC.UnsubscribeCode = 'ABUSERPT_UNSUB' AND DATEDIFF(DAY,b.Sendtime,GETDATE()) > 0
GROUP BY BUC.BlastID

INSERT INTO #BlastActivity
SELECT BUC.[BlastID], SUM(BUC.UniqueUnsubscribed), '', 'FeedbackHistory'
FROM [ECN5_Warehouse].[dbo].[BlastUnsubscribeCounts] BUC
JOIN [ECN5_COMMUNICATOR].[dbo].[Blast] b (nolock)on b.BlastID = BUC.BlastID
WHERE BUC.UnsubscribeCode = 'FEEDBACK_UNSUB' AND DATEDIFF(DAY,b.Sendtime,GETDATE()) > 0
GROUP BY BUC.BlastID

--INSERT INTO #BlastActivity
--SELECT BBC.[BlastID], SUM(BBC.UniqueBounces), '', 'SpamCountHistory'
--FROM [ECN5_Warehouse].[dbo].[BlastBounceCounts] BBC (nolock)
--JOIN [ECN5_COMMUNICATOR].[dbo].[Blast] b (nolock)on b.BlastID = BBC.BlastID
--WHERE [BounceCode] in ('SpamNofication','spamnotification') AND DATEDIFF(DAY,b.Sendtime,GETDATE()) > 0
--GROUP BY BBC.[BlastID]
---

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
		0,
		COUNT(distinct EmailID),
		'clickthrough'
	FROM
		BlastActivityClicks bac with(nolock)
		join #blasts b with(nolock) on bac.BlastID = b.BlastID
	GROUP by bac.BlastID
	ORDER by bac.BlastID

IF cast (@CurrentDate as date) BETWEEN @startdate AND @enddate  
BEGIN 
---------------------------------------
--Today's Data
---------------------------------------
INSERT INTO #BlastActivity
SELECT b.[BlastID], ISNULL(COUNT(UnSub.EmailID),0), '', 'Abuse'      
FROM [ecn_activity].[dbo].[BlastActivityUnSubscribes] UnSub (nolock) 
JOIN [ecn_activity].[dbo].UnsubscribeCodes Codes (nolock) on UnSub.UnsubscribeCodeID = Codes.UnsubscribeCodeID
JOIN [ECN5_COMMUNICATOR].[dbo].[Blast] b (nolock)on b.BlastID = b.BlastID
WHERE Codes.UnsubscribeCodeID = 4 and UnSub.[BlastID] = b.BlastID AND DATEDIFF(DAY,b.Sendtime,GETDATE()) = 0
GROUP BY b.BlastID

INSERT INTO #BlastActivity
SELECT b.[BlastID], ISNULL(COUNT(UnSub.EmailID),0), '', 'Feedback'      
FROM [ecn_activity].[dbo].[BlastActivityUnSubscribes] UnSub (nolock) 
JOIN [ecn_activity].[dbo].UnsubscribeCodes Codes (nolock) on UnSub.UnsubscribeCodeID = Codes.UnsubscribeCodeID
JOIN [ECN5_COMMUNICATOR].[dbo].[Blast] b (nolock)on b.BlastID = UnSub.BlastID
WHERE Codes.UnsubscribeCodeID = 1 and UnSub.[BlastID] = b.BlastID AND DATEDIFF(DAY,b.Sendtime,GETDATE()) = 0
GROUP BY b.BlastID

--INSERT INTO #BlastActivity
--SELECT BAB.[BlastID], COUNT(BAB.BlastID), '', 'SpamCount'
--FROM [ecn_activity].[dbo].[BlastActivityBounces] BAB (nolock)
--JOIN [ECN5_COMMUNICATOR].[dbo].[Blast] b (nolock)on b.BlastID = BAB.BlastID
--WHERE BounceCodeID in (10,11) AND DATEDIFF(DAY,b.Sendtime,GETDATE()) = 0
--GROUP BY BAB.[BlastID]
---
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
	ISNULL(SUBSTRING(f.FilterName,0,LEN(f.FilterName)),'') as 'FilterName', 
	b1.CampaignName, 
	SUM(CASE WHEN  actiontypecode='send' THEN ucounts ELSE 0 END) AS sendtotal,  
	SUM(CASE WHEN  actiontypecode='send' THEN ucounts ELSE 0 END) -  
		(SUM(CASE WHEN  actiontypecode='softbounce' THEN ucounts ELSE 0 END) + 
		SUM(CASE WHEN  actiontypecode='bounce' THEN ucounts ELSE 0 END) +
		SUM(CASE WHEN  actiontypecode='hardbounce' THEN ucounts ELSE 0 END)) AS  Delivered,  

   SUM(CASE WHEN  actiontypecode='softbounce' THEN ucounts ELSE 0 END) AS softbouncetotal,  
   SUM(CASE WHEN  actiontypecode='hardbounce' THEN ucounts ELSE 0 END) AS hardbouncetotal,  
   0 AS OtherBouncetotal,  
   SUM(CASE WHEN  actiontypecode='softbounce' THEN ucounts ELSE 0 END) + 
		SUM(CASE WHEN  actiontypecode='bounce' THEN ucounts ELSE 0 END) +
		SUM(CASE WHEN  actiontypecode='hardbounce' THEN ucounts ELSE 0 END) AS bouncetotal,  
   SUM(CASE WHEN  actiontypecode='open' THEN ucounts ELSE 0 END) AS UniqueOpens,  
   SUM(CASE WHEN  actiontypecode='open' THEN tcounts ELSE 0 END) AS TotalOpens,  
   SUM(CASE WHEN  actiontypecode='click' THEN ucounts ELSE 0 END) AS UniqueClicks,  
   SUM(CASE WHEN  actiontypecode='click' THEN tcounts ELSE 0 END) AS TotalClicks,  
   SUM(CASE WHEN actiontypecode = 'clickthrough' THEN ucounts ELSE 0 END) AS ClickThrough,
   SUM(CASE WHEN  actiontypecode='subscribe' THEN ucounts ELSE 0 END) AS UnsubscribeTotal,
   SUM(CASE WHEN  actiontypecode='suppressed' THEN ucounts ELSE 0 END) AS suppressedtotal ,
   SUM(CASE WHEN actiontypecode='mobileopens' THEN ucounts ELSE 0 END) AS MobileOpens,
   b1.FromEmail,
   b1.CampaignItemName,
   b1.CustomerName,    
	bf.Field1,
	bf.Field2,
	bf.Field3,
	bf.Field4,
	bf.Field5,
	SUM(CASE WHEN actiontypecode='Abuse' OR actiontypecode='AbuseHistory' THEN tcounts ELSE 0 END) AS Abuse,
	SUM(CASE WHEN actiontypecode='Feedback' OR actiontypecode='FeedbackHistory' THEN tcounts ELSE 0 END) AS Feedback,
	SUM(CASE WHEN actiontypecode='SpamCount' OR actiontypecode='SpamCountHistory' THEN tcounts ELSE 0 END) AS SpamCount	
FROM  
	#BlastActivity ba 
	JOIN #blasts b1 ON ba.blastID = b1.blastID
	left outer JOIN [ecn5_communicator].[dbo].[BlastFields] bf (nolock)ON bf.BlastID = b1.BlastID
	LEFT OUTER JOIN @Filters f on ba.BlastID = f.BlastID
GROUP BY 
	b1.BlastID,
	b1.CustomerName, 
	b1.CampaignItemName, 
	b1.sendtime, 
	b1.emailsubject, 
	groupname, 
	ISNULL(SUBSTRING(f.FilterName,0,LEN(f.FilterName)),''),
	b1.CampaignName,
	b1.FromEmail, 
	bf.Field1,
	bf.Field2,
	bf.Field3,
	bf.Field4,
	bf.Field5

ORDER BY 
	-- b1.sendtime ASC
	b1.CustomerName ASC
	, b1.CampaignItemName ASC
 
DROP TABLE #blasts
DROP TABLE #blastActivity

END

GO


