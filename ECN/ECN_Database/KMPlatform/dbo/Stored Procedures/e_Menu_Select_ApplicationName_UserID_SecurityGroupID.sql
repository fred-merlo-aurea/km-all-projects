CREATE PROCEDURE [dbo].[e_Menu_Select_ApplicationName_UserID_SecurityGroupID]  
@ApplicationName varchar(50),  
@UserID int,  
@SecurityGroupID int,  
@IsActive bit,  
@HasAccess bit,  
@IsServiceFeature bit = 0,
@clientID int =0  
AS  
--SecurityGroupID < 0 means its a sys admin  
  --SecurityGroupID < 0 means its a sys admin  
  declare @ClientGroupID int
	select @ClientGroupID = ClientGroupID from ClientGroupClientMap c with(nolock) where c.clientID = @clientID

 if(@SecurityGroupID > 0)  
BEGIN  
	declare @AdminLevel varchar(50)
	select @AdminLevel = ISNULL(sg.AdministrativeLevel, '')
	from SecurityGroup sg with(nolock) 
	where sg.SecurityGroupID = @SecurityGroupID
	
	declare @IsSysAdmin bit = 0
	select @IsSysAdmin = IsPlatformAdministrator
	from [user] u where u.UseriD = @UserID

	SELECT DISTINCT  sf.ServiceFeatureID,sf.ServiceID
	INTO #tempUserService
	FROM ServiceFeature sf WITH(NOLOCK)
	JOIN ServiceFeatureAccessMap sfam WITH(NOLOCK) ON sf.ServiceFeatureID = sfam.ServiceFeatureID
	JOIN SecurityGroupPermission sgp WITH(NOLOCK) ON sfam.ServiceFeatureAccessMapID = sgp.ServiceFeatureAccessMapID
	JOIN ClientServiceFeatureMap csf with(nolock) on sf.ServiceFeatureID = csf.ServiceFeatureID and csf.ClientID = @clientID	
	WHERE sgp.SecurityGroupID = @SecurityGroupID AND sgp.IsActive=1 and sfam.IsEnabled = 1 and csf.IsEnabled = 1

	
	
	select x.* 
	INTO #MenuItemsList
	FROM(
		select m.*
		from Menu m with(nolock) 
		join #tempUserService tus on m.ServiceFeatureID = tus.ServiceFeatureID
		join Application a with(nolock) on m.ApplicationID = a.ApplicationID
		join ClientServiceFeatureMap csfm with(nolock) on m.ServiceFeatureID = csfm.ServiceFeatureID
		where a.ApplicationName =@ApplicationName
		 and m.IsActive = 1   
		 and a.IsActive = 1  
		 and csfm.IsEnabled = 1
		 and m.ServiceFeatureID > 0
		 and ISNULL(m.IsClientGroupService,0) = 0		 
		 and ISNULL(m.IsSysAdmin,0) = 0
		 and ISNULL(m.IsChannelAdmin,0) = 0
		 and ISNULL(m.IsCustomerAdmin, 0) = 0
	UNION
		select m.*
		from Menu m with(nolock) 
		--join #tempUserService tus on m.ServiceFeatureID = tus.ServiceFeatureID
		join Application a with(nolock) on m.ApplicationID = a.ApplicationID
		join ClientGroupServiceMap csfm with(nolock) on m.ServiceID = csfm.ServiceID and csfm.ClientGroupID = @ClientGroupID
		where a.ApplicationName =@ApplicationName
		 and m.IsActive = 1   
		 and a.IsActive = 1  
		 and csfm.IsEnabled = 1		
		 and ISNULL(m.ServiceID,0) > 0 
		 and ISNULL(m.IsClientGroupService,0) = 1
		 		 and ISNULL(m.IsSysAdmin,0) = 0
		 and ISNULL(m.IsChannelAdmin,0) = 0
		 and ISNULL(m.IsCustomerAdmin, 0) = 0
	UNION
		select m.*
		from Menu m with(nolock) 
		join #tempUserService tus on m.ServiceID = tus.ServiceID
		join Application a with(nolock) on m.ApplicationID = a.ApplicationID
		join ClientServiceMap csfm with(nolock) on m.ServiceID = csfm.ServiceID and csfm.ClientID = @ClientID
		join ClientGroupClientMap cgcm with(nolock) on csfm.ClientID = cgcm.ClientID and cgcm.ClientgroupID = @ClientGroupID
		join ClientGroupServiceMap cgsm with(nolock) on cgsm.ClientGroupID = @ClientGroupID and cgsm.ServiceID = csfm.ServiceID
		where a.ApplicationName =@ApplicationName
		 and m.IsActive = 1   
		 and a.IsActive = 1  
		 and csfm.IsEnabled = 1		
		 and cgsm.IsEnabled = 1
		 and ISNULL(m.ServiceID,0) > 0 
		 and ISNULL(m.IsClientGroupService,0) = 0
		 and ISNULL(m.IsSysAdmin,0) = 0
		 and ISNULL(m.IsChannelAdmin,0) = 0
		 and ISNULL(m.IsCustomerAdmin, 0) = 0
	UNION
		select m.* 
		from Menu m with(nolock) 	
		join Application a with(nolock) on m.ApplicationID = a.ApplicationID	
		where a.ApplicationName =@ApplicationName
		 and m.IsActive = 1   
		 and a.IsActive = 1  
		 and ISNULL(m.ServiceFeatureID,0) > 0
		 and IsServiceFeature = 0
		 and ISNULL(m.IsSysAdmin,0) = 0
		 and ISNULL(m.IsChannelAdmin,0) = 0
		 and ISNULL(m.IsCustomerAdmin, 0) = 0
	UNION
		select m.* 
		from Menu m with(nolock) 	
		join Application a with(nolock) on m.ApplicationID = a.ApplicationID	
		where a.ApplicationName =@ApplicationName
		 and m.IsActive = 1   
		 and a.IsActive = 1  
		 and ISNULL(m.ServiceFeatureID,0)  = -1
		 and IsServiceFeature = 0
		 and ISNULL(m.ServiceID, 0) = 0
		 and ISNULL(m.IsSysAdmin,0) = 0
		 and ISNULL(m.IsChannelAdmin,0) = 0
		 and ISNULL(m.IsCustomerAdmin, 0) = 0
	UNION
		select m.* 
		from Menu m with(nolock) 	
		join Application a with(nolock) on m.ApplicationID = a.ApplicationID	
		where a.ApplicationName =@ApplicationName
		 and m.IsActive = 1   
		 and a.IsActive = 1  		 
		 and (ISNULL(m.IsSysAdmin,0) = case when @IsSysAdmin = 1 then 1 else -1 end
		 or ISNULL(m.IsChannelAdmin,0) = case when @AdminLevel = 'ChannelAdministrator' or @IsSysAdmin = 1  then 1 else -1 end
		 or ISNULL(m.IsCustomerAdmin, 0) = case when @AdminLevel = 'administrator' or @AdminLevel = 'ChannelAdministrator' or @IsSysAdmin = 1 then 1 else -1 end
		 )
	) x
	 
	 -- remove duplicates

	select MenuName, ParentMenuID, MIN(MenuID) as MenuID
	INTO #MenusToRemove 
	from #MenuItemsList
	group by MenuName, ParentMenuID
	HAVING COUNT(MenuID) > 1

	delete from #MenuItemsList
	where MenuID in (Select MenuID from #MenusToRemove)

	Select * from #MenuItemsList

	DROP TABLE #tempUserService
	DROP Table #MenuItemsList
	DROP TABLE #MenusToRemove

	
END  
ELSE if @SecurityGroupID = -1  
BEGIN  
	

	select x.* 
	INTO #MenuItemsList2
	FROM(
		 select  m.*  
		 from Menu m with(nolock)  
		 join Application a with(nolock) on m.ApplicationID = a.ApplicationID  
		 join ClientServiceMap csm with(nolock) on  m.serviceID = csm.ServiceID
		 join ClientGroupClientMap cgcm with(nolock) on csm.ClientID = cgcm.ClientID and cgcm.ClientgroupID = @ClientGroupID
		join ClientGroupServiceMap cgsm with(nolock) on cgsm.ClientGroupID = @ClientGroupID and cgsm.ServiceID = csm.ServiceID
		 where a.ApplicationName =@ApplicationName
		 and m.IsActive = 1   
		 and a.IsActive = 1  
		 and csm.ClientID =@clientID
		 and csm.IsEnabled = 1
		 and cgsm.IsEnabled = 1
		 and m.ServiceID > 0
		 and ISNULL(m.IsClientGroupService,0) = 0
	 UNION
		 select  m.*  
		 from Menu m with(nolock)  
		 join Application a with(nolock) on m.ApplicationID = a.ApplicationID  
		 join ClientServiceFeatureMap csm with(nolock) on  m.serviceFeatureID = csm.ServiceFeatureID and csm.ClientID = @clientID
		 where a.ApplicationName = @ApplicationName
		 and m.IsActive = 1   
		 and a.IsActive = 1  	 
		 and csm.IsEnabled = 1
		 and m.ServiceFeatureID > 0
		 and m.IsServiceFeature = 1
	 UNION
		select m.*
		from Menu m with(nolock) 
		--join #tempUserService tus on m.ServiceFeatureID = tus.ServiceFeatureID
		join Application a with(nolock) on m.ApplicationID = a.ApplicationID
		join ClientGroupServiceMap csfm with(nolock) on m.ServiceID = csfm.ServiceID and csfm.ClientGroupID = @ClientGroupID
		where a.ApplicationName =@ApplicationName
		 and m.IsActive = 1   
		 and a.IsActive = 1  
		 and csfm.IsEnabled = 1
		 and ISNULL(m.ServiceID,0) > 0		 
		 and ISNULL(m.IsClientGroupService,0) = 1
	 UNION
		 select m.*  
		 from Menu m with(nolock)  
		 join Application a with(nolock) on m.ApplicationID = a.ApplicationID  	 
		 where a.ApplicationName = @ApplicationName
		 and m.IsActive = 1   
		 and a.IsActive = 1  
		 and m.ServiceFeatureID < 0
		 and m.IsServiceFeature = 0
		 and ISNULL(m.ServiceID,0) = 0
	 )  x

	 select mil.MenuName, mil.ParentMenuID, MIN(mil.MenuID) as MenuID
	INTO #MenusToRemove2 
	from #MenuItemsList2 mil
	group by mil.MenuName, mil.ParentMenuID
	HAVING COUNT(MenuID) > 1

	delete from #MenuItemsList2
	where MenuID in (Select MenuID from #MenusToRemove2)

	Select * from #MenuItemsList2

	
	DROP Table #MenuItemsList2
	DROP TABLE #MenusToRemove2

END