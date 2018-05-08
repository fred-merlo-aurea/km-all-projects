CREATE PROCEDURE [dbo].[e_MasterCodeSheet_Delete_MasterID]
	@MasterID int
AS	
BEGIN

	SET NOCOUNT ON

	delete 
	from SubscriptionDetails 
	where MasterID = @MasterID

	delete 
	from SubscriberMasterValues 
	where MasterGroupID in (SELECT mcs.MasterGroupID 
							FROM SubscriberMasterValues smv
								JOIN Mastercodesheet mcs ON smv.MasterGroupID = mcs.MasterGroupID
							WHERE mcs.MasterID = @MasterID)

	delete 
	from CodeSheet_Mastercodesheet_Bridge 
	where MasterID = @MasterID

	delete 
	from Mastercodesheet 
	where MasterID = @MasterID

END