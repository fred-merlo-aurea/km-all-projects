CREATE PROCEDURE [dbo].[e_SecurityGroup_Select_UserID_ClientID]
@UserID int,
@ClientID int
AS
	select top 1 sg.*
	from SecurityGroup sg with(nolock)
	join UserClientSecurityGroupMap ucsgm with(nolock) on sg.SecurityGroupID = ucsgm.SecurityGroupID
	where ucsgm.UserID = @UserID
	and ucsgm.ClientID = @ClientID
