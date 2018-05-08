CREATE PROCEDURE [dbo].[e_Group_Exists_SubMgmt]
	@GroupID int,
	@CustomerID int
AS
	DECLARE @SubMGMTGroupID int = 0
	DECLARE @BCID int
	Select @BCID = BaseChannelID from ECN5_Accounts..Customer c with(nolock) where c.CustomerID = @CustomerID

	Select @SubMGMTGroupID = SubscriptionManagementGroupID 
	FROM ECN5_Accounts..SubscriptionManagementGroup smg with(nolock) 
	join ECN5_Accounts..SubscriptionManagement sm with(nolock) on smg.SubscriptionManagementPageID = sm.SubscriptionManagementID
	where smg.GroupID = @GroupID and smg.IsDeleted = 0 and sm.BaseChannelID = @BCID

	if @SubMGMTGroupID > 0
		SELECT 1
	else 
		SELECT 0
