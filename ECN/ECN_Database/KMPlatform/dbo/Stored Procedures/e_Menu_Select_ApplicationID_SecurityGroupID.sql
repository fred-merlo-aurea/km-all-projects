CREATE PROCEDURE [dbo].[e_Menu_Select_ApplicationID_SecurityGroupID]
@SecurityGroupID int,
@ApplicationID int
AS

SELECT distinct m.* 
FROM Menu m With(NoLock) JOIN
MenuServiceFeatureMap msfm ON msfm.MenuID = m.MenuID
WHERE 
m.ApplicationID = @ApplicationID AND 
m.IsActive = 1 AND
msfm.ServiceFeatureID IN
(
      SELECT csfm.ServiceFeatureID
      FROM SecurityGroup sg
      JOIN ClientServiceMap csm ON csm.ClientID = sg.ClientID
      JOIN ClientServiceFeatureMap csfm ON csfm.ClientID = sg.ClientID
      LEFT OUTER JOIN ServiceFeatureAccessMap sfam on csfm.ServiceFeatureID = sfam.ServiceFeatureID
      WHERE  sg.SecurityGroupID = @SecurityGroupID and csfm.IsEnabled = 1 and csm.IsEnabled = 1  and sfam.ServiceFeatureAccessMapID is null 
      
      UNION
      
      SELECT sfam.ServiceFeatureID
      FROM SecurityGroup sg
      JOIN SecurityGroupPermission sgp ON sgp.SecurityGroupID = sg.SecurityGroupID
      JOIN ServiceFeatureAccessMap sfam ON sfam.ServiceFeatureAccessMapID = sgp.ServiceFeatureAccessMapID
      JOIN Access a ON a.AccessID = sfam.AccessID
      WHERE sg.SecurityGroupID = @SecurityGroupID and sg.IsActive = 1 and sgp.IsActive = 1 and sfam.IsEnabled = 1 and a.IsActive=1
)