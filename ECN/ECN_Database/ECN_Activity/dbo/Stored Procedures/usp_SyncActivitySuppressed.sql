CREATE PROC [dbo].[usp_SyncActivitySuppressed]
AS 

SET NOCOUNT OFF 
--exec usp_syncActivitySuppressed
	DECLARE 
	@MinEAID INT,
	@MaxEAID INT,
	@OldEAID INT

SELECT 
	@MinEAID = MaxEAID, @OldEAID = oldminEAID
FROM 
	ECN_Activity.dbo.ActivityLogIdSync
WHERE
	TargetTable = 'suppressed'

if (@OldEAID is null)
	SELECT 
		@MaxEAID =max(eaid) 
	FROM 
		ECN5_COMMUNICATOR.dbo.EmailActivityLog a WITH(NOLOCK)
	WHERE 
		a.ActionTypeCode = 'suppressed'
		AND EAID >= @MinEAID
else
	SELECT 
		@MaxEAID =max(eaid) 
	FROM 
		ECN5_COMMUNICATOR.dbo.EmailActivityLog a WITH(NOLOCK)
	WHERE 
		a.ActionTypeCode = 'suppressed'
		AND EAID >= @MinEAID and EAID <  @OldEAID

IF @MaxEAID IS NOT NULL
BEGIN 
	
	--Insert New Suppresssed Records
	INSERT ecn_activity.dbo.BlastActivitySuppressed(
		BlastID,
		EmailID,
		SuppressedCodeID,
		SuppressedTime,
		EAID,
		BlastsAlreadySent)
	Select 
		x.BlastID,
		x.CreatedEmailID,
		x.SuppressedCodeID,
		x.ActionDate,
		x.EAID,
		x.ActionNotes
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
			a.ActionNotes,
			c.SuppressedCodeID,
			a.EAID
		FROM 
			ECN5_COMMUNICATOR.dbo.EmailActivityLog a WITH (NOLOCK)
			LEFT JOIN ECN_ACTIVITY.dbo.SuppressedCodes c WITH (NOLOCK)  ON a.ActionValue = c.SupressedCode
			left join ECN5_COMMUNICATOR.dbo.Emails e  WITH (NOLOCK) on e.emailID = a.EmailID
			left join ECN5_COMMUNICATOR.dbo.EmailHistory h  WITH (NOLOCK) on h.OldEmailID = a.EmailID and h.Action = 'merge' 
		WHERE 
			a.ActionTypeCode IN ('suppressed')
			AND EAID BETWEEN @MinEAID AND @MaxEAID
	)x
	where x.rownum = 1 and (x.email_emailid is not null OR x.history_newemailID IS NOT NULL)
		
	
	UPDATE
		ECN_Activity.dbo.ActivityLogIdSync
	SET
		MaxEAID  = @MaxEAID + 1
	WHERE
		TargetTable = 'suppressed'
END
