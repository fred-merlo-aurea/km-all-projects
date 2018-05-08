CREATE PROCEDURE [dbo].[e_Service_Select_SecurityGroup_ClientGroupID]
@SecurityGroupID int,
@ClientGroupID int
as
	select s.* 
	from service s with(nolock)
	join SecurityGroupServicMap sgsm with(nolock) on s.ServiceID = sgsm.ServiceID
	join ClientGroupServiceMap cgsm with(nolock) on sgsm.ServiceID = cgsm.ServiceID 
	where cgsm.ClientGroupID = @ClientGroupID
	and sgsm.SecurityGroupID = @SecurityGroupID
	and cgsm.IsEnabled = 'true'
	and sgsm.IsEnabled = 'true'
	and s.IsEnabled = 'true'
go