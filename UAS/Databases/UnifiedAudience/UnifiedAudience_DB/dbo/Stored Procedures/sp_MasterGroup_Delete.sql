CREATE proc [dbo].[sp_MasterGroup_Delete]
@MasterGroupID int
as
BEGIN

	SET NOCOUNT ON

	delete 
	from SubscriptionDetails 
	where MasterID in (select mcs.MasterID from Mastercodesheet mcs where MasterGroupID = @MasterGroupID)

	delete 
	from SubscriberMasterValues 
	where MasterGroupID = @MasterGroupID

	delete 
	from CodeSheet_Mastercodesheet_Bridge 
	where MasterID in (select mcs.MasterID from Mastercodesheet mcs where MasterGroupID = @MasterGroupID)

	delete 
	from Mastercodesheet 
	where MasterGroupID = @MasterGroupID

	delete 
	from DomainFieldMasterGroup 
	where MasterGroupID = @MasterGroupID

	delete from MasterGroups 
	where MasterGroupID = @MasterGroupID

END