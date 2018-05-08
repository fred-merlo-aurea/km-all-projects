CREATE proc [dbo].[v_ServiceFeature_GetEmptySecurityGroupTreeList] 
	@ClientGroupID   INT = 0,
	@ClientID        INT = -1
AS 
/*
-- basechannel.add screen
exec dbo.v_ServiceFeature_GetClientGroupTreeList 0
-- basechannel.udate screen
exec dbo.v_ServiceFeature_GetClientGroupTreeList 1
*/
    --DECLARE @SecurityGroupID INT = 3, @ClientGroupID   INT = 1, @ClientID        INT = 1;
  if(@ClientGroupID > 0 AND @ClientID < 0 )
 BEGIN
    --GRABS SERVICES
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
     JOIN ClientGroupServiceMap cgsm (NOLOCK) on s.ServiceID = cgsm.serviceID AND cgsm.ClientGroupID = @ClientGroupID
     join ClientGroupServiceFeatureMap cgsf on cgsm.ServiceID = cgsf.ServiceID AND cgsm.ClientGroupID = cgsf.ClientGroupID
	 join ServiceFeatureAccessMap sfam on sfam.ServiceFeatureID = cgsf.ServiceFeatureID
	 --join Access a on a.AccessID = sfam.AccessID 
	 join ServiceFeature sf on sf.ServiceFeatureID = sfam.ServiceFeatureID
    WHERE        s.IsEnabled = 1 
    AND (@ClientGroupID > 0 AND cgsm.IsEnabled = 1)
	GROUP BY s.ServiceID, s.ServiceName, s.Description, s.DisplayOrder
	HAVING COUNT( cgsf.ClientGroupServiceFeatureMapID ) > 0
UNION 
	----------GRABS SERVICE FEATURES
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
     FROM servicefeature sf (NOLOCK)
     JOIN service s(NOLOCK)  ON s.serviceid = sf.serviceid 
	 JOIN ClientGroupServiceFeatureMap cgsfm  (NOLOCK) ON @ClientGroupID > 0 AND sf.ServiceFeatureID = cgsfm.ServiceFeatureID AND cgsfm.ClientGroupID = @ClientGroupID
	 JOIN ServiceFeatureAccessMap sfm (NOLOCK) ON sf.ServiceFeatureID = sfm.ServiceFeatureID
	 LEFT OUTER JOIN Access a (NOLOCK) ON sfm.AccessID = a.AccessID
    where  s.IsEnabled = 1
      AND sf.IsEnabled = 1
      AND (@ClientGroupID > 0 AND cgsfm.IsEnabled = 1)
      AND sfm.IsEnabled = 1
--      AND ( sfm.IsEnabled IS NULL OR sfm.IsEnabled = 1 )
 GROUP BY sf.ServiceFeatureID, s.ServiceID, sf.ServiceFeatureID, s.ServiceName, sf.SFName, sf.Description, sf.IsAdditionalCost, s.DisplayOrder, sf.DisplayOrder
   HAVING COUNT( a.AccessID ) > 0
   ------GRABS CLIENT GROUP SERVICE FEATURES WITH ACCESS
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
          0, -- ISNULL(cgsgp.SecurityGroupPermissionID, 0), -- mapid
          0, -- ISNULL(cgsgp.IsActive, 0) IsEnabled,
          s.DisplayOrder ServiceDisplayOrder,
          sf.DisplayOrder ServiceFeatureDisplayOrder
          -- , *
     FROM ClientGroupServiceMap cgsm 
	join ClientGroupServiceFeatureMap cgsf on cgsm.ServiceID = cgsf.ServiceID
	join ServiceFeatureAccessMap sfam on sfam.ServiceFeatureID = cgsf.ServiceFeatureID
	join Access a on a.AccessID = sfam.AccessID 
	join ServiceFeature sf on sf.ServiceFeatureID = sfam.ServiceFeatureID
	join Service s on s.ServiceID = sf.ServiceID
   WHERE cgsm.ClientGroupID = @ClientGroupID
	 and cgsf.ClientGroupID = @ClientGroupID 
	 and cgsm.IsEnabled = 1
	 and cgsf.IsEnabled = 1
	 and sfam.IsEnabled = 1
	 and sf.IsEnabled = 1
	 and s.IsEnabled = 1
 ORDER BY ServiceDisplayOrder, 
     ServiceFeatureDisplayOrder
END
ELSE IF @ClientGroupID > 0 and @ClientID > 0
BEGIN
 --GRABS SERVICES
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
		 JOIN ClientGroupServiceMap cgsm (NOLOCK) on s.ServiceID = cgsm.serviceID AND cgsm.ClientGroupID = @ClientGroupID
		 JOIN ClientServiceMap csm (NOLOCK) on s.ServiceID = csm.ServiceID AND csm.ClientID = @ClientID
		 join ClientGroupServiceFeatureMap cgsf on s.ServiceID = cgsf.ServiceID AND cgsm.ClientGroupID = cgsf.ClientGroupID
		 join ClientServiceFeatureMap csf on csf.ClientID = csm.ClientID
		 join ServiceFeature sf2 ON csf.ServiceFeatureID = sf2.ServiceFeatureID AND sf2.ServiceID = s.ServiceID
		 join ServiceFeatureAccessMap sfam on sfam.ServiceFeatureID = cgsf.ServiceFeatureID OR sfam.ServiceFeatureID = csf.ServiceFeatureID
     WHERE        s.IsEnabled = 1 AND cgsm.IsEnabled = 1 AND csm.IsEnabled = 1  AND cgsf.IsEnabled = 1  AND csf.IsEnabled = 1 AND sf2.IsEnabled = 1 AND sfam.IsEnabled = 1 
	 GROUP BY s.ServiceID, s.ServiceName, s.Description, s.DisplayOrder
	 HAVING COUNT( csf.ClientServiceFeatureMapID ) > 0
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
    FROM servicefeature sf (NOLOCK)
		 JOIN service s(NOLOCK)  ON s.serviceid = sf.serviceid 
		 JOIN ClientServiceFeatureMap csfm (NOLOCK) on  sf.ServiceFeatureID  = csfm.ServiceFeatureID AND csfm.ClientID = @ClientID
		 JOIN ClientGroupServiceFeatureMap cgsfm  (NOLOCK) ON  sf.ServiceFeatureID = cgsfm.ServiceFeatureID AND cgsfm.ClientGroupID = @ClientGroupID
		 JOIN ServiceFeatureAccessMap sfm (NOLOCK) ON sf.ServiceFeatureID = sfm.ServiceFeatureID
		 JOIN Access a (NOLOCK) ON sfm.AccessID = a.AccessID
    where  s.IsEnabled = 1
		  AND sf.IsEnabled = 1
		  AND csfm.IsEnabled = 1 
		  AND cgsfm.IsEnabled = 1
		  AND sfm.IsEnabled = 1
	 ------GRABS CLIENT SERVICE FEATURES WITH ACCESS
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
          0, -- ISNULL(cgsgp.SecurityGroupPermissionID, 0), -- mapid
          0, -- ISNULL(cgsgp.IsActive, 0) IsEnabled,
          s.DisplayOrder ServiceDisplayOrder,
          sf.DisplayOrder ServiceFeatureDisplayOrder
          -- , *
     FROM ClientServiceFeatureMap csf  (NOLOCK)
			join ServiceFeature sf   (NOLOCK) on sf.ServiceFeatureID = csf.ServiceFeatureID
			join Service s    (NOLOCK) on s.ServiceID = sf.ServiceID
			join ClientServiceMap csm  (NOLOCK) on csf.ClientID = csm.ClientID AND csm.ServiceID = s.ServiceID
			join ServiceFeatureAccessMap sfam (NOLOCK) on sfam.ServiceFeatureID = csf.ServiceFeatureID
			join Access a    (NOLOCK) on a.AccessID = sfam.AccessID 
   WHERE 
			csm.ClientID = @ClientID
			 AND csf.ClientID = @ClientID
			 and csm.IsEnabled = 1
			 and csf.IsEnabled = 1
			 and sfam.IsEnabled = 1
			 and sf.IsEnabled = 1
			 and s.IsEnabled = 1
    ORDER BY ServiceDisplayOrder, 
     ServiceFeatureDisplayOrder
END