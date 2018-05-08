CREATE PROCEDURE [dbo].[e_User_Select_ClientGroupID]  
@ClientGroupID int  
as  
	select u.*
	from [User] u with(nolock)
	join UserClientSecurityGroupMap uc with(nolock) on u.UserID = uc.UserID
	join ClientGroupClientMap cgcm with (nolock) on cgcm.ClientID = uc.ClientID
	where 
			cgcm.ClientGroupID = @ClientGroupID and 
			uc.IsActive = 1 and 
			u.IsActive = 1 and u.Status <> 'DISABLED' and 
			cgcm.IsActive = 1
order by u.LastName,u.FirstName