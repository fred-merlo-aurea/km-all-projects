CREATE PROCEDURE [dbo].[job_FieldUpdateDataMatching]
	@ProcessCode varchar(50)

AS	
BEGIN   

	SET NOCOUNT ON 

	Update SF
		set SF.IGrp_No = S.IGRP_NO,
			SF.IGrp_Rank = 'M'
	from SubscriberFinal SF
		join PubSubscriptions PS on SF.Sequence = PS.SequenceID
		join Pubs P on SF.PubCode = P.PubCode and PS.PubID = P.PubID
		join Subscriptions S on PS.SubscriptionID = S.SubscriptionID
	where SF.ProcessCode = @ProcessCode and SF.Sequence > 0

END