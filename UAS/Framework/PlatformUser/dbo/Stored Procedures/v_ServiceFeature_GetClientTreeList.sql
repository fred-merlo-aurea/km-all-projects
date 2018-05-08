-- DROP proc [dbo].[v_ServiceFeature_GetClientTreeList]
CREATE proc [dbo].[v_ServiceFeature_GetClientTreeList] @ClientGroupID INT, @ClientID INT = 0 AS 
/*
-- customer.add screen
exec dbo.v_ServiceFeature_GetClientTreeList 0
-- customer.udate screen
exec dbo.v_ServiceFeature_GetClientTreeList 1
*/
   -- DECLARE @ClientGroupID INT = 75, @ClientID INT = 0;
SELECT 'S'  + CONVERT(VARCHAR(100), s.serviceid) 
                      AS ID,
	      s.ServiceID    ServiceID,
	      0              ServiceFeatureID,
          s.servicename  ServiceName, 
          NULL           ServiceFeatureName, 
          s.Description  Description,
          0              IsAdditionalCost,
          NULL           PID,
          isnull(csm.ClientServiceMapID,0) MAPID,
          isnull(csm.IsEnabled,0)          IsEnabled,
          s.DisplayOrder                   ServiceDisplayOrder,
          0  ServiceFeatureDisplayOrder
     FROM service s 
     JOIN ClientGroupServiceMap       cgsm  on s.ServiceID = cgsm.serviceID AND cgsm.ClientGroupID = @ClientGroupID
     LEFT OUTER JOIN ClientServiceMap csm   on s.ServiceID = csm.serviceID  AND csm.ClientID       = @ClientID
    where cgsm.IsEnabled = 1 AND s.IsEnabled = 1

UNION

SELECT 'SF' + CONVERT(VARCHAR(100), sf.servicefeatureid), 
	      s.ServiceID         ServiceID,
	      sf.ServiceFeatureID ServiceFeatureID,
          s.servicename, 
          sf.sfname, 
          sf.Description,
          sf.IsAdditionalCost,
          'S' + CONVERT(VARCHAR(100), s.serviceid), 
          isnull(csfm.ClientServiceFeatureMapID,0),
          isnull(csfm.IsEnabled,CASE sf.IsAdditionalCost WHEN 1 THEN 0 ELSE 1 END) IsEnabled,
          s.DisplayOrder ServiceDisplayOrder,
          sf.DisplayOrder ServiceFeatureDisplayOrder
     FROM service s
     JOIN servicefeature sf ON s.serviceid = sf.serviceid 
     LEFT JOIN ClientGroupServiceFeatureMap cgsfm ON cgsfm.ServiceFeatureID = sf.ServiceFeatureID AND cgsfm.ClientGroupID = @ClientGroupID
     LEFT OUTER JOIN ClientServiceFeatureMap csfm on sf.ServiceFeatureID  = csfm.ServiceFeatureID AND csfm.ClientID = @ClientID


where     s.IsEnabled = 1
      AND    sf.IsEnabled = 1
      AND cgsfm.IsEnabled = 1
    ORDER BY ServiceDisplayOrder, 
             ServiceFeatureDisplayOrder;
             
/*                
   SELECT 'S'  + CONVERT(VARCHAR(100), s.serviceid) 
                      AS ID,
	      s.ServiceID    ServiceID,
	      0              ServiceFeatureID,
          s.servicename  ServiceName, 
          NULL           ServiceFeatureName, 
          s.Description  Description,
          0              IsAdditionalCost,
          NULL           PID,
          isnull(csm.ClientServiceMapID,0) MAPID,
          isnull(csm.IsEnabled,0)          IsEnabled,
          s.DisplayOrder                   ServiceDisplayOrder,
          0  ServiceFeatureDisplayOrder
     FROM service s 
     JOIN ClientGroupServiceMap       cgsm  on s.ServiceID = cgsm.serviceID AND cgsm.ClientGroupID = @ClientGroupID
     LEFT OUTER JOIN ClientServiceMap csm   on s.ServiceID = csm.serviceID  AND csm.ClientID       = @ClientID
    where cgsm.IsEnabled = 1 AND s.IsEnabled = 1
    UNION 
   SELECT 'SF' + CONVERT(VARCHAR(100), sf.servicefeatureid), 
	      s.ServiceID         ServiceID,
	      sf.ServiceFeatureID ServiceFeatureID,
          s.servicename, 
          sf.sfname, 
          sf.Description,
          sf.IsAdditionalCost,
          'S' + CONVERT(VARCHAR(100), s.serviceid), 
          isnull(cgsfm.ClientGroupServiceFeatureMapID,0),
          isnull(cgsfm.IsEnabled,CASE sf.IsAdditionalCost WHEN 1 THEN 0 ELSE 1 END) IsEnabled,
          s.DisplayOrder ServiceDisplayOrder,
          sf.DisplayOrder ServiceFeatureDisplayOrder
     FROM service s
     JOIN servicefeature sf ON s.serviceid = sf.serviceid 
     LEFT JOIN ClientGroupServiceFeatureMap cgsfm ON cgsfm.ServiceFeatureID = sf.ServiceFeatureID AND cgsfm.ClientGroupID = @ClientGroupID
     LEFT OUTER JOIN ClientServiceFeatureMap csfm on sf.ServiceFeatureID  = csfm.ServiceFeatureID AND csfm.ClientID = @ClientID
    where     s.IsEnabled = 1
      AND    sf.IsEnabled = 1
      AND cgsfm.IsEnabled = 1
    ORDER BY ServiceDisplayOrder, 
             ServiceFeatureDisplayOrder;
*/             