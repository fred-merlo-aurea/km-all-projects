-- DROP proc [dbo].[v_ServiceFeature_GetClientTreeList]
CREATE proc [dbo].[v_ServiceFeature_GetClientTreeList] 
	@ClientGroupID INT, 
	@ClientID INT = -1 ,
	@IsAdditionalCost bit = 1
AS 
/*
-- customer.add screen
exec dbo.v_ServiceFeature_GetClientTreeList 0
-- customer.udate screen
exec dbo.v_ServiceFeature_GetClientTreeList 1
*/
    --DECLARE @ClientGroupID INT = 1, @ClientID INT = 1
SELECT 'S'  + CONVERT(VARCHAR(100), s.ServiceID) 
                      AS ID,
	      s.ServiceID    ServiceID,
	      0              ServiceFeatureID,
          s.ServiceName  ServiceName, 
          NULL           ServiceFeatureName, 
          s.Description  Description,
          0              IsAdditionalCost,
          NULL           PID,
          isnull(csm.ClientServiceMapID,0) MAPID,
          isnull(csm.IsEnabled,0)          IsEnabled,
          s.DisplayOrder                   ServiceDisplayOrder,
          0  ServiceFeatureDisplayOrder
     FROM Service s 
     JOIN ClientGroupServiceMap       cgsm  on s.ServiceID = cgsm.ServiceID AND cgsm.ClientGroupID = @ClientGroupID
     LEFT OUTER JOIN ClientServiceMap csm   on s.ServiceID = csm.ServiceID  AND csm.ClientID       = @ClientID
    where cgsm.IsEnabled = 1 AND s.IsEnabled = 1

UNION

SELECT distinct 'SF' + CONVERT(VARCHAR(100), sf.ServiceFeatureID), 
	      s.ServiceID         ServiceID,
	      sf.ServiceFeatureID ServiceFeatureID,
          s.ServiceName, 
          sf.SFName			  ServiceFeatureName, 
          sf.Description,
          sf.IsAdditionalCost,
          'S' + CONVERT(VARCHAR(100), s.ServiceID) PID, 
          isnull(csfm.ClientServiceFeatureMapID,0) MAPID,
          isnull(csfm.IsEnabled,CASE sf.IsAdditionalCost WHEN 1 THEN 0 ELSE 1 END) IsEnabled,
          s.DisplayOrder ServiceDisplayOrder,
          sf.DisplayOrder ServiceFeatureDisplayOrder
     FROM Service s
     JOIN ServiceFeature sf ON s.ServiceID = sf.ServiceID  
     LEFT JOIN ClientGroupServiceFeatureMap cgsfm ON cgsfm.ServiceFeatureID = sf.ServiceFeatureID AND cgsfm.ClientGroupID = @ClientGroupID
     LEFT OUTER JOIN ClientServiceFeatureMap csfm on sf.ServiceFeatureID  = csfm.ServiceFeatureID AND csfm.ClientID = @ClientID


where     s.IsEnabled = 1
      AND    sf.IsEnabled = 1
      AND cgsfm.IsEnabled = 1
      AND sf.IsAdditionalCost = @IsAdditionalCost
    ORDER BY ServiceDisplayOrder, 
             ServiceFeatureDisplayOrder;