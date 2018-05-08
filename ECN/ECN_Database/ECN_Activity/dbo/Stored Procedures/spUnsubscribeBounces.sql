CREATE proc [dbo].[spUnsubscribeBounces] 
(
	@blastID  int, 
	@bounceType  varchar(25),
	@ISP varchar(100)
)
as
Begin

		set nocount on

		declare @groupID int,
				@dt datetime

		set @dt = getdate()
		select  @groupID = groupID from ecn5_communicator..[BLAST] with (nolock) where blastID = @blastID

		/* --------------- If ISP Exists ---------------------*/
		if (len(@ISP) > 0)
		Begin

			/* ---- update Email table with subscribetype to U for unsubscribed  ---- */
			UPDATE	ecn5_communicator..EmailGroups 
			SET		SubscribeTypeCode = 'U' 
			where 
					groupID = @groupID	and SubscribeTypeCode <> 'U' and 
					EmailID in 
						(
							SELECT 
								distinct	e.EmailID 
							FROM 
								ecn5_communicator..Emails e with (nolock) 
								join  BlastActivityBounces bab with (nolock) on e.EmailID = bab.EmailID 
								join BounceCodes bc on bab.BounceCodeID = bc.BounceCodeID		
							where	
								bab.blastID = @blastID and bc.BounceCode in (select items from ecn5_communicator..fn_Split(@bounceType, ',')) and
									right(emailaddress,len(@ISP)) = @ISP --e.emailaddress like '%' + @ISP
						)

			/* ---- insert a different row in the EmailActivityLog table with ActionTypeCode to 'Subscribe' and ActionValue to a 'U'
			-- so that there is a record of the bounce -> Unsubscribed  ---- */

			INSERT INTO  ecn5_communicator..EmailActivityLog (EmailID, BlastID, ActionTypeCode, ActionDate, ActionValue )
			SELECT distinct	
				e.EmailID, @blastID, 'subscribe', @dt, 'U'
			FROM 
				ecn5_communicator..Emails e with (nolock) 
				join  BlastActivityBounces bab with (nolock) on e.EmailID = bab.EmailID 	
				join BounceCodes bc on bab.BounceCodeID = bc.BounceCodeID		
			where	
				bab.blastID = @blastID AND bc.BounceCode in (select items from ecn5_communicator..fn_Split(@bounceType, ',')) and
					right(emailaddress,len(@ISP)) = @ISP 


			-- Update the EmailActivityLog table with ActionTypeCode to 'bounce' and ActionValue to a 'U' 
			--	so that the undelivered count(bounce) is maintained in the "Successful:"  on reports  ---- */
			UPDATE 	ecn5_communicator..EmailActivityLog 
			SET 	ActionValue = 'U' 
			WHERE 
					BlastID = @blastID and ActionTypeCode = 'bounce'  and 
					ActionValue in (select items from ecn5_communicator..fn_Split(@bounceType, ',')) and
					EmailID in 
						(
							SELECT 
								distinct	e.EmailID 
							FROM 
								ecn5_communicator..Emails e with (nolock) 
								join  BlastActivityBounces bab with (nolock) on e.EmailID = bab.EmailID 	
								join BounceCodes bc on bab.BounceCodeID = bc.BounceCodeID	
							where	
								bab.blastID = @blastID and bc.BounceCode in (select items from ecn5_communicator..fn_Split(@bounceType, ',')) and
									right(emailaddress,len(@ISP)) = @ISP 
						)
		end
		Else
		Begin
			/* ---- -- update Email table with subscribetype to U for unsubscribed---- */

			UPDATE	
				ecn5_communicator..EmailGroups 
			SET		
				SubscribeTypeCode = 'U' 
			where 
					groupID = @groupID	and SubscribeTypeCode <> 'U' and 
					EmailID in 
						(
							SELECT 
								distinct EmailID 
							FROM 
								BlastActivityBounces bab with (nolock)
								join BounceCodes bc on bab.BounceCodeID = bc.BounceCodeID
							where  
								blastID = @blastID AND bc.BounceCode in (select items from ecn5_communicator..fn_Split(@bounceType, ','))
						)

			/* ---- -- insert a different row in the EmailActivityLog table with ActionTypeCode to 'Subscribe' and ActionValue to a 'U'
			-- so that there is a record of the bounce -> Unsubscribed ---- */

			INSERT INTO  ecn5_communicator..EmailActivityLog (EmailID, BlastID, ActionTypeCode, ActionDate, ActionValue )
			SELECT	
				distinct EmailID, @blastID, 'subscribe', @dt, 'U'
			FROM	
				BlastActivityBounces bab with (nolock) 	
				join BounceCodes bc on bab.BounceCodeID = bc.BounceCodeID 
			where	
				blastID = @blastID and bc.BounceCode in (select items from ecn5_communicator..fn_Split(@bounceType, ','))

			/* ---- -- Update the EmailActivityLog table with ActionTypeCode to 'bounce' and ActionValue to a 'U' 
			--	so that the undelivered count(bounce) is maintained in the "Successful:"  on reports---- */

			UPDATE 	
				ecn5_communicator..EmailActivityLog 
			SET 	
				ActionValue = 'U' 
			WHERE 
				BlastID = @blastID and ActionTypeCode = 'bounce'  and ActionValue in (select items from ecn5_communicator..fn_Split(@bounceType, ','))

		End

		set nocount off
End
