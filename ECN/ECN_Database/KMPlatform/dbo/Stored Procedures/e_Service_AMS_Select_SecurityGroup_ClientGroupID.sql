CREATE PROCEDURE [dbo].[e_Service_AMS_Select_SecurityGroup_ClientGroupID]
@SecurityGroupID int,
@ClientGroupID int
as
	select s.* 
	from Service s with(nolock)
	join ClientGroupServiceMap cgsm with(nolock) on s.ServiceID = cgsm.ServiceID
	join SecurityGroup sg with(nolock) on sg.ClientGroupID = cgsm.ClientGroupID
	where cgsm.ClientGroupID = @ClientGroupID
	and sg.SecurityGroupID = @SecurityGroupID
	and cgsm.IsEnabled = 'true'
	and sg.IsActive = 'true'
	and s.IsEnabled = 'true'