CREATE proc [dbo].[sp_Mastercodesheet_Delete](
@MasterID int
)
as
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

End