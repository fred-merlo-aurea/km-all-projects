CREATE proc [dbo].[v_ServiceFeature_GetSecurityGroupTreeList] 
	@SecurityGroupID INT = 0,
	@ClientGroupID   INT = 0,
	@ClientID        INT = -1
AS 



--declare @SecurityGroupID INT = 2076,
--	@ClientGroupID   INT = 0,
--	@ClientID        INT = -1
	
/*
-- role.udate screen (channel wide for channel 1
exec dbo.v_ServiceFeature_GetSecurityGroupTreeList 12345, 1, 0
-- role.udate screen (for client 1
exec dbo.v_ServiceFeature_GetSecurityGroupTreeList 12345, 0, 1
*/

   -- DECLARE @SecurityGroupID INT = 19, @ClientGroupID   INT = 0, @ClientID        INT = 6;
   declare @existingClientGroupID int,@existingClientID int
   
   select @existingClientGroupID = ISNULL(ClientGroupID,0), @existingClientID = ISNULL(ClientID,0)
   FROM SecurityGroup sg with(nolock)
   where sg.SecurityGroupID = @SecurityGroupID
   
   if @existingClientGroupID > 0
   BEGIN
   SELECT 'S'  + CONVERT(VARCHAR(100), s.serviceid) AS ID,
	      s.ServiceID       ServiceID,
	      0                 ServiceFeatureID,
	      0                 ServiceFeatureAccessMapID,
          s.servicename     ServiceName, 
          NULL              ServiceFeatureName,
          NULL              AccessLevelName, 
          s.Description     Description,
          0                 IsAdditionalCost,
          NULL              PID,
          0                 MAPID,
          1                 IsEnabled,
          s.DisplayOrder    ServiceDisplayOrder,
          0                 ServiceFeatureDisplayOrder          
     FROM service s 
     JOIN ClientGroupServiceMap cgsm (NOLOCK) on s.ServiceID = cgsm.serviceID AND cgsm.ClientGroupID = @existingClientGroupID
     join ClientGroupServiceFeatureMap cgsf with(nolock) on cgsm.ServiceID = cgsf.ServiceID AND cgsm.ClientGroupID = cgsf.ClientGroupID
	 JOIN ServiceFeatureAccessMap sfam with(nolock) on sfam.ServiceFeatureID = cgsf.ServiceFeatureID
	 JOIN Access a with(nolock) on a.AccessID = sfam.AccessID 
	 join ServiceFeature sf with(nolock) on sf.ServiceFeatureID = sfam.ServiceFeatureID
    WHERE        s.IsEnabled = 1 AND  cgsm.IsEnabled = 1 and cgsf.IsEnabled = 1 and sfam.IsEnabled = 1 and a.IsActive = 1 and sf.IsEnabled = 1
    GROUP BY s.ServiceID, s.ServiceName, s.Description, s.DisplayOrder
    HAVING COUNT( a.AccessID ) > 0
    UNION 
   SELECT 'SF' + CONVERT(VARCHAR(100), sf.servicefeatureid), 
	      s.ServiceID         ServiceID,
	      sf.ServiceFeatureID ServiceFeatureID,
	      0                 ServiceFeatureAccessMapID,
          s.servicename, 
          sf.sfname, 
          NULL                    AccessLevelName, 
          sf.Description,
          sf.IsAdditionalCost,
          'S' + CONVERT(VARCHAR(100), s.serviceid), 
          0, -- mapid
          1 IsEnabled,
          s.DisplayOrder ServiceDisplayOrder,
          sf.DisplayOrder ServiceFeatureDisplayOrder
     FROM service s 
     JOIN ClientGroupServiceMap cgsm (NOLOCK) on s.ServiceID = cgsm.serviceID AND cgsm.ClientGroupID = @existingClientGroupID
     join ClientGroupServiceFeatureMap cgsf with(nolock) on cgsm.ServiceID = cgsf.ServiceID AND cgsm.ClientGroupID = cgsf.ClientGroupID
	 JOIN ServiceFeatureAccessMap sfam with(nolock) on sfam.ServiceFeatureID = cgsf.ServiceFeatureID
	 JOIN Access a with(nolock) on a.AccessID = sfam.AccessID 
	 join ServiceFeature sf with(nolock) on sf.ServiceFeatureID = sfam.ServiceFeatureID
    WHERE        s.IsEnabled = 1 AND  cgsm.IsEnabled = 1 and cgsf.IsEnabled = 1 and sfam.IsEnabled = 1 and a.IsActive = 1 and sf.IsEnabled = 1
 GROUP BY sf.ServiceFeatureID, s.ServiceID, sf.ServiceFeatureID, s.ServiceName, sf.SFName, sf.Description, sf.IsAdditionalCost, s.DisplayOrder, sf.DisplayOrder
    UNION 
   SELECT 'A' + CONVERT(VARCHAR(100), sf.servicefeatureid), 
	      s.ServiceID         ServiceID,
	      sf.ServiceFeatureID ServiceFeatureID,
		  ISNULL(sfam.ServiceFeatureAccessMapID, 0) ServiceFeatureAccessMapID,
          s.ServiceName, 
          sf.sfname, 
          ISNULL(a.AccessName, '???') AccessLevelName, 
          sfam.description [Description],
          sf.IsAdditionalCost,
          'SF' + CONVERT(VARCHAR(100), sf.servicefeatureid), -- PID
          ISNULL(sgp.SecurityGroupPermissionID, 0), -- mapid
          ISNULL(sgp.IsActive, 0) IsEnabled,
          s.DisplayOrder ServiceDisplayOrder,
          sf.DisplayOrder ServiceFeatureDisplayOrder
          -- , *
     FROM service s 
     JOIN ClientGroupServiceMap cgsm (NOLOCK) on s.ServiceID = cgsm.serviceID AND cgsm.ClientGroupID = @existingClientGroupID
     join ClientGroupServiceFeatureMap cgsf with(nolock) on cgsm.ServiceID = cgsf.ServiceID AND cgsm.ClientGroupID = cgsf.ClientGroupID
	 JOIN ServiceFeatureAccessMap sfam with(nolock) on sfam.ServiceFeatureID = cgsf.ServiceFeatureID
	 JOIN Access a with(nolock) on a.AccessID = sfam.AccessID 
	 join ServiceFeature sf with(nolock) on sf.ServiceFeatureID = sfam.ServiceFeatureID
	 LEFT OUTER JOIN SecurityGroupPermission sgp with(nolock) on sfam.ServiceFeatureAccessMapID = sgp.ServiceFeatureAccessMapID and sgp.SecurityGroupID = @SecurityGroupID
    WHERE        s.IsEnabled = 1 AND  cgsm.IsEnabled = 1 and cgsf.IsEnabled = 1 and sfam.IsEnabled = 1 and a.IsActive = 1 and sf.IsEnabled = 1 
    ORDER BY ServiceDisplayOrder, 
             ServiceFeatureDisplayOrder;
END
ELSE IF @existingClientID > 0
BEGIN

	select @existingClientGroupID = ClientgroupID 
	from
			ClientGroupClientMap with (NOLOCK)
	Where
			ClientID = @existingClientID
					
		SELECT 'S'  + CONVERT(VARCHAR(100), s.serviceid) AS ID,
				  s.ServiceID       ServiceID,
				  0                 ServiceFeatureID,
				  0                 ServiceFeatureAccessMapID,
				  s.servicename     ServiceName, 
				  NULL              ServiceFeatureName,
				  NULL              AccessLevelName, 
				  s.Description     Description,
				  0                 IsAdditionalCost,
				  NULL              PID,
				  0                 MAPID,
				  1                 IsEnabled,
				  s.DisplayOrder    ServiceDisplayOrder,
				  0                 ServiceFeatureDisplayOrder          
			 FROM service s 
			 JOIN ClientGroupServiceMap cgsm (NOLOCK) on s.ServiceID = cgsm.serviceID AND cgsm.ClientGroupID = @existingClientGroupID
			 join ClientGroupServiceFeatureMap cgsf with(nolock) on cgsm.ServiceID = cgsf.ServiceID AND cgsm.ClientGroupID = cgsf.ClientGroupID
			 JOIN ClientServiceMap csm with(nolock) on csm.ServiceID = cgsm.serviceID and csm.ClientID = @existingClientID
			 JOIN ClientServiceFeatureMap csfm with(nolock) on csfm.ServiceFeatureID =  cgsf.ServiceFeatureID  and csfm.ClientID = @existingClientID
			 JOIN ServiceFeatureAccessMap sfam with(nolock) on sfam.ServiceFeatureID = csfm.ServiceFeatureID
			 JOIN Access a with(nolock) on a.AccessID = sfam.AccessID 
			 join ServiceFeature sf with(nolock) on sf.ServiceFeatureID = sfam.ServiceFeatureID
			WHERE        s.IsEnabled = 1 AND  cgsm.IsEnabled = 1 and cgsf.IsEnabled = 1 and csm.IsEnabled = 1 and csfm.IsEnabled = 1 and sfam.IsEnabled = 1 and a.IsActive = 1 and sf.IsEnabled = 1
			GROUP BY s.ServiceID, s.ServiceName, s.Description, s.DisplayOrder
			HAVING COUNT( a.AccessID ) > 0
			UNION 
		   SELECT 'SF' + CONVERT(VARCHAR(100), sf.servicefeatureid), 
				  s.ServiceID         ServiceID,
				  sf.ServiceFeatureID ServiceFeatureID,
				  0                 ServiceFeatureAccessMapID,
				  s.servicename, 
				  sf.sfname, 
				  NULL                    AccessLevelName, 
				  sf.Description,
				  sf.IsAdditionalCost,
				  'S' + CONVERT(VARCHAR(100), s.serviceid), 
				  0, -- mapid
				  1 IsEnabled,
				  s.DisplayOrder ServiceDisplayOrder,
				  sf.DisplayOrder ServiceFeatureDisplayOrder
			  FROM service s 
			 JOIN ClientGroupServiceMap cgsm (NOLOCK) on s.ServiceID = cgsm.serviceID AND cgsm.ClientGroupID = @existingClientGroupID
			 join ClientGroupServiceFeatureMap cgsf with(nolock) on cgsm.ServiceID = cgsf.ServiceID AND cgsm.ClientGroupID = cgsf.ClientGroupID
			 JOIN ClientServiceMap csm with(nolock) on csm.ServiceID = cgsm.serviceID and csm.ClientID = @existingClientID
			 JOIN ClientServiceFeatureMap csfm with(nolock) on csfm.ServiceFeatureID =  cgsf.ServiceFeatureID  and csfm.ClientID = @existingClientID
			 JOIN ServiceFeatureAccessMap sfam with(nolock) on sfam.ServiceFeatureID = csfm.ServiceFeatureID
			 JOIN Access a with(nolock) on a.AccessID = sfam.AccessID 
			 join ServiceFeature sf with(nolock) on sf.ServiceFeatureID = sfam.ServiceFeatureID
			WHERE        s.IsEnabled = 1 AND  cgsm.IsEnabled = 1 and cgsf.IsEnabled = 1 and csm.IsEnabled = 1 and csfm.IsEnabled = 1 and sfam.IsEnabled = 1 and a.IsActive = 1 and sf.IsEnabled = 1
		 GROUP BY sf.ServiceFeatureID, s.ServiceID, sf.ServiceFeatureID, s.ServiceName, sf.SFName, sf.Description, sf.IsAdditionalCost, s.DisplayOrder, sf.DisplayOrder
			UNION 
		   SELECT 'A' + CONVERT(VARCHAR(100), sf.servicefeatureid), 
				  s.ServiceID         ServiceID,
				  sf.ServiceFeatureID ServiceFeatureID,
				  ISNULL(sfam.ServiceFeatureAccessMapID, 0) ServiceFeatureAccessMapID,
				  s.ServiceName, 
				  sf.sfname, 
				  ISNULL(a.AccessName, '???') AccessLevelName, 
				  sfam.description [Description],
				  sf.IsAdditionalCost,
				  'SF' + CONVERT(VARCHAR(100), sf.servicefeatureid), -- PID
				  ISNULL(sgp.SecurityGroupPermissionID, 0), -- mapid
				  ISNULL(sgp.IsActive, 0) IsEnabled,
				  s.DisplayOrder ServiceDisplayOrder,
				  sf.DisplayOrder ServiceFeatureDisplayOrder
				  -- , *
			  FROM service s 
			 JOIN ClientGroupServiceMap cgsm (NOLOCK) on s.ServiceID = cgsm.serviceID AND cgsm.ClientGroupID = @existingClientGroupID
			 join ClientGroupServiceFeatureMap cgsf with(nolock) on cgsm.ServiceID = cgsf.ServiceID AND cgsm.ClientGroupID = cgsf.ClientGroupID
			 JOIN ClientServiceMap csm with(nolock) on csm.ServiceID = cgsm.serviceID and csm.ClientID = @existingClientID
			 JOIN ClientServiceFeatureMap csfm with(nolock) on csfm.ServiceFeatureID =  cgsf.ServiceFeatureID  and csfm.ClientID = @existingClientID
			 JOIN ServiceFeatureAccessMap sfam with(nolock) on sfam.ServiceFeatureID = csfm.ServiceFeatureID
			 JOIN Access a with(nolock) on a.AccessID = sfam.AccessID 
			 join ServiceFeature sf with(nolock) on sf.ServiceFeatureID = sfam.ServiceFeatureID
			 LEFT OUTER JOIN SecurityGroupPermission sgp with(nolock) on sfam.ServiceFeatureAccessMapID = sgp.ServiceFeatureAccessMapID and sgp.SecurityGroupID = @SecurityGroupID
			 WHERE  s.IsEnabled = 1 AND  cgsm.IsEnabled = 1 and cgsf.IsEnabled = 1 and csm.IsEnabled = 1 and csfm.IsEnabled = 1 and sfam.IsEnabled = 1 and a.IsActive = 1 and sf.IsEnabled = 1 
			ORDER BY ServiceDisplayOrder, 
					 ServiceFeatureDisplayOrder;
END