CREATE PROCEDURE [dbo].[e_User_Select_ClientID]
@ClientID int
as
	select u.*
	from [User] u with(nolock)
	join UserClientSecurityGroupMap uc with(nolock) on u.UserID = uc.UserID
	where uc.ClientID = @ClientID and uc.IsActive = 1 and u.IsActive = 1 and u.Status <> 'DELETED'
	order by u.LastName,u.FirstName