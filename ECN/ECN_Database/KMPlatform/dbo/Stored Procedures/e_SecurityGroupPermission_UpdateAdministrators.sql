CREATE PROCEDURE [dbo].[e_SecurityGroupPermission_UpdateAdministrators]
	@ClientID int,
	@ClientGroupID int,
	@UserID int
AS
	
	--Customer Admin
	declare @CAdminID int
	select @CAdminID = SecurityGroupID from SecurityGroup sg with(nolock) where sg.ClientID = @ClientID and sg.AdministrativeLevel = 'Administrator'
	update sgp
	set sgp.IsActive = csfm.IsEnabled, DateUpdated = GETDATE(), UpdatedByUserID = @UserID
	from ClientServiceFeatureMap csfm with(nolock)
	join ServiceFeatureAccessMap sfam with(nolock) on csfm.ServiceFeatureID = sfam.ServiceFeatureID
	join SecurityGroupPermission sgp with(nolock) on sgp.ServiceFeatureAccessMapID = sfam.ServiceFeatureAccessMapID
	where csfm.ClientID = @ClientID and sgp.SecurityGroupID = @CAdminID

	insert into SecurityGroupPermission(SecurityGroupID, ServiceFeatureAccessMapID, IsActive, DateCreated, CreatedByUserID)
	select @CAdminID, sfam.ServiceFeatureAccessMapID, csfm.IsEnabled, GETDATE(), @UserID
	from ClientServiceFeatureMap csfm with(nolock)
	join ServiceFeatureAccessMap sfam with(nolock) on csfm.ServiceFeatureID = sfam.ServiceFeatureID
	left outer join SecurityGroupPermission sgp with(nolock) on sgp.ServiceFeatureAccessMapID = sfam.ServiceFeatureAccessMapID and sgp.SecurityGroupID = @CAdminID
	where csfm.ClientID = @ClientID and sgp.SecurityGroupPermissionID is null and csfm.IsEnabled = 1

	--BaseChannelAdmin
	declare @BCAdminID int
	select @BCAdminID = SecurityGroupID from SecurityGroup sg with(nolock) where sg.ClientGroupID = @ClientGroupID and sg.AdministrativeLevel = 'ChannelAdministrator'

	update sgp
	set sgp.IsActive = csfm.IsEnabled, DateUpdated = GETDATE(), UpdatedByUserID = @UserID
	from ClientServiceFeatureMap csfm with(nolock)
	join ServiceFeatureAccessMap sfam with(nolock) on csfm.ServiceFeatureID = sfam.ServiceFeatureID
	join SecurityGroupPermission sgp with(nolock) on sgp.ServiceFeatureAccessMapID = sfam.ServiceFeatureAccessMapID
	where csfm.ClientID = @ClientID and sgp.SecurityGroupID = @BCAdminID and csfm.IsEnabled = 1

	insert into SecurityGroupPermission(SecurityGroupID, ServiceFeatureAccessMapID, IsActive, DateCreated, CreatedByUserID)
	select @BCAdminID, sfam.ServiceFeatureAccessMapID, csfm.IsEnabled, GETDATE(), @UserID
	from ClientServiceFeatureMap csfm with(nolock)
	join ServiceFeatureAccessMap sfam with(nolock) on csfm.ServiceFeatureID = sfam.ServiceFeatureID
	left outer join SecurityGroupPermission sgp with(nolock) on sgp.ServiceFeatureAccessMapID = sfam.ServiceFeatureAccessMapID and sgp.SecurityGroupID = @BCAdminID
	where csfm.ClientID = @ClientID and sgp.SecurityGroupPermissionID is null and csfm.IsEnabled = 1