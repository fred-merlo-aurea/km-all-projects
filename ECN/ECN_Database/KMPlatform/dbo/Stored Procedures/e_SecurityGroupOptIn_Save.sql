
CREATE PROCEDURE [dbo].[e_SecurityGroupOptIn_Save] 
	@UserID int,
	@ClientID int,
	@ClientGroupID int,
	@SecurityGroupID int,
	@SendTime datetime,
	@SetID uniqueidentifier,
	@HasAccepted bit,
	@UserClientSecurityGroupMapID int,
	@CreatedByUserID int,
	@IsDeleted bit,
	@SecurityGroupOptInID int = null
AS
BEGIN
if @SecurityGroupOptInID is null
BEGIN
	INSERT INTO SecurityGroupOptIn(UserID, SecurityGroupID, ClientID, ClientGroupID, SendTime, HasAccepted, DateAccepted, SetID, CreatedByUserID, CreatedDate, IsDeleted)
	VALUES(@UserID, @SecurityGroupID, @ClientID, @ClientGroupID, @SendTime, @HasAccepted, null, @SetID, @CreatedByUserID, GETDATE(), @IsDeleted)
	SELECT @@IDENTITY;
END
ELSE 
BEGIN
	UPDATE SecurityGroupOptIn
	set UserID = @UserID, SecurityGroupID = @SecurityGroupID, ClientID = @ClientID, ClientGroupID = @ClientGroupID, SendTime = @SendTime, HasAccepted = @HasAccepted, SetID = @SetID,IsDeleted = @IsDeleted
	WHERE SecurityGroupOptInID = @SecurityGroupOptInID
	SELECT @SecurityGroupOptInID
END
END