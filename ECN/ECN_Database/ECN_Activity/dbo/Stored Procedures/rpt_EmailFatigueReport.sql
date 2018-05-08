
CREATE Procedure [dbo].[rpt_EmailFatigueReport]

@CustomerId	INT,
@StartDate	Date = NULL,
@EndDate	Date = NULL	,
@FilterField Varchar(100) = NULL ,
@FIlterValue Varchar(100) = NULL 
--Filter Field is MessageTypeId, GroupID, CampaignId.  
AS

BEGIN

	--declare @CustomerId	INT,
	--@StartDate	DateTime = NULL,
	--@EndDate	DateTime = NULL	,
	--@FilterField Varchar(100) = NULL ,
	--@FIlterValue Varchar(100) = NULL 

	--set @CustomerId = 1797
	--set @StartDate = '03/01/2014'
	--set @EndDate  = '03/28/2014'


	SET NOCOUNT ON

	IF @StartDate IS NULL SET @StartDate = DATEADD (MONTH , -1, GETDATE())
	IF @EndDate IS NULL SET @EndDate = GETDATE()


	--TEMPORARY LOG PROC STATS
		DECLARE @ID UNIQUEIDENTIFIER
		SET @ID = NEWID()

		INSERT INTO ecn_Temp.dbo.StatsFatigueReport (
			Id,
			CustomerId,
			ParamStartDate,
			ParamEndDate,
			FilterField,
			FilterValue,
			ExecStartTime,
			ExecEndTime)
		
		VALUES (
			@ID,
			@CustomerId,
			@StartDate,
			@EndDate,
			@FilterField,
			@FIlterValue,
			GETDATE(),
			NULL)

	DECLARE 
		@MinSendId INT,
		@MinOpenId INT,
		@MinClickId INT
	--	@MaxSendId INT,
	--	@MaxOpenId INT,
	--	@MaxClickId INT


	SELECT @MinOpenId = MIN(Minopenid) from ECN5_Warehouse.dbo.BlastOpenRangeByDate where CONVERT(Date,opendate) >= @StartDate
	SELECT @MinClickId = MIN(MinClickid) from ECN5_Warehouse.dbo.BlastClickRangeByDate where CONVERT(Date,clickdate) >= @StartDate
	SELECT @MinSendId =  MIN(MinSendid) from ECN5_Warehouse.dbo.BlastSEndRangeByDate where CONVERT(Date,senddate) >= @StartDate

	--SELECT @MaxOpenId = MAX(Maxopenid) from ECN5_Warehouse.dbo.BlastOpenRangeByDate where CONVERT(Date,opendate) <= @EndDate
	--SELECT @MaxClickId = MAX(MaxClickid) from ECN5_Warehouse.dbo.BlastClickRangeByDate where CONVERT(Date,clickdate) <= @EndDate
	--SELECT @MaxSendId =  MAX(MaxSendid) from ECN5_Warehouse.dbo.BlastSEndRangeByDate where CONVERT(Date,senddate) <= @EndDate

	CREATE TABLE #Summary (
		Touches INT, 
		Opens	INT,
		Clicks	INT,
		Emails	INT
		)

	------------------------------------------------------------
	--BUILD DATA SET
	------------------------------------------------------------

	declare @opens table (emailID int, blastID int, UNIQUE CLUSTERED (emailID,blastID))
	declare @Clicks table (emailID int, blastID int, UNIQUE CLUSTERED (emailID,blastID))


	insert into @opens
	SELECT distinct bab.emailID, bab.BlastID from ECN_Activity.dbo.BlastActivityOpens bab WITH (NOLOCK) join ECN5_COMMUNICATOR..Blast b WITH (NOLOCK) on bab.BlastID = b.blastID  
	WHERE CustomerID = @CustomerId and TestBlast ='N' and OpenID >= @MinOpenId 


	insert into @Clicks
	SELECT distinct bac.emailID,bac.BlastID FROM ECN_Activity.dbo.BlastActivityClicks bac WITH (NOLOCK)  join ECN5_COMMUNICATOR..Blast b WITH (NOLOCK) on bac.BlastID = b.blastID  
	WHERE CustomerID = @CustomerId and TestBlast ='N' and  ClickID >= @MinClickId 


	IF @FilterField = 'GroupID'
	BEGIN


	INSERT INTO #Summary
	SELECT 
		Touches, 
		Opens,
		Clicks,
		COUNT(emailid) as Emails
	FROM
	(
		SELECT 
				s.EmailID, 
				COUNT(DISTINCT(s.BlastID)) AS Touches,
				COUNT (DISTINCT(o.BlastID))  AS Opens,
				COUNT(DISTINCT(c.BlastId)) AS Clicks
		FROM 
			ECN_Activity.dbo.BlastActivitySends s WITH (NOLOCK)
			JOIN ECN5_COMMUNICATOR..Blast b WITH (NOLOCK) on b.BlastID = s.BlastID 
			LEFT JOIN @opens o ON s.blastid = o.blastid and s.emailid = o.emailid 
			LEFT JOIN @Clicks c ON s.blastid = c.blastid and s.emailid = c.emailid
		WHERE 
			b.CustomerID = @CustomerId 
			and B.TestBlast ='N'			
			AND cast (b.SendTime as date) BETWEEN @StartDate AND @EndDate
			AND b.GroupID = @FIlterValue
		GROUP BY
			s.EmailID
	) a
	Group By touches,Opens,Clicks
	ORDER by 1,2,3



	END

	IF @FilterField = 'MessageTypeId'

	BEGIN

	INSERT INTO #Summary
	SELECT 
		Touches, 
		Opens,
		Clicks,
		COUNT(emailid) as Emails
	FROM
	(
		SELECT 
				s.EmailID, 
				COUNT(DISTINCT(s.BlastID)) AS Touches,
				COUNT (DISTINCT(o.BlastID))  AS Opens,
				COUNT(DISTINCT(c.BlastId)) AS Clicks
		FROM 
			ECN_Activity.dbo.BlastActivitySends s WITH (NOLOCK)
			JOIN ECN5_COMMUNICATOR..Blast b WITH (NOLOCK) on b.BlastID = s.BlastID 
			JOIN ECN5_Communicator.dbo.Layout l WITH (NOLOCK)  on l.LayoutID = B.LayoutID 
			JOIN ECN5_Communicator.dbo.MessageType mt WITH (NOLOCK)  on mt.MessageTypeID = l.MessageTypeID
			LEFT JOIN @opens o ON s.blastid = o.blastid and s.emailid = o.emailid 
			LEFT JOIN @Clicks c ON s.blastid = c.blastid and s.emailid = c.emailid
		WHERE 
			b.CustomerID = @CustomerId 
			AND B.TestBlast = 'N'
			AND cast( b.SendTime as date) BETWEEN @StartDate AND @EndDate
			AND mt.MessageTypeID = @FIlterValue
		GROUP BY
			s.EmailID
	) a
	Group By touches,Opens,Clicks
	ORDER by 1,2,3

		
	END

	IF @FilterField = 'CampaignId'
	BEGIN

	INSERT INTO #Summary
	SELECT 
		Touches, 
		Opens,
		Clicks,
		COUNT(emailid) as Emails
	FROM
	(
		SELECT 
				s.EmailID, 
				COUNT(DISTINCT(s.BlastID)) AS Touches,
				COUNT (DISTINCT(o.BlastID))  AS Opens,
				COUNT(DISTINCT(c.BlastId)) AS Clicks
		FROM 
			ECN_Activity.dbo.BlastActivitySends s WITH (NOLOCK)
			JOIN ECN5_COMMUNICATOR..Blast b WITH (NOLOCK) on b.BlastID = s.BlastID 
			JOIN ECN5_Communicator.dbo.campaignItemBlast cib  WITH (NOLOCK) ON cib.BlastID = b.BlastID
			JOIN ECN5_Communicator.dbo.CampaignItem ci  WITH (NOLOCK) on ci.CampaignItemID = cib.CampaignItemID
			JOIN ECN5_Communicator.dbo.Campaign cam  WITH (NOLOCK) on ci.CampaignID = cam.CampaignID
			LEFT JOIN @opens o ON s.blastid = o.blastid and s.emailid = o.emailid 
			LEFT JOIN @Clicks c ON s.blastid = c.blastid and s.emailid = c.emailid
		WHERE 
			b.CustomerID = @CustomerId 
			and TestBlast ='N'			
			AND cast( b.SendTime as date) BETWEEN @StartDate AND @EndDate
			AND cam.CampaignId = @FIlterValue
			--AND cam.CampaignName = @FIlterValue
		GROUP BY
			s.EmailID
	) a
	Group By touches,Opens,Clicks
	ORDER by 1,2,3


	END

	IF @FilterField IS NULL

	BEGIN

	INSERT INTO #Summary
	SELECT 
		Touches, 
		Opens,
		Clicks,
		COUNT(emailid) as Emails
	FROM
	(
		SELECT 
				s.EmailID, 
				COUNT(DISTINCT(s.BlastID)) AS Touches,
				COUNT (DISTINCT(o.BlastID))  AS Opens,
				COUNT(DISTINCT(c.BlastId)) AS Clicks
		FROM 
			ECN_Activity.dbo.BlastActivitySends s WITH (NOLOCK)
			JOIN ECN5_COMMUNICATOR..Blast b WITH (NOLOCK) on b.BlastID = s.BlastID 
			LEFT JOIN @opens o ON s.blastid = o.blastid and s.emailid = o.emailid 
			LEFT JOIN @Clicks c ON s.blastid = c.blastid and s.emailid = c.emailid
		WHERE 
			b.CustomerID = @CustomerId 
			and TestBlast ='N'
			AND cast( b.SendTime as date) BETWEEN @StartDate AND @EndDate
		GROUP BY
			s.EmailID
	) a
	Group By touches,Opens,Clicks
	ORDER by 1,2,3


	END



	----------------------
	--Display data set
	----------------------

	--SELECT 
	--(SELECT COUNT (DISTINCT( BlastID)) from ECN5_COMMUNICATOR..Blast where CustomerID = @CustomerId and SendTime BETWEEN @StartDate AND @EndDate) AS TotalBlasts,
	--(SELECT COUNT (DISTINCT( BlastID)) from ECN5_COMMUNICATOR..Blast where CustomerID = @CustomerId and SendTime BETWEEN @StartDate AND @EndDate) AS TotalBlasts,


	select TOP 1 
	--	-1 'Ignore',
		-1 as Grouping ,
		'Sent' AS Action,
		ISNULL((SELECT SUM(emails) from #summary where touches = 1 ),0) AS Touch1, 
		ISNULL((SELECT SUM(emails) from #summary where touches = 2 ),0) AS Touch2,
		ISNULL((SELECT SUM(emails) from #summary where touches = 3 ),0) AS Touch3,
		ISNULL((SELECT SUM(emails) from #summary where touches = 4 ),0) AS Touch4,
		ISNULL((SELECT SUM(emails) from #summary where touches = 5 ),0) AS Touch5,
		ISNULL((SELECT SUM(emails) from #summary where touches = 6 ),0) AS Touch6,
		ISNULL((SELECT SUM(emails) from #summary where touches = 7 ),0) AS Touch7,
		ISNULL((SELECT SUM(emails) from #summary where touches = 8 ),0) AS Touch8,
		ISNULL((SELECT SUM(emails) from #summary where touches = 9 ),0) AS Touch9,
		ISNULL((SELECT SUM(emails) from #summary where touches = 10 ),0) AS Touch10,
		ISNULL((SELECT SUM(emails) from #summary where touches BETWEEN 11 AND 20),0) AS Touch11_20,
		ISNULL((SELECT SUM(emails) from #summary where touches BETWEEN 21 AND 30),0) AS Touch21_30,
		ISNULL((SELECT SUM(emails) from #summary where touches BETWEEN 31 AND 40),0) AS Touch31_40,
		ISNULL((SELECT SUM(emails) from #summary where touches BETWEEN 41 AND 50),0) AS Touch41_50,
		ISNULL((SELECT SUM(emails) from #summary where touches > 50),0) AS Touch51Plus
	from #summary 


	UNION ALL


	SELECT DISTINCT 
	--	0,
		CASE 
			WHEN a.Opens BETWEEN 11 AND 20 THEN 11
			WHEN a.Opens BETWEEN 21 AND 30 THEN 21
			WHEN a.Opens BETWEEN 31 AND 40 THEN 31
			WHEN a.Opens BETWEEN 41 AND 50 THEN 41
			WHEN a.Opens > 50 THEN 51
			ELSE a.Opens END,
		'Opens',
		ISNULL(MAX([1]),0),
		ISNULL(MAX([2]),0),
		ISNULL(MAX([3]),0),
		ISNULL(MAX([4]),0),
		ISNULL(MAX([5]),0),
		ISNULL(MAX([6]),0),
		ISNULL(MAX([7]),0),
		ISNULL(MAX([8]),0),
		ISNULL(MAX([9]),0),
		ISNULL(MAX([10]),0),
		ISNULL(MAX([11-20]),0),
		ISNULL(MAX([21-30]),0),
		ISNULL(MAX([31-40]),0),
		ISNULL(MAX([41-50]),0),
		ISNULL(MAX([50+]),0)

	FROM 
		#summary  A 
	LEFT JOIN (SELECT Opens,SUM(emails)[1] from #summary where touches = 1 group by Opens) b1 ON A.Opens = b1.Opens
	LEFT JOIN (SELECT Opens,SUM(emails)[2] from #summary where touches = 2 group by Opens) b2 ON A.Opens = b2.Opens
	LEFT JOIN (SELECT Opens,SUM(emails)[3] from #summary where touches = 3 group by Opens) b3 ON A.Opens = b3.Opens
	LEFT JOIN (SELECT Opens,SUM(emails)[4] from #summary where touches = 4 group by Opens) b4 ON A.Opens = b4.Opens
	LEFT JOIN (SELECT Opens,SUM(emails)[5] from #summary where touches = 5 group by Opens) b5 ON A.Opens = b5.Opens
	LEFT JOIN (SELECT Opens,SUM(emails)[6] from #summary where touches = 6 group by Opens) b6 ON A.Opens = b6.Opens
	LEFT JOIN (SELECT Opens,SUM(emails)[7] from #summary where touches = 7 group by Opens) b7 ON A.Opens = b7.Opens
	LEFT JOIN (SELECT Opens,SUM(emails)[8] from #summary where touches = 8 group by Opens) b8 ON A.Opens = b8.Opens
	LEFT JOIN (SELECT Opens,SUM(emails)[9] from #summary where touches = 9 group by Opens) b9 ON A.Opens = b9.Opens
	LEFT JOIN (SELECT Opens,SUM(emails)[10] from #summary where touches = 10 group by Opens) b10 ON A.Opens = b10.Opens
	LEFT JOIN (SELECT Opens,SUM(emails)[11-20] from #summary where touches BETWEEN 11 AND 20 group by Opens) b11 ON A.Opens = b11.Opens
	LEFT JOIN (SELECT Opens,SUM(emails)[21-30] from #summary where touches BETWEEN 21 AND 30 group by Opens) b21 ON A.Opens = b21.Opens
	LEFT JOIN (SELECT Opens,SUM(emails)[31-40] from #summary where touches BETWEEN 31 AND 40 group by Opens) b31 ON A.Opens = b31.Opens
	LEFT JOIN (SELECT Opens,SUM(emails)[41-50] from #summary where touches BETWEEN 41 AND 50 group by Opens) b41 ON A.Opens = b41.Opens
	LEFT JOIN (SELECT Opens,SUM(emails)[50+] from #summary where touches > 50 group by Opens) b51 ON A.Opens = b51.Opens
	GROUP BY
		CASE
			WHEN a.Opens BETWEEN 11 AND 20 THEN 11
			WHEN a.Opens BETWEEN 21 AND 30 THEN 21
			WHEN a.Opens BETWEEN 31 AND 40 THEN 31
			WHEN a.Opens BETWEEN 41 AND 50 THEN 41
			WHEN a.Opens > 50 THEN 51
			ELSE a.Opens END
	--ORDER BY 1,3,2 Desc

	UNION ALL

	SELECT DISTINCT 
	--	0,
		CASE 
			WHEN a.Clicks BETWEEN 11 AND 20 THEN 11
			WHEN a.Clicks BETWEEN 21 AND 30 THEN 21
			WHEN a.Clicks BETWEEN 31 AND 40 THEN 31
			WHEN a.Clicks BETWEEN 41 AND 50 THEN 41
			WHEN a.Clicks > 50 THEN 51
			ELSE a.Clicks END,
		'Clicks',
		ISNULL(MAX([1]),0),
		ISNULL(MAX([2]),0),
		ISNULL(MAX([3]),0),
		ISNULL(MAX([4]),0),
		ISNULL(MAX([5]),0),
		ISNULL(MAX([6]),0),
		ISNULL(MAX([7]),0),
		ISNULL(MAX([8]),0),
		ISNULL(MAX([9]),0),
		ISNULL(MAX([10]),0),
		ISNULL(MAX([11-20]),0),
		ISNULL(MAX([21-30]),0),
		ISNULL(MAX([31-40]),0),
		ISNULL(MAX([41-50]),0),
		ISNULL(MAX([50+]),0)

	FROM 
		#summary  A 
	LEFT JOIN (SELECT Clicks,SUM(emails)[1] from #summary where touches = 1 group by Clicks) b1 ON A.Clicks = b1.Clicks
	LEFT JOIN (SELECT Clicks,SUM(emails)[2] from #summary where touches = 2 group by Clicks) b2 ON A.Clicks = b2.Clicks
	LEFT JOIN (SELECT Clicks,SUM(emails)[3] from #summary where touches = 3 group by Clicks) b3 ON A.Clicks = b3.Clicks
	LEFT JOIN (SELECT Clicks,SUM(emails)[4] from #summary where touches = 4 group by Clicks) b4 ON A.Clicks = b4.Clicks
	LEFT JOIN (SELECT Clicks,SUM(emails)[5] from #summary where touches = 5 group by Clicks) b5 ON A.Clicks = b5.Clicks
	LEFT JOIN (SELECT Clicks,SUM(emails)[6] from #summary where touches = 6 group by Clicks) b6 ON A.Clicks = b6.Clicks
	LEFT JOIN (SELECT Clicks,SUM(emails)[7] from #summary where touches = 7 group by Clicks) b7 ON A.Clicks = b7.Clicks
	LEFT JOIN (SELECT Clicks,SUM(emails)[8] from #summary where touches = 8 group by Clicks) b8 ON A.Clicks = b8.Clicks
	LEFT JOIN (SELECT Clicks,SUM(emails)[9] from #summary where touches = 9 group by Clicks) b9 ON A.Clicks = b9.Clicks
	LEFT JOIN (SELECT Clicks,SUM(emails)[10] from #summary where touches = 10 group by Clicks) b10 ON A.Clicks = b10.Clicks
	LEFT JOIN (SELECT Clicks,SUM(emails)[11-20] from #summary where touches BETWEEN 11 AND 20 group by Clicks) b11 ON A.Clicks = b11.Clicks
	LEFT JOIN (SELECT Clicks,SUM(emails)[21-30] from #summary where touches BETWEEN 21 AND 30 group by Clicks) b21 ON A.Clicks = b21.Clicks
	LEFT JOIN (SELECT Clicks,SUM(emails)[31-40] from #summary where touches BETWEEN 31 AND 40 group by Clicks) b31 ON A.Clicks = b31.Clicks
	LEFT JOIN (SELECT Clicks,SUM(emails)[41-50] from #summary where touches BETWEEN 41 AND 50 group by Clicks) b41 ON A.Clicks = b41.Clicks
	LEFT JOIN (SELECT Clicks,SUM(emails)[50+] from #summary where touches > 50 group by Clicks) b51 ON A.Clicks = b51.Clicks
	GROUP BY
		CASE
			WHEN a.Clicks BETWEEN 11 AND 20 THEN 11
			WHEN a.Clicks BETWEEN 21 AND 30 THEN 21
			WHEN a.Clicks BETWEEN 31 AND 40 THEN 31
			WHEN a.Clicks BETWEEN 41 AND 50 THEN 41
			WHEN a.Clicks > 50 THEN 51
			ELSE a.Clicks END
	ORDER BY 1,2,3 Desc

	--TEMPORARY LOG PROC TIME
	UPDATE Ecn_Temp.dbo.StatsFatigueReport 
	SET 
		ExecEndTime = GETDATE()
	WHERE 
		Id = @ID
		AND ExecEndTime IS NULL	


	DROP TABLE #Summary

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[rpt_EmailFatigueReport] TO [webuser]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[rpt_EmailFatigueReport] TO [ecn5]
    AS [dbo];

