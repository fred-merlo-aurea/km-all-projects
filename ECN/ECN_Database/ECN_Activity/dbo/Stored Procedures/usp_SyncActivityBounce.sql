CREATE PROC [dbo].[usp_SyncActivityBounce]
AS 

SET NOCOUNT OFF 
-- execute usp_SyncActivityBounce

DECLARE 
	@MinEAID INT,
	@MaxEAID INT,
	@OldEAID INT

SELECT 
	@MinEAID = MaxEAID, @OldEAID = oldminEAID
FROM 
	ECN_Activity.dbo.ActivityLogIdSync
WHERE
	TargetTable = 'bounce'

if (@OldEAID is null)
	SELECT 
		@MaxEAID =max(eaid) 
	FROM 
		ECN5_COMMUNICATOR.dbo.EmailActivityLog a WITH(NOLOCK)
	WHERE 
		a.ActionTypeCode = 'bounce'
		AND EAID >= @MinEAID
else
	SELECT 
		@MaxEAID =max(eaid) 
	FROM 
		ECN5_COMMUNICATOR.dbo.EmailActivityLog a WITH(NOLOCK)
	WHERE 
		a.ActionTypeCode = 'bounce'
		AND EAID >= @MinEAID and EAID <  @OldEAID


IF @MaxEAID IS NOT NULL
BEGIN 
	--Insert New Bounce Records
	INSERT ecn_activity.dbo.BlastActivityBounces(
		BlastID,
		EmailID,
		BounceTime,
		BounceCodeID,
		BounceMessage,
		EAID)
	Select 
		x.BlastID,
		x.CreatedEmailID,
		x.ActionDate,
		ISNULL(x.BounceCodeID,15),
		x.ActionNotes,
		x.EAID
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
			c.BounceCodeID,
			a.EAID
		FROM 
			ECN5_COMMUNICATOR.dbo.EmailActivityLog a WITH (NOLOCK)
			LEFT JOIN ECN_ACTIVITY.dbo.BounceCodes c WITH (NOLOCK)  ON a.ActionValue = c.BounceCode
			left join ECN5_COMMUNICATOR.dbo.Emails e  WITH (NOLOCK) on e.emailID = a.EmailID
			left join ECN5_COMMUNICATOR.dbo.EmailHistory h  WITH (NOLOCK) on h.OldEmailID = a.EmailID and h.Action = 'merge' 
		WHERE 
			a.ActionTypeCode IN ('bounce')
			AND EAID BETWEEN @MinEAID AND @MaxEAID
	)x
	where x.rownum = 1 and (x.email_emailid is not null OR x.history_newemailID IS NOT NULL)


	UPDATE
		ECN_Activity.dbo.ActivityLogIdSync
	SET
		MaxEAID  = @MaxEAID + 1
	WHERE
		TargetTable = 'bounce'
END
