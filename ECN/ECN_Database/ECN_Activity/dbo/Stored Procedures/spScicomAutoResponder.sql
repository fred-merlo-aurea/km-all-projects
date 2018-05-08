create proc [dbo].[spScicomAutoResponder]
as
BEGIN
	declare @BounceCodeID int
	select @BounceCodeID = BounceCodeID from ecn_Activity.dbo.BounceCodes where BounceCode = 'autoresponder'

	delete from ecn_Activity.dbo.BlastActivityBounces 
	where	BounceCodeID = @BounceCodeID and
			BlastID in 
			(
				select	BlastID
				from	ecn5_communicator.dbo.[BLAST] b join ecn5_accounts..[CUSTOMER] c on b.CustomerID = c.CustomerID 
				where		
						BaseChannelID = 58 and TestBlast= 'n' 
			)
END
