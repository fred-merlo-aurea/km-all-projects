CREATE proc [dbo].[MovedToActivity_spScicomAutoResponder]
as
BEGIN
	declare @BounceCodeID int
	select @BounceCodeID = BounceCodeID from ecn_Activity.dbo.BounceCodes where BounceCode = 'autoresponder'

	delete from ecn_Activity.dbo.BlastActivityBounces 
	where	BounceCodeID = @BounceCodeID and
			BlastID in 
			(
				select	BlastID
				from	[BLAST] b 
				where		
						b.CustomerID in	(select CustomerID  from [ECN5_ACCOUNTS].[DBO].[CUSTOMER]   where BaseChannelID = 58) and 
						TestBlast= 'n' 
			)
END
