CREATE PROC [dbo].[usp_SyncActivityResend]
AS 

SET NOCOUNT OFF 
-- exec usp_SyncActivityResend

	
DECLARE 
	@MinEAID INT,
	@MaxEAID INT,
	@OldEAID INT

SELECT 
	@MinEAID = MaxEAID, @OldEAID = oldminEAID
FROM 
	ECN_Activity.dbo.ActivityLogIdSync
WHERE
	TargetTable = 'Resend'

if (@OldEAID is null)
	SELECT 
		@MaxEAID =max(eaid) 
	FROM 
		ECN5_COMMUNICATOR.dbo.EmailActivityLog a WITH(NOLOCK)
	WHERE 
		a.ActionTypeCode = 'Resend'
		AND EAID >= @MinEAID
else
	SELECT 
		@MaxEAID =max(eaid) 
	FROM 
		ECN5_COMMUNICATOR.dbo.EmailActivityLog a WITH(NOLOCK)
	WHERE 
		a.ActionTypeCode = 'Resend'
		AND EAID >= @MinEAID and EAID <  @OldEAID


IF @MaxEAID IS NOT NULL
BEGIN 
	
	--Insert New Resend Records
	INSERT ecn_activity.dbo.BlastActivityResends(
		BlastID,
		EmailID,
		ResendTime,
		EAID)
	Select 
		x.BlastID,
		x.CreatedEmailID,
		x.ActionDate,
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
			a.EAID
		FROM 
			ECN5_COMMUNICATOR.dbo.EmailActivityLog a WITH (NOLOCK)
			left join ECN5_COMMUNICATOR.dbo.Emails e  WITH (NOLOCK) on e.emailID = a.EmailID
			left join ECN5_COMMUNICATOR.dbo.EmailHistory h  WITH (NOLOCK) on h.OldEmailID = a.EmailID and h.Action = 'merge' 
		WHERE 
			a.ActionTypeCode IN ('resend')
			AND EAID BETWEEN @MinEAID AND @MaxEAID
	)x
	where x.rownum = 1 and (x.email_emailid is not null OR x.history_newemailID IS NOT NULL)

	UPDATE
		ECN_Activity.dbo.ActivityLogIdSync
	SET
		MaxEAID  = @MaxEAID + 1
	WHERE
		TargetTable = 'resend'
END
