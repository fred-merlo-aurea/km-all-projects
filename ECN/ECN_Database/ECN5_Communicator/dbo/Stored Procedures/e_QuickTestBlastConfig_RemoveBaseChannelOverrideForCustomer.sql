CREATE PROCEDURE [dbo].[e_QuickTestBlastConfig_RemoveBaseChannelOverrideForCustomer]
	@BaseChannelID int
AS
BEGIN
	update QuickTestBlastConfig set CustomerDoesOverride = 0
	where CustomerID in 
	(select CustomerID from [ECN5_ACCOUNTS].[dbo].[Customer] where BaseChannelID= @BaseChannelID and IsDeleted=0)
END
