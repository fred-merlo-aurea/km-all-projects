CREATE PROCEDURE [dbo].[job_SequenceDataMatching]
	@ProcessCode varchar(50)
AS	
	Update SF
		set SF.IGrp_No = S.IGRP_NO, SF.IGrp_Rank = 'M'
	from SubscriberFinal SF
		join PubSubscriptions PS on PS.SequenceID = SF.Sequence
		join Pubs P on P.PubCode = SF.PubCode and P.PubID = PS.PubID
		join Subscriptions S on S.SubscriptionID = PS.SubscriptionID
	where SF.ProcessCode = @ProcessCode and SF.Sequence > 0