-- DROP proc [dbo].[v_ServiceFeature_GetSecurityGroupTreeList] 
CREATE proc [dbo].[v_ServiceFeature_GetSecurityGroupTreeList] 
	@SecurityGroupID INT = 0,
	@ClientGroupID   INT = 0,
	@ClientID        INT = -1
AS 
/*
-- role.udate screen (channel wide for channel 1
exec dbo.v_ServiceFeature_GetSecurityGroupTreeList 12345, 1, 0
-- role.udate screen (for client 1
exec dbo.v_ServiceFeature_GetSecurityGroupTreeList 12345, 0, 1
*/
   -- DECLARE @SecurityGroupID INT = 19, @ClientGroupID   INT = 0, @ClientID        INT = 6;
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
     LEFT OUTER JOIN ClientGroupServiceMap cgsm (NOLOCK) on s.ServiceID = cgsm.serviceID AND cgsm.ClientGroupID = @ClientGroupID
     LEFT OUTER JOIN ClientServiceMap csm (NOLOCK) on s.ServiceID = csm.ServiceID AND csm.ClientID = @ClientID
    LEFT OUTER join ClientGroupServiceFeatureMap cgsf on cgsm.ServiceID = cgsf.ServiceID AND cgsm.ClientGroupID = cgsf.ClientGroupID
	LEFT OUTER join ServiceFeatureAccessMap sfam on sfam.ServiceFeatureID = cgsf.ServiceFeatureID
	LEFT OUTER join Access a on a.AccessID = sfam.AccessID 
	LEFT OUTER join ServiceFeature sf on sf.ServiceFeatureID = sfam.ServiceFeatureID
    WHERE        s.IsEnabled = 1 AND ((@ClientID > 0 AND csm.IsEnabled = 1) OR (@ClientGroupID > 0 AND cgsm.IsEnabled = 1))
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
     FROM service s(NOLOCK) 
     JOIN servicefeature sf (NOLOCK) ON s.serviceid = sf.serviceid 
	 LEFT OUTER JOIN ClientServiceFeatureMap csfm (NOLOCK) on sf.ServiceFeatureID  = csfm.ServiceFeatureID AND csfm.ClientID = @ClientID
     LEFT OUTER JOIN ClientGroupServiceFeatureMap cgsfm  (NOLOCK) ON sf.ServiceFeatureID = cgsfm.ServiceFeatureID AND cgsfm.ClientGroupID = @ClientGroupID
     -- LEFT OUTER join        ClientServiceMap csm  (NOLOCK) on csfm.ClientID = csm.ClientID AND csm.ServiceID = s.ServiceID
     -- LEFT OUTER join        ClientGroupServiceMap cgsm  (NOLOCK) on csfm.ClientID = cgsm.ClientGroupID AND cgsm.ServiceID = s.ServiceID
     LEFT OUTER join ServiceFeatureAccessMap sfam (NOLOCK) on sfam.ServiceFeatureID = NULLIF(csfm.ServiceFeatureID, -1)
                                                           or sfam.ServiceFeatureID = NULLIF(cgsfm.ServiceFeatureID, -1)
	 LEFT OUTER join                  Access a    (NOLOCK) on a.AccessID = sfam.AccessID 
    where  s.IsEnabled = 1
      AND sf.IsEnabled = 1
      AND ((@ClientID > 0 AND csfm.IsEnabled = 1) OR (@ClientGroupID > 0 AND cgsfm.IsEnabled = 1))
 GROUP BY sf.ServiceFeatureID, s.ServiceID, sf.ServiceFeatureID, s.ServiceName, sf.SFName, sf.Description, sf.IsAdditionalCost, s.DisplayOrder, sf.DisplayOrder
   HAVING COUNT( a.AccessID ) > 0
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
     FROM ClientGroupServiceMap cgsm 
	join ClientGroupServiceFeatureMap cgsf on cgsm.ServiceID = cgsf.ServiceID
	join ServiceFeatureAccessMap sfam on sfam.ServiceFeatureID = cgsf.ServiceFeatureID
	join Access a on a.AccessID = sfam.AccessID 
	join ServiceFeature sf on sf.ServiceFeatureID = sfam.ServiceFeatureID
	join Service s on s.ServiceID = sf.ServiceID
	LEFT OUTER JOIN SecurityGroupPermission sgp ON sgp.SecurityGroupID = @SecurityGroupID AND sgp.ServiceFeatureAccessMapID = sfam.ServiceFeatureAccessMapID
   WHERE cgsm.ClientGroupID = @ClientGroupID
	 and cgsf.ClientGroupID = @ClientGroupID 
	 and cgsm.IsEnabled = 1
	 and cgsf.IsEnabled = 1
	 and sfam.IsEnabled = 1
	 and sf.IsEnabled = 1
	 and s.IsEnabled = 1
UNION SELECT 'A' + CONVERT(VARCHAR(100), sf.servicefeatureid), 
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
     FROM ClientServiceFeatureMap csf  (NOLOCK)
	join          ServiceFeature sf   (NOLOCK) on sf.ServiceFeatureID = csf.ServiceFeatureID
	join                 Service s    (NOLOCK) on s.ServiceID = sf.ServiceID
	join        ClientServiceMap csm  (NOLOCK) on csf.ClientID = csm.ClientID AND csm.ServiceID = s.ServiceID
	join ServiceFeatureAccessMap sfam (NOLOCK) on sfam.ServiceFeatureID = csf.ServiceFeatureID
	join                  Access a    (NOLOCK) on a.AccessID = sfam.AccessID 
	LEFT OUTER JOIN SecurityGroupPermission sgp ON sgp.SecurityGroupID = @SecurityGroupID AND sgp.ServiceFeatureAccessMapID = sfam.ServiceFeatureAccessMapID
   WHERE csm.ClientID = @ClientID
     AND csf.ClientID = @ClientID
	 and csm.IsEnabled = 1
	 and csf.IsEnabled = 1
	 and sfam.IsEnabled = 1
	 and sf.IsEnabled = 1
	 and s.IsEnabled = 1
    ORDER BY ServiceDisplayOrder, 
             ServiceFeatureDisplayOrder;