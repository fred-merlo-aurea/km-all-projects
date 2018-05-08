CREATE PROC [dbo].[usp_SyncActivityRefer]
AS 

SET NOCOUNT OFF 
-- execute usp_SyncActivityRefer

	
	DECLARE 
	@MinEAID INT,
	@MaxEAID INT,
	@OldEAID INT

SELECT 
	@MinEAID = MaxEAID, @OldEAID = oldminEAID
FROM 
	ECN_Activity.dbo.ActivityLogIdSync
WHERE
	TargetTable = 'Refer'

if (@OldEAID is null)
	SELECT 
		@MaxEAID =max(eaid) 
	FROM 
		ECN5_COMMUNICATOR.dbo.EmailActivityLog a WITH(NOLOCK)
	WHERE 
		a.ActionTypeCode = 'Refer'
		AND EAID >= @MinEAID
else
	SELECT 
		@MaxEAID =max(eaid) 
	FROM 
		ECN5_COMMUNICATOR.dbo.EmailActivityLog a WITH(NOLOCK)
	WHERE 
		a.ActionTypeCode = 'Refer'
		AND EAID >= @MinEAID and EAID <  @OldEAID

CREATE TABLE #SyncTempRefer(
	BlastID			INT,
	EmailID			INT,
	ReferTime		DATETIME,
	EmailAddress	VARCHAR(255),
	EAID			INT)
CREATE INDEX IDX_SyncTempRefer ON #SyncTempRefer (BlastId,EmailId)

IF @MaxEAID IS NOT NULL
BEGIN 
--	Insert New Refer Records
	INSERT dbo.#SyncTempRefer(
		BlastID,
		EmailID,
		ReferTime,
		EmailAddress,
		EAID)
	Select 
		x.BlastID,
		x.CreatedEmailID,
		x.ActionDate,
		x.ActionValue,
		x.EAID
	FROM
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
			a.ActionTypeCode IN ('Refer')
			AND EAID BETWEEN @MinEAID AND @MaxEAID
	)x
	where x.rownum = 1 and (x.email_emailid is not null OR x.history_newemailID IS NOT NULL)

	INSERT INTO ecn_activity.dbo.BlastActivityRefer(
		BlastId,
		EmailId,
		ReferTime,
		EmailAddress,
		EAID)
	SELECT 
		BlastId,
		EmailId,
		ReferTime,
		EmailAddress,
		EAID
	FROM
		#SyncTempRefer

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
		MIN(ReferTime),
		'Browser info not available. Auto-generated Open based on Forward Activity.' AS BrowserInfo,
		MIN(EAID),
		15 AS EmailClientID,
		5 AS PlatformID,
		MIN(DATEADD(second,Datepart(second,ReferTime) - Datepart(second,ReferTime) % 2  , CONVERT(DATETIME,CONVERT(SMALLDatetime,ReferTime))))
	FROM 
		#SyncTempRefer s
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
		TargetTable = 'Refer'

END
DROP TABLE #SyncTempRefer
	