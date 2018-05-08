CREATE  PROC [dbo].[usp_SyncActivityOpen]
AS 

SET NOCOUNT OFF 
-- EXECUTE usp_SyncActivityOpen

	
DECLARE 
	@MinEAID INT,
	@MaxEAID INT,
	@OldEAID INT

SELECT 
	@MinEAID = MaxEAID, @OldEAID = oldminEAID
FROM 
	ECN_Activity.dbo.ActivityLogIdSync
WHERE
	TargetTable = 'Open'

if (@OldEAID is null)
	SELECT 
		@MaxEAID =max(eaid) 
	FROM 
		ECN5_COMMUNICATOR.dbo.EmailActivityLog a WITH(NOLOCK)
	WHERE 
		a.ActionTypeCode = 'Open'
		AND EAID >= @MinEAID
else
	SELECT 
		@MaxEAID =max(eaid) 
	FROM 
		ECN5_COMMUNICATOR.dbo.EmailActivityLog a WITH(NOLOCK)
	WHERE 
		a.ActionTypeCode = 'Open'
		AND EAID >= @MinEAID and EAID <  @OldEAID


CREATE TABLE #SyncTempOpens(
	BlastID			INT,
	EmailID			INT,
	OpenTime		DATETIME,
	BrowserInfo		VARCHAR(2048),
	EAID			INT,
	EmailClientID	INT,
	PlatformID		INT,
	RoundedTime		DATETIME)

CREATE INDEX IDX_SyncTempOpens ON #SyncTempOpens (BlastId,EmailId)

IF @MaxEAID IS NOT NULL
BEGIN 
		
--	Get New Open Records
	INSERT INTO #SyncTempOpens(
		BlastID			,
		EmailID			,
		OpenTime		,
		BrowserInfo		,
		EAID			,
		EmailClientID	,
		PlatformID		,
		RoundedTime		)

	Select 
		x.BlastID,
		x.CreatedEmailID,
		x.ActionDate,
		x.ActionValue,
		x.EAID,
		[ecn_Activity].dbo.[fn_GetEmailClientID] (x.ActionValue),
		[ecn_Activity].dbo.[fn_GetPlatformID] (x.ActionValue)		,
--		DATEADD(second,Datepart(second,ActionDate) - Datepart(second,ActionDate) % 2  ,DATEADD(ms,-Datepart(ms,ActionDate) ,DATEADD(second,-Datepart(second,ActionDate) ,ActionDate)))
		DATEADD(second,Datepart(second,CAST(x.ActionDate AS DATETIME2(0))) - Datepart(second,CAST(x.ActionDate AS DATETIME2(0))) % 2  , CONVERT(DATETIME,CONVERT(CHAR(16),x.ActionDate,120)))
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
			a.EAID
		FROM 
			ECN5_COMMUNICATOR.dbo.EmailActivityLog a WITH (NOLOCK)
			left join ECN5_COMMUNICATOR.dbo.Emails e  WITH (NOLOCK) on e.emailID = a.EmailID
			left join ECN5_COMMUNICATOR.dbo.EmailHistory h  WITH (NOLOCK) on h.OldEmailID = a.EmailID and h.Action = 'merge' 
		WHERE 
			a.ActionTypeCode IN ('open')
			AND EAID BETWEEN @MinEAID AND @MaxEAID
	)x
	where x.rownum = 1 and (x.email_emailid is not null OR x.history_newemailID IS NOT NULL)

	SELECT
		DISTINCT a.EAID 
	INTO 
		#delList
	FROM
		#SyncTempOpens a 
		JOIN #SyncTempOpens b on a.blastid = b.blastid and a.EmailID = b.EmailID 
	WHERE
		CONVERT(date,a.OpenTime) = CONVERT(date,b.OpenTime) 
		AND ABS(DATEDIFF(MS,a.OpenTime, b.OpenTime)) < 2000 
		and a.EAID > b.EAID
	ORDER BY 
		1

	DELETE 
		#SyncTempOpens 
	WHERE
		EAID IN (SELECT EAID FROM #delList)
			
	--Insert New Open Records
	INSERT dbo.BlastActivityOpens(
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
		OpenTime,
		BrowserInfo,
		EAID,
		EmailClientID,
		PlatformID,
		RoundedTime
	FROM 
		#SyncTempOpens		

	UPDATE
		ECN_Activity.dbo.ActivityLogIdSync
	SET
		MaxEAID  = @MaxEAID + 1
	WHERE
		TargetTable = 'open'

DROP TABLE #SyncTempOpens
DROP TABLE #DelList
END
