CREATE proc [dbo].[MovedToActivity_sp_ReSend_Softbounce_Update]
(
	@blastID int
)
as
Begin
	INSERT INTO ECN_ACTIVITY..OldProcsInUse (ProcName, DateUsed) VALUES ('sp_ReSend_Softbounce_Update', GETDATE())
	set nocount on
	declare @tsoftbounce table (emailID int)

	insert into @tsoftbounce
	SELECT	distinct EmailID from EmailActivityLog 
	WHERE	blastID =  @blastID
	AND (ActionValue ='soft' or ActionValue = 'softbounce') 

select * from @tsoftbounce

	UPDATE	EmailActivityLog 
	SET		ActionTypeCode = 'bounce', 
			ActionValue = 'resend' 
	from 
			emailactivitylog eal join 
			@tsoftbounce t on eal.emailID = t.emailID
	WHERE 
			eal.BlastID = @blastID AND 
			(eal.ActionValue ='soft' or eal.ActionValue = 'softbounce') 

	INSERT INTO EmailActivityLog (EmailID, BlastID, ActionTypeCode, ActionDate, ActionValue ) 
	select emailID, @blastID, 'resend', getdate(), 'resend' from @tsoftbounce

End
