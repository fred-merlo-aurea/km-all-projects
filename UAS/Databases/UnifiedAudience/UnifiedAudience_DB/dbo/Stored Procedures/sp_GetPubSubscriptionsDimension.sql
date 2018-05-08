CREATE PROCEDURE [dbo].[sp_GetPubSubscriptionsDimension]
	@SubscriptionID int,
	@PubID int
AS
BEGIN

	select rg.ResponseGroupID,
		rg.ResponseGroupName,
		ResponseDesc + ' ' + '(' + ResponseValue + ')' as ResponseDesc
	from PubSubscriptionDetail psd with(nolock) 
		join CodeSheet cs with(nolock) on cs.CodeSheetID =  psd.CodeSheetID 
		join ResponseGroups rg on cs.ResponseGroupID = rg.ResponseGroupID 
		join PubSubscriptions ps  with(nolock) on ps.PubSubscriptionID = psd.PubSubscriptionID
	where ps.PubID = @PubID and ps.SubscriptionID = @SubscriptionID

END