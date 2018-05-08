CREATE proc [dbo].[sp_Pubs_Delete]
@PubID int
as
BEGIN

	SET NOCOUNT ON

	delete 
	from CodeSheet_Mastercodesheet_Bridge 
	where CodeSheetID in (select c.CodeSheetID from CodeSheet c where PubID = @PubID)

	delete 
	from PubSubscriptionDetail 
	where CodeSheetID in (select c.CodeSheetID from CodeSheet c where PubID = @PubID)

	delete 
	from SubscriberClickActivity  
	where PubSubscriptionID in ( select ps.PubSubscriptionID from PubSubscriptions ps where PubID = @PubID)

	delete 
	from SubscriberOpenActivity  
	where PubSubscriptionID in ( select ps.PubSubscriptionID from PubSubscriptions ps  where PubID = @PubID)

	delete 
	from PubSubscriptionDetail  
	where PubSubscriptionID in ( select ps.PubSubscriptionID from PubSubscriptions ps  where PubID = @PubID)

	delete 
	from SubscriberTopicActivity 
	where PubSubscriptionID in ( select ps.PubSubscriptionID from PubSubscriptions ps  where PubID = @PubID)

	delete 
	from PubSubscriptionsExtension 
	where  PubSubscriptionID in ( select ps.PubSubscriptionID from PubSubscriptions ps  where PubID = @PubID)

	delete 
	from PubSubscriptionsExtensionmapper 
	where PubID = @PubID

	delete 
	from PubSubscriptions 
	where PubID = @PubID

	delete 
	from PubGroups 
	where pubID = @PubID

	delete 
	from CodeSheet 
	where CodeSheetID in (select c.CodeSheetID from CodeSheet c  where PubID = @PubID)

	delete 
	from ResponseGroups 
	where PubID = @PubID

	delete 
	from Pubs 
	where PubID =  @PubID

End