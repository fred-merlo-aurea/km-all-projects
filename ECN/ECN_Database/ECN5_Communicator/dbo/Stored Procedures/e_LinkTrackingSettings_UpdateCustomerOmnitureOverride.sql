CREATE PROCEDURE [dbo].[e_LinkTrackingSettings_UpdateCustomerOmnitureOverride]
	@BaseChannelID int,
	@AllowOverride bit,
	@UserID int
AS
	if @AllowOverride = 0
	BEGIN
		update lts 
		set lts.XMLConfig = REPLACE(lts.XMLConfig,'<Override>True</Override>', '<Override>False</Override>' ), lts.UpdatedDate = GETDATE(), lts.UpdatedUserID = @UserID
		from LinkTrackingSettings lts 
		join ECN5_Accounts..Customer c on lts.CustomerID = c.CustomerID
		where c.BaseChannelID = @BaseChannelID and ISNULL(lts.IsDeleted,0) = 0
	END
	ELSE
	BEGIN
		update lts 
		set lts.XMLConfig = REPLACE(lts.XMLConfig,'<Override>False</Override>', '<Override>True</Override>' ), lts.UpdatedDate = GETDATE(), lts.UpdatedUserID = @UserID
		from LinkTrackingSettings lts 
		join ECN5_Accounts..Customer c on lts.CustomerID = c.CustomerID
		where c.BaseChannelID = @BaseChannelID and ISNULL(lts.IsDeleted,0) = 0
	END

