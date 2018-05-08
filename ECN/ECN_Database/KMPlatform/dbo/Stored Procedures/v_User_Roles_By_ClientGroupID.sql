
CREATE PROCEDURE [dbo].[v_User_Roles_By_ClientGroupID]  
	@ClientGroupID int  
as	select u.UserID, u.UserName, 
		   u.Status, u.IsKMStaff, u.IsPlatformAdministrator, 
		   --u.FirstName, u.LastName, 
		   CASE WHEN LTRIM(RTRIM(ISNULL(u.FirstName,''))) <> '' AND LTRIM(RTRIM(ISNULL(u.LastName ,''))) <> '' THEN u.LastName + ', ' + u.LastName ELSE '' END Name,
	       uc.UserClientSecurityGroupMapID,
	       cgcm.ClientGroupClientMapID, cgcm.ClientGroupID,
	       c.ClientID, c.ClientName, 
	       s.SecurityGroupID, s.SecurityGroupName, s.AdministrativeLevel
	from [User] u with(nolock)
	join UserClientSecurityGroupMap uc with(nolock) on u.UserID = uc.UserID
	join ClientGroupClientMap cgcm with (nolock) on cgcm.ClientID = uc.ClientID
	join Client c with (nolock) on c.ClientID = cgcm.ClientID
	join SecurityGroup s with (nolock) on s.SecurityGroupID = uc.SecurityGroupID
	where 
			cgcm.ClientGroupID = @ClientGroupID and 
			cgcm.IsActive = 1 AND
			uc.IsActive = 1 and 
			u.Status <> 'DISABLED' and
			s.IsActive = 1