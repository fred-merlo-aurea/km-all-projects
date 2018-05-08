CREATE PROCEDURE [dbo].[e_User_Select_ClientID_SecurityGroupID]
@ClientID int,
@SecurityGroupID int
as
	select u.*
	from [User] u with(nolock)
	join UserClientSecurityGroupMap ucsgm with(nolock) on u.UserID = ucsgm.UserID 
	where ucsgm.ClientID = @ClientID 
	and ucsgm.SecurityGroupID = @SecurityGroupID
	and u.IsActive = 'true'
