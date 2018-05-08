CREATE proc [dbo].[MovedToActivity_sp_UnsubscribeBounces] 
(
	@blastID  int, 
	@bounceType  varchar(25),
	@ISP varchar(100)
)
as
Begin
	INSERT INTO ECN_ACTIVITY..OldProcsInUse (ProcName, DateUsed) VALUES ('sp_UnsubscribeBounces', GETDATE())
		set nocount on

		declare @groupID int,
				@dt datetime

		set @dt = getdate()
		select  @groupID = groupID from [BLAST] where blastID = @blastID

		/* --------------- If ISP Exists ---------------------*/
		if (len(@ISP) > 0)
		Begin

			/* ---- update Email table with subscribetype to U for unsubscribed  ---- */
			UPDATE	EmailGroups 
			SET		SubscribeTypeCode = 'U' 
			where 
					groupID = @groupID	and SubscribeTypeCode <> 'U' and 
					EmailID in 
						(
							SELECT distinct	e.EmailID FROM Emails e join  EmailActivityLog ea on e.EmailID = ea.EmailID 		
							where	ea.blastID = @blastID AND ActionTypeCode = 'bounce' and ea.ActionValue in (select items from dbo.fn_Split(@bounceType, ',')) and
									right(emailaddress,len(@ISP)) = @ISP --e.emailaddress like '%' + @ISP
						)

			/* ---- insert a different row in the EmailActivityLog table with ActionTypeCode to 'Subscribe' and ActionValue to a 'U'
			-- so that there is a record of the bounce -> Unsubscribed  ---- */

			INSERT INTO  EmailActivityLog (EmailID, BlastID, ActionTypeCode, ActionDate, ActionValue )
			SELECT distinct	e.EmailID, @blastID, 'subscribe', @dt, 'U'
			FROM Emails e join  EmailActivityLog ea on e.EmailID = ea.EmailID 		
			where	ea.blastID = @blastID AND ActionTypeCode = 'bounce' and ea.ActionValue in (select items from dbo.fn_Split(@bounceType, ',')) and
					right(emailaddress,len(@ISP)) = @ISP 


			-- Update the EmailActivityLog table with ActionTypeCode to 'bounce' and ActionValue to a 'U' 
			--	so that the undelivered count(bounce) is maintained in the "Successful:"  on reports  ---- */
			UPDATE 	EmailActivityLog 
			SET 	ActionValue = 'U' 
			WHERE 
					BlastID = @blastID and ActionTypeCode = 'bounce'  and 
					ActionValue in (select items from dbo.fn_Split(@bounceType, ',')) and
					EmailID in 
						(
							SELECT distinct	e.EmailID FROM Emails e join  EmailActivityLog ea on e.EmailID = ea.EmailID 		
							where	ea.blastID = @blastID AND ActionTypeCode = 'bounce' and ea.ActionValue in (select items from dbo.fn_Split(@bounceType, ',')) and
									right(emailaddress,len(@ISP)) = @ISP 
						)
		end
		Else
		Begin
			/* ---- -- update Email table with subscribetype to U for unsubscribed---- */

			UPDATE	EmailGroups 
			SET		SubscribeTypeCode = 'U' 
			where 
					groupID = @groupID	and SubscribeTypeCode <> 'U' and 
					EmailID in 
						(
							SELECT distinct EmailID FROM EmailActivityLog
							where  blastID = @blastID AND ActionTypeCode = 'bounce' and ActionValue in (select items from dbo.fn_Split(@bounceType, ','))
						)

			/* ---- -- insert a different row in the EmailActivityLog table with ActionTypeCode to 'Subscribe' and ActionValue to a 'U'
			-- so that there is a record of the bounce -> Unsubscribed ---- */

			INSERT INTO  EmailActivityLog (EmailID, BlastID, ActionTypeCode, ActionDate, ActionValue )
			SELECT	distinct EmailID, @blastID, 'subscribe', @dt, 'U'
			FROM	EmailActivityLog 
			where	blastID = @blastID AND ActionTypeCode = 'bounce' and ActionValue in (select items from dbo.fn_Split(@bounceType, ','))

			/* ---- -- Update the EmailActivityLog table with ActionTypeCode to 'bounce' and ActionValue to a 'U' 
			--	so that the undelivered count(bounce) is maintained in the "Successful:"  on reports---- */

			UPDATE 	EmailActivityLog 
			SET 	ActionValue = 'U' 
			WHERE 
					BlastID = @blastID and ActionTypeCode = 'bounce'  and ActionValue in (select items from dbo.fn_Split(@bounceType, ','))

		End

		set nocount off
End
