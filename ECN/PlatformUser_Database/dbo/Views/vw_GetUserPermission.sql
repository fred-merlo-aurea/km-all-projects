CREATE view vw_GetUserPermission  
as  
SELECT
  ucsgm.userID,
  ucsgm.ClientID,
  s.ServiceID,
  s.ServiceCode,
  sf.ServiceFeatureID,
  sf.SFCode,
  a.AccessID,
  a.AccessCode
FROM UserClientSecurityGroupMap ucsgm
JOIN securityGroup sg
  ON sg.securitygroupID = ucsgm.securitygroupID
JOIN SecurityGroupPermission sgp
  ON sgp.securitygroupID = sg.securitygroupID
JOIN ServiceFeatureAccessMap sfam
  ON sfam.ServiceFeatureAccessMapID = sgp.ServiceFeatureAccessMapID
JOIN Access a 
  ON a.AccessID  = sfam.AccessID
JOIN ServiceFeature sf
  ON sf.ServiceFeatureID = sfam.ServiceFeatureID
JOIN Service s
  ON s.serviceID = sf.ServiceID
JOIN ClientServiceMap cam
  ON cam.ServiceID = sf.ServiceID
  AND cam.clientID = ucsgm.ClientID 
JOIN ClientServiceFeatureMap csfm
  on csfm.ServiceFeatureID = sf.ServiceFeatureID
	AND csfm.clientID = ucsgm.ClientID
 where
	cam.IsEnabled = 1 and
	sg.IsActive = 1 and
	ucsgm.IsActive = 1 and
	sgp.IsActive = 1 and
	sfam.IsEnabled = 1 and
	sf.IsEnabled = 1 and
	s.IsEnabled = 1 and
	csfm.IsEnabled = 1