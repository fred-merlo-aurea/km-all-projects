CREATE  PROC dbo.e_LandingPageAssign_RemoveBaseChannelOverrideForCustomer 
(
	@BaseChannelID int 
	)
AS 
BEGIN
	update LandingPageAssign set CustomerDoesOverride = 0
	where CustomerID in 
	(select CustomerID from Customer where BaseChannelID= @BaseChannelID and IsDeleted=0)
END