﻿CREATE proc [dbo].[v_ServiceFeature_GetClientGroupTreeList] 
	@ClientGroupID INT = 0,
	@IsAdditionalCost bit = 1
AS 
/*
-- basechannel.add screen
exec dbo.v_ServiceFeature_GetClientGroupTreeList 0
-- basechannel.udate screen
exec dbo.v_ServiceFeature_GetClientGroupTreeList 1
*/
   -- DECLARE @ClientGroupID INT = 0;
   SELECT 'S'  + CONVERT(VARCHAR(100), s.ServiceID) AS ID,
	      s.ServiceID       ServiceID,
	      0                 ServiceFeatureID,
          s.ServiceName     ServiceName, 
          NULL                    ServiceFeatureName, 
          s.Description     Description,
          0                       IsAdditionalCost,
          NULL                    PID,
          isnull(cgsm.ClientGroupServiceMapID,0) MAPID,
          isnull(cgsm.IsEnabled,0) IsEnabled,
          s.DisplayOrder ServiceDisplayOrder,
          0 ServiceFeatureDisplayOrder
          -- CAST(s.DisplayOrder As DECIMAL(18,9)) DisplayOrder
     FROM Service s 
     LEFT OUTER JOIN   ClientGroupServiceMap cgsm  on s.ServiceID = cgsm.ServiceID AND cgsm.ClientGroupID = @ClientGroupID
    where s.IsEnabled = 1
    UNION 
   SELECT 'SF' + CONVERT(VARCHAR(100), sf.ServiceFeatureID), 
	      s.ServiceID         ServiceID,
	      sf.ServiceFeatureID ServiceFeatureID,
          s.ServiceName, 
          sf.SFName, 
          sf.Description,
          sf.IsAdditionalCost,
          'S' + CONVERT(VARCHAR(100), s.ServiceID), 
          isnull(cgsfm.ClientGroupServiceFeatureMapID,0),
          isnull(cgsfm.IsEnabled,CASE sf.IsAdditionalCost WHEN 1 THEN 0 ELSE 1 END) IsEnabled,
          s.DisplayOrder ServiceDisplayOrder,
          sf.DisplayOrder ServiceFeatureDisplayOrder
          -- CAST( CAST(s.DisplayOrder As Varchar(50)) + '.' + CAST(sf.DisplayOrder As Varchar(50)) As DECIMAL(18,9)) DisplayOrder
     FROM Service s
     JOIN ServiceFeature sf ON s.ServiceID = sf.ServiceID 
     LEFT OUTER JOIN ClientGroupServiceFeatureMap cgsfm ON sf.ServiceID = cgsfm.ServiceID 
													  AND sf.ServiceFeatureID = cgsfm.ServiceFeatureID
													  AND cgsfm.ClientGroupID = @ClientGroupID
    where  s.IsEnabled = 1
      AND sf.IsEnabled = 1
      AND sf.IsAdditionalCost = @IsAdditionalCost
    -- ORDER BY DisplayOrder
    ORDER BY ServiceDisplayOrder, 
             ServiceFeatureDisplayOrder;