CREATE PROCEDURE [dbo].[e_QuickTestBlastConfig_Save]
	@QTBCID int,
	@BaseChannelID int,
	@CustomerID int,
	@CustomerCanOverride bit = NULL,
	@CustomerDoesOverride bit = NULL,
	@BaseChannelDoesOverride bit = NULL,
	@AllowAdhocEmails bit = NULL,
	@AutoCreateGroup bit = NULL,
	@AutoArchiveGroup bit = NULL,
	@UserID int
AS
if(@QTBCID>0)
BEGIN
	update QuickTestBlastConfig set 
	BaseChannelID=@BaseChannelID,
	CustomerID=@CustomerID,
	CustomerCanOverride=@CustomerCanOverride,
	CustomerDoesOverride= @CustomerDoesOverride,
	AllowAdhocEmails= @AllowAdhocEmails,
	AutoCreateGroup= @AutoCreateGroup,
	AutoArchiveGroup= @AutoArchiveGroup,
	UpdatedUserID= @UserID,
	UpdatedDate= GETDATE(),
	IsDefault=0,
	BaseChannelDoesOverride=@BaseChannelDoesOverride
	where QTBCID=@QTBCID
	
END
ELSE
BEGIN
	insert into QuickTestBlastConfig(BaseChannelID, CustomerID, CustomerCanOverride, CustomerDoesOverride,
	AllowAdhocEmails, AutoCreateGroup , AutoArchiveGroup, CreatedUserID, CreatedDate, IsDefault,BaseChannelDoesOverride)
	values (@BaseChannelID, @CustomerID, @CustomerCanOverride, @CustomerDoesOverride, 
	@AllowAdhocEmails, @AutoCreateGroup ,@AutoArchiveGroup,  @UserID, GETDATE(), 0, @BaseChannelDoesOverride)	
	SELECT @QTBCID = @@IDENTITY
END

SELECT @QTBCID
