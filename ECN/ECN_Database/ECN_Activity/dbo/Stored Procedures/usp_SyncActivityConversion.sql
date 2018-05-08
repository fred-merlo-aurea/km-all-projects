CREATE PROC [dbo].[usp_SyncActivityConversion]
AS 

SET NOCOUNT OFF 
-- EXECUTE usp_SyncActivityConversion

DECLARE 
	@MinEAID INT,
	@MaxEAID INT,
	@OldEAID INT

SELECT 
	@MinEAID = MaxEAID, @OldEAID = oldminEAID
FROM 
	ECN_Activity.dbo.ActivityLogIdSync
WHERE
	TargetTable = 'conversion'

if (@OldEAID is null)
	SELECT 
		@MaxEAID =max(eaid) 
	FROM 
		ECN5_COMMUNICATOR.dbo.EmailActivityLog a WITH(NOLOCK)
	WHERE 
		a.ActionTypeCode = 'Conversion'
		AND EAID >= @MinEAID
else
	SELECT 
		@MaxEAID =max(eaid) 
	FROM 
		ECN5_COMMUNICATOR.dbo.EmailActivityLog a WITH(NOLOCK)
	WHERE 
		a.ActionTypeCode = 'Conversion'
		AND EAID >= @MinEAID and EAID <  @OldEAID

CREATE TABLE #SyncTempConversion (
	BlastID			INT,
	EmailID			INT,
	ConversionTime	DATETIME,
	URL				VARCHAR(2048),
	EAID			INT)
CREATE INDEX IDX_SyncTempConversion ON #SyncTempConversion (BlastId,EmailId)

IF @MaxEAID IS NOT NULL
BEGIN 
	
--	Insert New Conversion Records
	INSERT INTO  #SyncTempConversion(
		BlastID,
		EmailID,
		ConversionTime,
		URL,
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
			a.ActionTypeCode IN ('conversion')
			AND EAID BETWEEN @MinEAID AND @MaxEAID
	)x
	where x.rownum = 1 and (x.email_emailid is not null OR x.history_newemailID IS NOT NULL)

	--Insert New Conversion Records	
	INSERT INTO  ecn_activity.dbo.BlastActivityConversion(
		BlastID,
		EmailID,
		ConversionTime,
		URL,
		EAID)
	SELECT 
		BlastID,
		EmailID,
		ConversionTime,
		URL,
		EAID
	FROM 
		#SyncTempConversion

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
		MIN(ConversionTime),
		'Browser info not available. Auto-generated Open based on Conversion Activity.' AS BrowserInfo,
		MIN(EAID),
		15 AS EmailClientID,
		5 AS PlatformID,
		MIN(DATEADD(second,Datepart(second,ConversionTime) - Datepart(second,ConversionTime) % 2  , CONVERT(DATETIME,CONVERT(SMALLDatetime,ConversionTime))))
	FROM 
		#SyncTempConversion s
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
		TargetTable = 'conversion'

END


DROP TABLE #SyncTempConversion