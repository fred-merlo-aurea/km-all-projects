CREATE PROCEDURE [dbo].[e_SubscriberFinal_SelectForFieldUpdate]
	@ProcessCode varchar(50)
AS
BEGIN

	SET NOCOUNT ON
	
	Select SF.*
	from SubscriberFinal SF
		left join Subscriptions S on S.IGrp_No = SF.IGrp_No
		left join Pubs P on P.PubCode = SF.PubCode
		left join PubSubscriptions PS on PS.SubscriptionID = S.SubscriptionID and PS.PubID = P.PubID	
	where SF.ProcessCode = @ProcessCode AND SF.Ignore = 'true' 
	--AND SF.SubscriberFinalID not in 
	--(
	--	Select SF2.SubscriberFinalID		
	--	from SubscriberFinal SF2
	--	left join Subscriptions S2 on S2.IGrp_No = SF2.IGrp_No
	--	left join Pubs P2 on P2.PubCode = SF2.PubCode
	--	left join PubSubscriptions PS2 on PS2.SubscriptionID = S2.SubscriptionID and PS2.PubID = P2.PubID	
	--	where SF2.ProcessCode = @ProcessCode and PS2.PubSubscriptionID is not null
	--)

END