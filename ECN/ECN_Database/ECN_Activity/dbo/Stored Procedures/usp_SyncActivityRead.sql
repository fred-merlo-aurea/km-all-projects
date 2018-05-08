CREATE PROC [dbo].[usp_SyncActivityRead]
AS 

SET NOCOUNT OFF 
-- execute usp_SyncActivityRead

	

	DECLARE 
	@MinEAID INT,
	@MaxEAID INT,
	@OldEAID INT

SELECT 
	@MinEAID = MaxEAID, @OldEAID = oldminEAID
FROM 
	ECN_Activity.dbo.ActivityLogIdSync
WHERE
	TargetTable = 'Read'

if (@OldEAID is null)
	SELECT 
		@MaxEAID =max(eaid) 
	FROM 
		ECN5_COMMUNICATOR.dbo.EmailActivityLog a WITH(NOLOCK)
	WHERE 
		a.ActionTypeCode = 'Read'
		AND EAID >= @MinEAID
else
	SELECT 
		@MaxEAID =max(eaid) 
	FROM 
		ECN5_COMMUNICATOR.dbo.EmailActivityLog a WITH(NOLOCK)
	WHERE 
		a.ActionTypeCode = 'Read'
		AND EAID >= @MinEAID and EAID <  @OldEAID

CREATE TABLE #SyncTempReads (
	BlastID			INT,
	EmailID			INT,
	ReadTime		DATETIME,
	BrowserInfo		VARCHAR(2048),
	EAID			INT,
	EmailClientID	INT,
	PlatformID		INT)
CREATE INDEX IDX_SyncTempReads ON #SyncTempReads (BlastId,EmailId)



IF @MaxEAID IS NOT NULL
BEGIN 
	
			
	--Insert New Read Records
	INSERT #SyncTempReads(
		BlastID,
		EmailID,
		ReadTime,
		BrowserInfo,
		EAID,
		EmailClientID,
		PlatformID)
	Select 
		x.BlastID,
		x.CreatedEmailID,
		x.ActionDate,
		x.ActionValue,
		x.EAID,
		[ecn_Activity].dbo.[fn_GetEmailClientID] (x.ActionValue),
		[ecn_Activity].dbo.[fn_GetPlatformID] (x.ActionValue)		
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
			a.ActionTypeCode IN ('Read')
			AND EAID BETWEEN @MinEAID AND @MaxEAID
	)x
	where x.rownum = 1 and (x.email_emailid is not null OR x.history_newemailID IS NOT NULL)
	
	INSERT INTO ecn_activity.dbo.BlastActivityReads(
		BlastID,
		EmailID,
		ReadTime,
		BrowserInfo,
		EAID,
		EmailClientID,
		PlatformID)
	SELECT 
		BlastID,
		EmailID,
		ReadTime,
		BrowserInfo,
		EAID,
		EmailClientID,
		PlatformID
	FROM
		#SyncTempReads


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
		MIN(ReadTime),
		'Browser info not available. Auto-generated Open based on Read Activity.' AS BrowserInfo,
		MIN(EAID),
		15 AS EmailClientID,
		5 AS PlatformID,
		MIN(DATEADD(second,Datepart(second,ReadTime) - Datepart(second,ReadTime) % 2  , CONVERT(DATETIME,CONVERT(SMALLDatetime,ReadTime))))
	FROM 
		#SyncTempReads s
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
		TargetTable = 'Read'
END
DROP table #SyncTempReads
