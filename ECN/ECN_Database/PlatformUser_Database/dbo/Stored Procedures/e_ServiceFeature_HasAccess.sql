CREATE PROCEDURE [dbo].[e_ServiceFeature_HasAccess]
@UserID int,
@ClientID int,
@ServiceCode varchar(20),
@SFCode  varchar(50),
@AccessCode  varchar(20)
AS
Begin

	declare @SecuritygroupID int,
			@ClientGroupID int
	
	set @securityGroupID = 0
	
	select	
			@securityGroupID = ucsg.SecurityGroupID,
			@clientgroupID = ISNULL(clientgroupID,0)
	from 
			UserClientSecurityGroupMap ucsg  with (NOLOCK) join
			Client c with (NOLOCK) on c.ClientID = ucsg.ClientID join
			ClientGroupClientMap cgcm  with (NOLOCK)  on cgcm.ClientID = c.ClientID
	where
			ucsg.UserID = @UserID and
			ucsg.ClientID = @ClientID and 
			ucsg.IsActive = 1 and
			c.IsActive = 1
	
			
	if @securityGroupID = 0
	Begin
		select 0
	End
	Else
	Begin
		if	exists (select top 1 1 from ClientGroupServiceMap cgsm with (NOLOCK) join [Service] s  with (NOLOCK) on cgsm.ServiceID = s.ServiceID where ClientGroupID = @ClientGroupID and s.ServiceCode = @ServiceCode and cgsm.IsEnabled = 1 and s.IsEnabled = 1) and 
			exists (select top 1 1 from ClientGroupServiceFeatureMap cgsfm with (NOLOCK) join [ServiceFeature] sf  with (NOLOCK) on cgsfm.ServiceFeatureID = sf.ServiceFeatureID where ClientGroupID = @ClientGroupID and sf.SFCode = @SFCode and cgsfm.IsEnabled = 1 and sf.IsEnabled = 1) and
			exists (select top 1 1 from ClientServiceMap csm with (NOLOCK) join [Service] s  with (NOLOCK) on csm.ServiceID = s.ServiceID where ClientID = @ClientID and s.ServiceCode = @ServiceCode and csm.IsEnabled = 1 and s.IsEnabled = 1) and 
			exists (select top 1 1 from ClientServiceFeatureMap csfm with (NOLOCK) join [ServiceFeature] sf  with (NOLOCK) on csfm.ServiceFeatureID = sf.ServiceFeatureID where ClientID = @ClientID and sf.SFCode = @SFCode and csfm.IsEnabled = 1 and sf.IsEnabled = 1) 
		Begin
			select	distinct 1
			 from
				SecurityGroupPermission sgp with (NOLOCK) join
				ServiceFeatureAccessMap sfam with (NOLOCK) on sfam.ServiceFeatureAccessMapID = sgp.ServiceFeatureAccessMapID join
				ServiceFeature sf with (NOLOCK) on sf.ServiceFeatureID = sfam.ServiceFeatureID join
				[Service] s with (NOLOCK) on s.serviceID = sf.ServiceID  join 
				Access a with (NOLOCK) on sfam.AccessID = a.AccessID

			where
				sgp.SecurityGroupID = @SecurityGroupID and
				s.ServiceCode = @ServiceCode and
				sf.SFCode = @SFCode and
				a.AccessCode = @AccessCode and
				sgp.IsActive = 1 and
				sfam.IsEnabled = 1 and
				sf.IsEnabled = 1 and
				s.IsEnabled = 1 
		End	
		else
			select 0			
	End

End
