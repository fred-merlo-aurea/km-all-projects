CREATE PROCEDURE [dbo].[job_AccountNumberDataMatching]
	@ProcessCode varchar(50)
AS	
BEGIN
	
	SET NOCOUNT ON
	
	--Update all IGrp_No to default
	Update SF 
		set SF.IGrp_No = '00000000-0000-0000-0000-000000000000',
			SF.IGrp_Rank = 'M'
	from SubscriberFinal SF	
	where SF.ProcessCode = @ProcessCode
	
	
	--Update the IGrp_No on Account Number
	Update SF
		set SF.IGrp_No = S.IGRP_NO,
			SF.IGrp_Rank = 'M'
	from SubscriberFinal SF
		join PubSubscriptions PS on PS.AccountNumber = SF.AccountNumber
		join Pubs P on P.PubCode = SF.PubCode and P.PubID = PS.PubID
		join Subscriptions S on S.SubscriptionID = PS.SubscriptionID
	where SF.ProcessCode = @ProcessCode and (SF.AccountNumber <> '0' AND SF.AccountNumber <> '')
	
	
	--If one account number and no pubcode --double check the pub part
	Update SF
		set SF.IGrp_No = S.IGRP_NO,
			SF.IGrp_Rank = 'M'
	from SubscriberFinal SF
		join PubSubscriptions PS on PS.AccountNumber = SF.AccountNumber
		join Pubs P on P.PubCode = SF.PubCode and PS.PubID is null
		join Subscriptions S on S.SubscriptionID = PS.SubscriptionID
	where SF.ProcessCode = @ProcessCode and (SF.AccountNumber <> '0' AND SF.AccountNumber <> '') and SF.IGrp_No = '00000000-0000-0000-0000-000000000000'
	

	--Update records that update assume they are new
	Update SF 
		set SF.IGrp_No = NEWID(),
			SF.IGrp_Rank = 'M'
	from SubscriberFinal SF	
	where SF.ProcessCode = @ProcessCode and SF.IGrp_No = '00000000-0000-0000-0000-000000000000'

END