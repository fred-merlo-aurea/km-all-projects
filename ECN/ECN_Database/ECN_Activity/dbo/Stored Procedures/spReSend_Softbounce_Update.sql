CREATE proc [dbo].[spReSend_Softbounce_Update]
(
	@blastID int
)
as
Begin
	
	set nocount on
	declare @ResendBounceCodeID int
	
	select @ResendBounceCodeID = bouncecodeID from ecn_Activity.dbo.BounceCodes where BounceCode = 'resend'
	
	declare @tsoftbounce table (BounceID int, emailID int)

	insert into @tsoftbounce
	SELECT	BounceID, EmailID
	from ecn_Activity.dbo.BlastActivityBounces eab join ecn_Activity.dbo.BounceCodes bc on eab.BounceCodeID = bc.BounceCodeID
	WHERE	blastID =  @blastID
	AND (bc.BounceCode ='soft' or bc.BounceCode = 'softbounce') 

	select distinct emailID from @tsoftbounce

	UPDATE	ecn_Activity.dbo.BlastActivityBounces 
	SET		BounceCodeID = @ResendBounceCodeID
	from 
			ecn_Activity.dbo.BlastActivityBounces eab join 
			@tsoftbounce t on eab.BounceID = t.BounceID

	INSERT INTO ecn5_communicator.dbo.EmailActivityLog (EmailID, BlastID, ActionTypeCode, ActionDate, ActionValue ) 
	select distinct emailID, @blastID, 'resend', getdate(), 'resend' from @tsoftbounce

End
