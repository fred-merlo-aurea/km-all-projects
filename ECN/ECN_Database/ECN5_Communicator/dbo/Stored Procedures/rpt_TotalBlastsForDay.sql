/************************
Created: 2014-03-24 
Upated:

Notes: opens and clicks are only for the day specified, not the total number of opens and clicks for that blast.


************************/


CREATE procedure [dbo].[rpt_TotalBlastsForDay]

	@Date Datetime 	
	
AS

SET NOCOUNT ON

/************************
Declare Local Variables
************************/

DECLARE 
	@StartDate Datetime,
	@EndDate Datetime ,
	@MinClickId int,
	@MaxClickId int,
	@MinOpenId	int,
	@MaxOpenId	int

/************************
Set Variable values
************************/

SET @StartDate = @Date
SET @EndDate = DATEADD(SECOND,-1,(DATEADD(DAY, 1,@Date)))

SELECT 
	@MinOpenId = MinOpenId ,
	@MaxOpenId = MaxOpenId
FROM  ECN5_Warehouse.dbo.BlastOpenRangeByDate  WHERE CONVERT(DateTime,OpenDate) = @StartDate

SELECT 
	@MinClickId = MinClickId,
	@MaxClickId = MaxClickId
FROM  ECN5_Warehouse.dbo.BlastClickRangeByDate  WHERE CONVERT(DateTime,Clickdate) = @StartDate

/************************
Build Data Set
************************/

SELECT
	b.CustomerID, 
	cust.CustomerName, 
	ci.CampaignItemName, 
	b.BlastID, 
	b.EmailSubject, 
	GroupName, 
	b.SendTime, 
	b.StatusCode AS Status, 
	b.TestBlast AS IsTestBlast,
	ISNULL(Sendtotal,0) AS SendCount,
	ISNULL((SELECT COUNT(DISTINCT(EmailID))FROM ECN_Activity.dbo.BlastActivityOpens o2 WHERE OpenId BETWEEN @MinOpenId AND @MaxOpenId AND o2.blastid = b.blastid ),0)  AS UniqueEmailOpenCount,
	ISNULL((SELECT COUNT(DISTINCT(EmailID))FROM ECN_Activity.dbo.BlastActivityClicks c2 WHERE ClickId BETWEEN @MinClickId AND @MaxClickId AND c2.blastid = b.blastid ),0)  AS UniqueEmailClickCount 
FROM 
	 ECN5_COMMUNICATOR..Blast b 
	JOIN ECN5_ACCOUNTS.dbo.Customer cust on cust.CustomerID = b.CustomerID
	JOIN ECN5_Communicator.dbo.Groups gr on b.GroupID = gr.GroupID
	JOIN (SELECT CampaignItemId,Blastid from ECN5_Communicator.dbo.campaignItemBlast WHERE BlastID IS NOT NULL 
		 UNION SELECT CampaignItemId,blastid from ECN5_Communicator.dbo.CampaignItemTestBlast WHERE BlastID IS NOT NULL) cib ON cib.BlastID = b.BlastID
	JOIN ECN5_Communicator.dbo.CampaignItem ci on ci.CampaignItemID = cib.CampaignItemID
WHERE 
	b.blastid IN (select BlastID From Blast where SendTime BETWEEN @StartDate AND @EndDate)
	AND b.StatusCode IN ('Pending','Active','Sent')
GROUP BY
	b.CustomerID, 
	cust.CustomerName, 
	ci.CampaignItemName, 
	b.BlastID, 
	b.EmailSubject, 
	GroupName, 
	b.SendTime, 
	b.StatusCode, 
	b.TestBlast	,
	SendTotal
ORDER BY
	b.SendTime asc


GRANT EXEC ON rpt_TotalBlastsForDay to ECN5
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[rpt_TotalBlastsForDay] TO [ecn5]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[rpt_TotalBlastsForDay] TO [webuser]
    AS [dbo];

