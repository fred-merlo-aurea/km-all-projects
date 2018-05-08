CREATE proc [dbo].[v_ServiceFeature_GetClientGroupTreeList] @ClientGroupID INT = 0 AS 
/*
-- basechannel.add screen
exec dbo.v_ServiceFeature_GetClientGroupTreeList 0
-- basechannel.udate screen
exec dbo.v_ServiceFeature_GetClientGroupTreeList 1
*/
   -- DECLARE @ClientGroupID INT = 0;
   SELECT 'S'  + CONVERT(VARCHAR(100), s.serviceid) AS ID,
	      s.ServiceID       ServiceID,
	      0                 ServiceFeatureID,
          s.servicename     ServiceName, 
          NULL                    ServiceFeatureName, 
          s.Description     Description,
          0                       IsAdditionalCost,
          NULL                    PID,
          isnull(cgsm.ClientGroupServiceMapID,0) MAPID,
          isnull(cgsm.IsEnabled,0) IsEnabled,
          s.DisplayOrder ServiceDisplayOrder,
          0 ServiceFeatureDisplayOrder
          -- CAST(s.DisplayOrder As DECIMAL(18,9)) DisplayOrder
     FROM service s 
     LEFT OUTER JOIN   ClientGroupServiceMap cgsm  on s.ServiceID = cgsm.serviceID AND cgsm.ClientGroupID = @ClientGroupID
    where s.IsEnabled = 1
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
          -- CAST( CAST(s.DisplayOrder As Varchar(50)) + '.' + CAST(sf.DisplayOrder As Varchar(50)) As DECIMAL(18,9)) DisplayOrder
     FROM service s
     JOIN servicefeature sf ON s.serviceid = sf.serviceid 
     LEFT OUTER JOIN ClientGroupServiceFeatureMap cgsfm  
	   ON sf.ServiceID = cgsfm.serviceID 
	  AND sf.ServiceFeatureID = cgsfm.ServiceFeatureID
	  AND sf.ServiceFeatureID = cgsfm.ServiceFeatureID
	  AND cgsfm.ClientGroupID = @ClientGroupID
    where  s.IsEnabled = 1
      AND sf.IsEnabled = 1
    -- ORDER BY DisplayOrder
    ORDER BY ServiceDisplayOrder, 
             ServiceFeatureDisplayOrder;