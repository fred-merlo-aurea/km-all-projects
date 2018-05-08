CREATE  PROC dbo.e_SFSettings_RemoveBaseChannelOverrideForCustomer 
(
	@BaseChannelID int 
)
AS 
BEGIN
	update sfsettings set CustomerDoesOverride = 0
	where CustomerID in 
	(select CustomerID from Customer where BaseChannelID= @BaseChannelID and IsDeleted=0)
END