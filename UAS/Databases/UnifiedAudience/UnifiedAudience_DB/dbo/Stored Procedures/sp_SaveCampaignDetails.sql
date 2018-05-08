CREATE proc [dbo].[sp_SaveCampaignDetails]
(  
 @CampaignFilterID int,  
 @xmlSubscriber text 
)  
as  
BEGIN

	SET NOCOUNT ON	

	declare @docHandle int

	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xmlSubscriber  
  
	insert into CampaignFilterDetails(CampaignFilterID, SubscriptionID)  
	SELECT  @CampaignFilterID, x.SubscriptionID
	FROM OPENXML(@docHandle, N'/XML/sID')	  
	WITH (SubscriptionID INT '.') x 
		left outer join CampaignFilterDetails cfd on x.subscriptionID = cfd.SubscriptionID and cfd.CampaignFilterID = @CampaignFilterID
	where cfd.SubscriptionID is null
	
	
	EXEC sp_xml_removedocument @docHandle    

END