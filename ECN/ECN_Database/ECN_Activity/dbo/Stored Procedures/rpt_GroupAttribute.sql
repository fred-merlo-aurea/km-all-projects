CREATE PROCEDURE rpt_GroupAttribute
	@GroupIds VARCHAR(500),
	@CustomerId INT ,
	@StartDate DATE,
	@EndDate DATE 

AS

SET NOCOUNT ON
SET DATEFIRST 7

DECLARE @BasechannelId int 

--SET @EndDate = DATEADD(DAY, 1,@EndDate)
SET @BasechannelId = (SELECT DISTINCT BaseChannelId FROM ECN5_ACCOUNTS.DBO.Customer WHERE  CustomerId = @CustomerId)

DECLARE @Groups Table (GroupId INT)

INSERT INTO @Groups SELECT * from fn_split(@Groupids,',')

CREATE TABLE #Blasts 
(
	GroupId INT,
	BlastID INT, 
	CampaignItemId INT,
	EmailSubject varchar(1000), 
	SendTime datetime,  
	Year INT,
	DayOfWeek VARCHAR(10),
	CampaignName varchar(250), 
	FilterName varchar(250), 
	BlastField1 varchar(250), 
	BlastField2 varchar(250), 
	BlastField3 varchar(250), 
	BlastField4 varchar(250), 
	BlastField5 varchar(250),
	TemplateName varchar(250),
	ABAmount INT,
	ABIsAmount varchar(20),
	MessageName varchar(250),
	CampaignItemName varchar(250),
	SuppressionGroups varchar(1000),
	GroupName varchar(250),
	EmailFrom varchar(100)
UNIQUE CLUSTERED (BlastID), Filter varchar(500))
	
---------------------
-- GET Blast Data
---------------------
INSERT INTO #Blasts (
	GroupId,
	BlastId,
	CampaignItemId,
	EmailSubject,
	SendTime,
	Year,
	DayOfWeek,
	CampaignName,
	FilterName,
	BlastField1,
	BlastField2,
	BlastField3,
	BlastField4,
	BlastField5,
	TemplateName,
	ABAmount,
	ABIsAmount,
	MessageName,
	CampaignItemName,
	GroupName,
	EmailFrom)
SELECT  
	b.GroupId,
	b.BlastID, 
	ci.CampaignItemId,
	b.EmailSubject,
	b.sendtime, 
	YEAR(b.SendTime),
	CASE DATEPART(DW,b.SendTime) 
		WHEN 1 THEN 'Sunday' 
		WHEN 2 THEN 'Monday' 
		WHEN 3 THEN 'Tuesday' 
		WHEN 4 THEN 'Wednesday' 
		WHEN 5 THEN 'Thursday' 
		WHEN 6 THEN 'Friday' 
		WHEN 7 THEN 'Saturday'END ,
	c.CampaignName, 
	'' as FilterName,
	bf.Field1,
	bf.Field2,
	bf.Field3,
	bf.Field4,
	bf.Field5,
	cit.TemplateName AS TemplateName,
	b.OverrideAmount,
	CASE 
		WHEN b.OverrideIsAmount =0 AND b.OverrideAmount = 100 THEN 'All'
		WHEN b.OverrideIsAmount = 1 THEN 'Number' 
		WHEN b.OverrideIsAmount = 0 AND b.OverrideAmount != 100 THEN 'Percent' 
		ELSE 'N/A' END  AS IsAmount,
	l.Layoutname AS MessageName,
	ci.CampaignItemName as CampaignItemName,
	gr.groupName as GroupName,
	b.EmailFrom as EmailFrom
FROM	
	ECN5_Communicator.dbo.[BLAST] b 
	INNER JOIN @Groups g on b.GroupId = g.GroupId
	JOIN ECN5_Communicator..Groups gr with(nolock) on g.GroupId = gr.groupid
	INNER JOIN ECN5_Communicator.dbo.CampaignItemBlast cib on b.BlastID = cib.BlastId 
	INNER JOIN ECN5_Communicator.dbo.CampaignItem ci on cib.CampaignItemId = ci.CampaignItemId 
	INNER JOIN ECN5_Communicator..Campaign c on c.CampaignID = ci.CampaignID
	LEFT OUTER JOIN ECN5_Communicator.dbo.BlastFields bf on b.BlastId = BF.BlastId
	LEFT OUTER JOIN ECN5_Communicator.dbo.CampaignItemTemplates cit on ci.CampaignItemTemplateId = cit.CampaignItemTemplateId
	--LEFT OUTER JOIN ECN5_Communicator.dbo.SampleBlasts sb on b.Blastid = sb.BlastId
	LEFT OUTER JOIN ECN5_Communicator.dbo.Layout l ON b.LayoutId = l.LayoutId
WHERE	
	statuscode = 'sent' AND 
	b.testblast='N' AND 
cast(b.sendtime as date) BETWEEN @startdate AND @enddate;

--For getting filters
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
	JOIN #Blasts bl on b.blastid = bl.BlastID
	JOIN ecn5_communicator..CampaignItemBlast cib ON cib.BlastID = b.BlastID	
	LEFT OUTER JOIN ECN5_COMMUNICATOR..CampaignItemBlastFilter cibf with(nolock) on cib.CampaignItemBlastID = cibf.CampaignItemBlastID AND cibf.IsDeleted=0
	LEFT OUTER JOIN ECN5_COMMUNICATOR..SmartSegment ss with(nolock) on cibf.SmartSegmentID = ss.SmartSegmentID	AND ss.IsDeleted=0
	LEFT OUTER JOIN ECN5_COMMUNICATOR..Filter f with(nolock) on cibf.FilterID = f.FilterID	AND f.IsDeleted=0
GROUP BY b.BlastID, cibf.CampaignItemBlastID, ss.SmartSegmentName, f.FilterID


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
---------------------
-- GET Activity Data
---------------------
--DROP TABLE #Activity
CREATE TABLE #Activity(
	BlastID int, 
	actiontype varchar(20), 
	UniqueCount int, 
	TotalCount int, 
UNIQUE CLUSTERED (BlastID, actiontype))


INSERT INTO #Activity
SELECT 
	b.BlastID, 
	'send' AS ActionTypeCode, 
	ISNULL(COUNT( DISTINCT EmailID),0) AS UniqueCount, 
	ISNULL(COUNT(SendID),0) AS TotalCount
FROM 
	ECN_ACTIVITY.DBO.BlastActivitySends bas WITH (NOLOCK) 
	JOIN #Blasts b on b.blastID = bas.blastID 
GROUP BY
	b.BlastID 

UNION

SELECT 
	b.BlastID, 
	'bounce' AS ActionTypeCode, 
	ISNULL(COUNT( DISTINCT EmailID),0) AS UniqueCount, 
	ISNULL(COUNT(BounceID),0) AS TotalCount
FROM 
	ECN_ACTIVITY.DBO.BlastActivityBounces bab WITH (NOLOCK) 
	JOIN #Blasts b on b.blastID = bab.blastID
GROUP BY 
	b.BlastID   

UNION

SELECT 
	b.BlastID, 
	'open' AS ActionTypeCode, 
	ISNULL(COUNT( DISTINCT EmailID),0) AS UniqueCount, 
	ISNULL(COUNT(OpenID),0) AS TotalCount
FROM 
	ECN_ACTIVITY.DBO.BlastActivityOpens bao WITH (NOLOCK) 
	JOIN #Blasts b on b.blastID = bao.blastID
GROUP BY 
	b.BlastID

UNION

SELECT 
	b.BlastID, 
	'refer' AS ActionTypeCode, 
	ISNULL(COUNT( DISTINCT EmailID),0) AS UniqueCount, 
	ISNULL(COUNT(ReferID),0) AS TotalCount
FROM 
	ECN_ACTIVITY.DBO.BlastActivityRefer bar WITH (NOLOCK) 
	JOIN #Blasts b on b.blastID = bar.blastID
GROUP BY
	b.BlastID

UNION

SELECT 
	b.BlastID, 
	'suppression' AS ActionTypeCode, 
	ISNULL(COUNT( DISTINCT EmailID),0) AS UniqueCount, 
	ISNULL(COUNT(SuppressID),0) AS TotalCount
FROM 
	ECN_ACTIVITY.DBO.BlastActivitySuppressed bas WITH (NOLOCK) 
	JOIN #Blasts b on b.blastID = bas.blastID 
GROUP BY 
	b.BlastID 

UNION

SELECT  
		inn.BlastID,
		'click' AS ActionTypeCode,
		ISNULL(SUM(DistinctCount),0) AS UniqueCount, 
		ISNULL(SUM(total),0) AS TotalCount       
FROM (        
	SELECT
		b.BlastID, 
		COUNT(distinct URL) AS DistinctCount, 
		COUNT(ClickID) AS total         
	FROM  
		ECN_ACTIVITY.DBO.BlastActivityClicks bac WITH (NOLOCK) 
		JOIN #Blasts b on b.blastID = bac.blastID
	GROUP BY
		b.BlastID, 
		URL, 
		EmailID        
	) AS inn   
GROUP BY 
	BlastID

UNION
SELECT b.BlastID,
		'ClickThrough' AS ActionTypeCode,
		ISNULL(COUNT(distinct EmailID),0) as UniqueCount,
		0
FROM
	ECN_Activity..BlastActivityClicks bac with(nolock)
	join #Blasts b on bac.BlastID = b.BlastID
GROUP by b.BlastID
UNION 

SELECT 
b.BlastID, 
	'resend' AS ActionTypeCode, 
	ISNULL(COUNT( DISTINCT EmailID),0) AS UniqueCount, 
	ISNULL(COUNT(ResendID),0) AS TotalCount
FROM 
	ECN_ACTIVITY.DBO.BlastActivityResends bas WITH (NOLOCK) 
	JOIN #Blasts b on b.blastID = bas.blastID 
GROUP BY 
	b.BlastID 
------------------------------------------------------
--GET Blast Unsubscribe Details 
------------------------------------------------------

UNION 

SELECT 
	b.BlastID, 
	UnsubscribeCode AS ActionTypeCode,
	ISNULL(count(distinct bau.EmailID),0) AS unique_total, 
	ISNULL(count(bau.UnsubscribeID),0) AS Total
FROM
	ecn_Activity..UnsubscribeCodes uc 
	LEFT JOIN ecn_Activity..BlastActivityUnSubscribes bau ON uc.UnsubscribeCodeID = bau.UnsubscribeCodeID  
	JOIN #Blasts b on b.blastid = bau.BlastId
WHERE 
	UnsubscribeCode IN ('ABUSERPT_UNSUB','FEEDBACK_UNSUB','MASTSUP_UNSUB','subscribe')
GROUP BY
	b.BlastId,
	UnsubscribeCode

UNION

SELECT	
	b.BlastID, 
	ActionTypeCode = CASE WHEN BounceCode != 'softbounce' and bc.BounceCode != 'hardbounce' THEN 'otherbounce' ELSE BounceCode END,
	ISNULL(count(distinct bab.EmailID),0) AS unique_total, 
	ISNULL(count(bab.BounceID),0) AS Total
FROM 
	ECN_Activity..BounceCodes bc
	LEFT JOIN ecn_Activity..BlastActivityBounces bab ON bab.BounceCodeID = bc.BounceCodeID 
	JOIN #Blasts b on b.blastid = bab.BlastId
GROUP BY
	b.BlastId,
	CASE WHEN BounceCode != 'softbounce' and bc.BounceCode != 'hardbounce' THEN 'otherbounce' ELSE BounceCode END

-------------------------------------------------------------
--OMNITURE DATA
-------------------------------------------------------------
--------------------------------------
--GET Override and Delimiter Values
--------------------------------------
DECLARE @AllowOverride VARCHAR(100)
DECLARE @Override VARCHAR(100)
DECLARE @Delimiter CHAR(1)

SELECT 
	@AllowOverride = CAST(XMLConfig AS XML).value('data(/Settings/AllowCustomerOverride)[1]','VARCHAR(10)') 
FROM 
	ECN5_Communicator.dbo.LinkTrackingSettings 
WHERE 
	BasechannelId = @BasechannelId

SELECT 
	@Override = CAST(XMLConfig AS XML).value('data(/Settings/Override)[1]','VARCHAR(10)') 
FROM 
	ECN5_Communicator.dbo.LinkTrackingSettings 
WHERE 
	CustomerID = @CustomerId

SELECT 
	@Delimiter= CAST(XMLConfig AS XML).value('data(/Settings/Delimiter)[1]','VARCHAR(10)') 
FROM 
	ECN5_COMMUNICATOR.dbo.LinkTrackingSettings 
WHERE 
	CASE WHEN @AllowOverride = 'TRUE' AND @OverRide = 'TRUE' THEN Customerid END = @CustomerId
	OR CASE WHEN @AllowOverride = 'FALSE' OR @OverRide ='FALSE' THEN BasechannelId END = @BasechannelId

--SELECT @AllowOverride,@override,@Delimiter

--------------------------------------
--GET list of Values to Concatenate
--------------------------------------
DECLARE @OmnitureValues VARCHAR(MAX)
DECLARE @t_OmnitureValues Table (BlastId INT, ValueString VARCHAR(MAX))

SET @BasechannelId = (SELECT DISTINCT BaseChannelId FROM ECN5_ACCOUNTS.DBO.Customer WHERE  CustomerId = @CustomerId)

INSERT INTO @t_OmnitureValues
SELECT 
	b.BlastId,
	CASE WHEN ISNULL(CustomValue,'') != '' THEN CustomValue ELSE Value END 
FROM 
	ECN5_Communicator.dbo.LinkTracking lt  
	INNER JOIN ECN5_Communicator.dbo.LinkTrackingParam ltp ON lt.LTID = ltp.LTID
	INNER JOIN ECN5_Communicator.dbo.LinkTrackingParamOption ltpo ON ltp.LTPID = ltpo.LTPID
	INNER JOIN ECN5_Communicator.dbo.CampaignItemLinkTracking cit ON cit.LTPOID = ltpo.LTPOID
	INNER JOIN ECN5_Communicator.dbo.CampaignItemBlast cib ON cit.CampaignItemId = cib.CampaignItemId 
	INNER JOIN ECN5_Communicator.dbo.Blast b ON b.BlastID = cib.BlastId 
	INNER JOIN @Groups G ON b.Groupid = g.GroupId
WHERE 
	lt.DisplayName = 'Omniture'
	AND b.CustomerId = @CustomerId
ORDER BY
	b.BlastId,
	ltp.LTPID

--------------------------------------
--GET list of Domains to Concatenate
--------------------------------------
DECLARE @Domains VARCHAR(1000)
DECLARE @LTID INT
SET @LTID = (SELECT LTID FROM ECN5_COMMUNICATOR.dbo.LinkTracking lt WHERE DisplayName = 'Omniture')

SELECT @Domains = STUFF((
SELECT 
	','+ Domain
FROM
	ECN5_COMMUNICATOR.dbo.LinkTrackingDomain 
WHERE 
	LTID = @LTID 
	AND Customerid = @CustomerId
	AND IsDeleted = 0
FOR XML PATH('')) ,1,1,''
	) 

-------------------------------------------------
--GET list of Suppression Groups to Concatenate
-------------------------------------------------
DECLARE @t_SuppressionGroups Table (CampaignItemId INT, GroupName VARCHAR(MAX))

INSERT INTO 
	@t_SuppressionGroups
SELECT 
	cis.CampaignItemId, 
	g.GroupName 
FROM 
	ECN5_Communicator.dbo.CampaignItemSuppression  cis
	INNER JOIN ECN5_Communicator.dbo.Groups g on cis.groupid = g.groupid
	INNER JOIN #Blasts b on b.CampaignItemId = cis.CampaignItemId

-------------------------------------------------------------


SELECT	
	ISNULL(@Domains,'ALL') AS OmnitureDomains,
	ISNULL(STUFF((
		SELECT 	@Delimiter+ ValueString
		FROM	@t_OmnitureValues ov1
		WHERE  ov1.BlastId = ov2.BlastId
		FOR XML PATH('')) ,1,1,''),'N/A') AS OmnitureValues,
	ISNULL(STUFF((
		SELECT  '/' + GroupName
		FROM	@t_SuppressionGroups sg1
		WHERE sg1.CampaignItemId =sg2.CampaignItemId
		FOR XML PATH('')) ,1,1,''),'N/A') AS SuppressionGroups,
	b.GroupId,
	b.BlastId,
	REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(EmailSubject, CHAR(133),'...'), CHAR(146), ''''),CHAR(145), ''''), NCHAR(8212),'-'),NCHAR(8211),'-') as EmailSubject,
	SendTime,
	Year,
	DayOfWeek,
	ISNULL(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(CampaignName, CHAR(133),'...'),CHAR(146),''''),CHAR(145), ''''), NCHAR(8212),'-'), NCHAR(8211), '-'),'N/A') AS CampaignName,
	ISNULL(SUBSTRING(f.FilterName,0,LEN(f.FilterName)),'N/A') AS FilterName,
	ISNULL(BlastField1,'N/A') AS BlastField1,
	ISNULL(BlastField2,'N/A') AS BlastField2,
	ISNULL(BlastField3,'N/A') AS BlastField3,
	ISNULL(BlastField4,'N/A') AS BlastField4,
	ISNULL(BlastField5,'N/A') AS BlastField5,
	ISNULL(TemplateName,'N/A') AS TemplateName,
	ISNULL(MessageName,'N/A') AS MessageName,
	ISNULL(ABAmount,0) as ABAmount,
	ABIsAmount,
	CampaignItemName,
	b.GroupName,
	EmailFrom,

	ISNULL(MAX(CASE WHEN Actiontype='send' THEN uniquecount end),0) AS usend,
	ISNULL(MAX(CASE WHEN Actiontype='send' THEN TotalCount end),0) AS tsend,
	ISNULL(MAX(CASE WHEN Actiontype='open' THEN uniquecount end),0) AS uopen,
	ISNULL(MAX(CASE WHEN Actiontype='open' THEN TotalCount end),0) AS topen,
	ISNULL(MAX(CASE WHEN Actiontype='click' THEN uniquecount end),0) AS uClick,
	ISNULL(MAX(CASE WHEN Actiontype='click' THEN TotalCount end),0) AS tClick,
	ISNULL(MAX(CASE WHEN Actiontype='subscribe' THEN uniquecount end),0) AS uUnsubscribe,
	ISNULL(MAX(CASE WHEN Actiontype='subscribe' THEN TotalCount end),0) AS tUnsubscribe,
	ISNULL(MAX(CASE WHEN Actiontype='refer' THEN uniquecount end),0) AS uRefer,
	ISNULL(MAX(CASE WHEN Actiontype='refer' THEN TotalCount end),0) AS tRefer,
	ISNULL(MAX(CASE WHEN Actiontype='suppression' THEN TotalCount end),0) AS Suppressed,
	ISNULL(MAX(CASE WHEN ActionType = 'clickthrough' THEN UniqueCount end),0) as ClickThrough,
	ISNULL(MAX(CASE WHEN Actiontype='bounce' THEN uniquecount end),0) AS ubounce,
	ISNULL(MAX(CASE WHEN Actiontype='bounce' THEN TotalCount end),0) AS tbounce,
	ISNULL(MAX(CASE WHEN Actiontype='FeedBack_Unsub' THEN uniquecount end),0) AS uFeedBack_Unsub,
	ISNULL(MAX(CASE WHEN Actiontype='FeedBack_Unsub' THEN TotalCount end),0) AS tFeedBack_Unsub,
	ISNULL(MAX(CASE WHEN Actiontype='HardBounce' THEN uniquecount end),0) AS uHardBounce,
	ISNULL(MAX(CASE WHEN Actiontype='HardBounce' THEN TotalCount end),0) AS tHardBounce,
	ISNULL(MAX(CASE WHEN Actiontype='MastSup_Unsub' THEN uniquecount end),0) AS uMastSup_Unsub,
	ISNULL(MAX(CASE WHEN Actiontype='MastSup_Unsub' THEN TotalCount end),0) AS tMastSup_Unsub,
	ISNULL(MAX(CASE WHEN Actiontype='OtherBounce' THEN uniquecount end),0) AS uOtherBounce,
	ISNULL(MAX(CASE WHEN Actiontype='OtherBounce' THEN TotalCount end),0) AS tOtherBounce,
	ISNULL(MAX(CASE WHEN Actiontype='SoftBounce' THEN uniquecount end),0) AS uSoftBounce,
	ISNULL(MAX(CASE WHEN Actiontype='SoftBounce' THEN TotalCount end),0) AS tSoftBounce,

	ISNULL(MAX(CASE WHEN Actiontype='AbuseRpt_Unsub' THEN uniquecount end),0) AS uAbuseRpt_Unsub,
	ISNULL(MAX(CASE WHEN Actiontype='AbuseRpt_Unsub' THEN TotalCount end),0) AS tAbuseRpt_Unsub,
	ISNULL(MAX(CASE WHEN Actiontype='Subscribe' THEN uniquecount end),0) AS uSubscribe,
	ISNULL(MAX(CASE WHEN Actiontype='Subscribe' THEN TotalCount end),0) AS tSubscribe,
	ISNULL(MAX(CASE WHEN Actiontype = 'resend' THEN uniquecount end), 0) AS uresend,
	ISNULL(MAX(CASE WHEN Actiontype = 'resend' THEN Totalcount end), 0) AS tresend

FROM
	#Blasts b
	LEFT JOIN #Activity a ON b.blastID = a.BlastID
	LEFT JOIN @t_OmnitureValues ov2 ON b.blastID = ov2.BlastID
	LEFT JOIN @t_SuppressionGroups sg2 ON b.CampaignItemId = sg2.CampaignItemId
	LEFT JOIN @Filters f on b.blastid = f.BlastID
GROUP BY 
	b.GroupId,
	b.BlastId,
	ov2.BlastId,
	EmailSubject,
	SendTime,
	Year,
	DayOfWeek,
	CampaignName,
	f.FilterName,
	BlastField1,
	BlastField2,
	BlastField3,
	BlastField4,
	BlastField5,
	TemplateName,
	MessageName,
	ABAmount,
	ABIsAmount,
	sg2.CampaignItemId,
	CampaignItemName,
	b.groupName,
	EmailFrom
ORDER BY 
	Sendtime ASC;

DROP TABLE #Blasts 
DROP TABLE #Activity
DROP TABLE #FilterTemp