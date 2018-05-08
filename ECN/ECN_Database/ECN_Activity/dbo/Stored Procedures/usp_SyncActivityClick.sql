CREATE PROC [dbo].[usp_SyncActivityClick]
AS 

SET NOCOUNT OFF 
-- EXECUTE usp_SyncActivityClick

DECLARE 
	@MinEAID INT,
	@MaxEAID INT,
	@OldEAID INT


SELECT 
	@MinEAID = MaxEAID, @OldEAID = oldminEAID
FROM 
	ECN_Activity.dbo.ActivityLogIdSync
WHERE
	TargetTable = 'click'

if (@OldEAID is null)
	SELECT 
		@MaxEAID =max(eaid) 
	FROM 
		ECN5_COMMUNICATOR.dbo.EmailActivityLog a WITH(NOLOCK)
	WHERE 
		a.ActionTypeCode = 'Click'
		AND EAID >= @MinEAID
else
	SELECT 
		@MaxEAID =max(eaid) 
	FROM 
		ECN5_COMMUNICATOR.dbo.EmailActivityLog a WITH(NOLOCK)
	WHERE 
		a.ActionTypeCode = 'Click'
		AND EAID >= @MinEAID and EAID <  @OldEAID






CREATE TABLE #SyncTempClicks (
	BlastID			INT,
	EmailID			INT,
	ClickTime		DATETIME,
	URL				VARCHAR(2048),
	BlastLinkID		INT,
	EAID			INT,
	UniqueLinkID	INT,
	RoundedTime		DATETIME)

CREATE INDEX IDX_SyncTempClicks ON #SyncTempClicks (BlastId,EmailId)

IF @MaxEAID IS NOT NULL
BEGIN 
--	Get New Click Records
	INSERT INTO #SyncTempClicks(
		BlastID	,
		EmailID	,
		ClickTime,
		URL		,
		BlastLinkID,
		EAID		,
		UniqueLinkID,
		RoundedTime	)
	Select 
		x.BlastID,
		x.CreatedEmailID,
		x.ActionDate,
		x.ActionValue,
		NULL AS BlastLinkID,
		x.EAID,
		CASE WHEN isnumeric(x.ActionNotes) = 1 THEN Convert(int,x.ActionNotes) ELSE NULL END AS UniqueLinkID,
		DATEADD(second,Datepart(second,x.ActionDate) - Datepart(second,x.ActionDate) % 5  ,DATEADD(ms,-Datepart(ms,x.ActionDate) ,DATEADD(second,-Datepart(second,x.ActionDate) ,x.ActionDate))) 
--		DATEADD(second,Datepart(second,ActionDate) - Datepart(second,ActionDate) % 5  , CONVERT(DATETIME,CONVERT(SMALLDatetime,ActionDate)))
	from
	(
		SELECT 
			ROW_NUMBER() OVER (partition by e.EmailID,a.BlastID,a.EmailID,a.ActionDate order by a.EAID) as rownum,
			coalesce(h.newEmailID, a.EmailID) as CreatedEmailID,
			a.EmailID as activityEmailID,
			h.NewEmailID history_newemailID,
			e.EmailID email_emailid,
			a.BlastID,
			a.EmailID activity_emailID,
			a.ActionDate,
			a.ActionValue,
			a.ActionNotes,
			a.EAID
		FROM 
			ECN5_COMMUNICATOR.dbo.EmailActivityLog a WITH (NOLOCK)
			left join ECN5_COMMUNICATOR.dbo.Emails e  WITH (NOLOCK) on e.emailID = a.EmailID
			left join ECN5_COMMUNICATOR.dbo.EmailHistory h  WITH (NOLOCK) on h.OldEmailID = a.EmailID and h.Action = 'merge' 
		WHERE 
			a.ActionTypeCode IN ('click')
			AND EAID BETWEEN @MinEAID AND @MaxEAID
	)x
	where x.rownum = 1 and (x.email_emailid is not null OR x.history_newemailID IS NOT NULL)

--Remove overlapping records
	SELECT
		DISTINCT a.EAID 
	INTO 
		#delList
	FROM
		#SyncTempClicks a 
		JOIN #SyncTempClicks b on a.blastid = b.blastid and a.EmailID = b.EmailID 
	WHERE
		CONVERT(date,a.ClickTime) = CONVERT(date,b.ClickTime) 
		AND ABS(DATEDIFF(MS,a.ClickTime, b.ClickTime)) < 5000 
		and a.EAID > b.EAID
	ORDER BY 
		1

	DELETE 
		#SyncTempClicks
	WHERE
		EAID IN (SELECT EAID FROM #delList)


--Insert New Click Records
	INSERT INTO ecn_activity.dbo.BlastActivityClicks(
		BlastID	,
		EmailID	,
		ClickTime,
		URL		,
		BlastLinkID,
		EAID		,
		UniqueLinkID,
		RoundedTime	)
	SELECT
		BlastID	,
		EmailID	,
		ClickTime,
		URL		,
		BlastLinkID,
		EAID		,
		UniqueLinkID,
		RoundedTime
	FROM 
		#SyncTempClicks

--Create Open Record If Missing
	INSERT INTO ecn_activity.dbo.BlastActivityOpens (
		BlastID,
		EmailID,
		OpenTime,
		BrowserInfo,
		EAID,
		EmailClientID,
		PlatformID,
		RoundedTime)
	SELECT 
		BlastID,
		EmailID,
		MIN (ClickTime),
		'Browser info not available. Auto-generated Open based on Click Activity.' AS BrowserInfo,
		MIN (EAID),
		15 AS EmailClientID,
		5 AS PlatformID,
		MIN(DATEADD(second,Datepart(second,ClickTime) - Datepart(second,ClickTime) % 2  ,DATEADD(ms,-Datepart(ms,ClickTime) ,DATEADD(second,-Datepart(second,ClickTime) ,ClickTime)))) AS RoundedTime
--		MIN(DATEADD(second,Datepart(second,ClickTime) - Datepart(second,ClickTime) % 2  , CONVERT(DATETIME,CONVERT(SMALLDatetime,ClickTime))))
	FROM 
		#SyncTempClicks s
	WHERE 
		NOT EXISTS (SELECT 1 FROM ecn_activity.dbo.BlastActivityOpens ao WHERE s.BlastId = ao.BlastId AND s.EmailId = ao.EmailId)
	GROUP BY
		BlastID,
		EmailID

	UPDATE
		ECN_Activity.dbo.ActivityLogIdSync
	SET
		MaxEAID  = @MaxEAID + 1
	WHERE
		TargetTable = 'click'

	DROP TABLE #DelList
END
DROP TABLE #SyncTempClicks
