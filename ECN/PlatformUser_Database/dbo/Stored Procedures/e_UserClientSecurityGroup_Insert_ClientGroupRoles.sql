CREATE PROCEDURE [dbo].[e_UserClientSecurityGroup_Insert_ClientGroupRoles]
	@ClientID int,
	@UserID int
AS
	declare @ClientGroupID int
	Select @ClientGroupID = ClientGroupID 
	FROM ClientGroupClientMap cgcm with(nolock) 
	where cgcm.ClientID = @ClientID

	INSERT INTO UserClientSecurityGroupMap(UserID, ClientID, SecurityGroupID, CreatedByUserID, DateCreated, IsActive, InactiveReason)
	SELECT ucsgm.UserID, @ClientID, sg.SecurityGroupID, ucsgm.CreatedByUserID, ucsgm.DateCreated, ucsgm.IsActive ,ucsgm.InactiveReason
		from SecurityGroup sg with(nolock)
		join UserClientSecurityGroupMap ucsgm with(nolocK) on sg.SecurityGroupID = ucsgm.SecurityGroupID
	where sg.ClientGroupID = @ClientGroupID and sg.IsActive = 1 and ucsgm.ClientID != @ClientID

	INSERT INTO SecurityGroupOptIn(SecurityGroupID, UserID, SetID, ClientID, ClientGroupID, SendTime)
	Select sgoi.SecurityGroupID, sgoi.UserID, sgoi.SetID,sgoi.ClientID,sgoi.ClientGroupID,sgoi.SendTime
	FROM SecurityGroupOptIn sgoi with(nolocK)
	join UserClientSecurityGroupMap ucsgm with(nolock) on sgoi.UserID = ucsgm.UserID and sgoi.SecurityGroupID = ucsgm.SecurityGroupID
	where sgoi.HasAccepted = 0 and ISNULL(sgoi.IsDeleted, 0) = 0 and sgoi.ClientGroupID = @ClientGroupID