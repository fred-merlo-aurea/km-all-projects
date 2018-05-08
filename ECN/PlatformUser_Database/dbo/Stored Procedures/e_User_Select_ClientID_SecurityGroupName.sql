CREATE PROCEDURE [dbo].[e_User_Select_ClientID_SecurityGroupName]
@ClientID int,
@SecurityGroupName varchar(50)
as
	declare @SecurityGroupID int = (select distinct sg.SecurityGroupID
									from ClientGroupClientMap cgcm with(nolock)
									join ClientGroup cg with(nolock) on cgcm.ClientGroupID = cg.ClientGroupID
									join SecurityGroup sg with(nolock) on sg.ClientID = cgcm.ClientID 
									where cgcm.ClientID = @ClientID 
									and sg.SecurityGroupName = @SecurityGroupName)
	select u.*
	from [User] u
	join UserClientSecurityGroupMap ucsgm on u.UserID = ucsgm.UserID 
	where ucsgm.ClientID = @ClientID 
	and ucsgm.SecurityGroupID = @SecurityGroupID
	and u.IsActive = 'true'
go
