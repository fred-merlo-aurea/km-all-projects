CREATE PROCEDURE [dbo].[e_SFSettings_Save]   
@SFSettingsID int,
@BaseChannelID int = NULL,
@CustomerID int = NULL,
@CustomerCanOverride bit = NULL,
@CustomerDoesOverride bit = NULL,
@SandboxMode bit = NULL,
@PushChannelMasterSuppression bit = NULL,
@RefreshToken varchar(2000) = NULL,
@ConsumerKey varchar(2000) = NULL,
@ConsumerSecret varchar(2000) = NULL,
@UserID int

AS
if(@SFSettingsID>0)
BEGIN
	update SFSettings set 
	BaseChannelID=@BaseChannelID,
	CustomerID=@CustomerID,
	CustomerCanOverride=@CustomerCanOverride,
	CustomerDoesOverride= @CustomerDoesOverride,
	RefreshToken= @RefreshToken,
	UpdatedUserID= @UserID,
	UpdatedDate= GETDATE(),
	ConsumerKey=@ConsumerKey,
	ConsumerSecret=@ConsumerSecret,
	PushChannelMasterSuppression=@PushChannelMasterSuppression,
	SandboxMode=@SandboxMode
	where SFSettingsID=@SFSettingsID
	
END
ELSE
BEGIN
	insert into SFSettings( BaseChannelID, CustomerID, CustomerCanOverride, CustomerDoesOverride,
	RefreshToken, CreatedUserID, CreatedDate,ConsumerKey,ConsumerSecret, PushChannelMasterSuppression,SandboxMode )
	values ( @BaseChannelID, @CustomerID, @CustomerCanOverride, @CustomerDoesOverride, 
	@RefreshToken, @UserID, GETDATE(),@ConsumerKey,@ConsumerSecret,@PushChannelMasterSuppression,@SandboxMode)	
	SELECT @SFSettingsID = @@IDENTITY
END

SELECT @SFSettingsID