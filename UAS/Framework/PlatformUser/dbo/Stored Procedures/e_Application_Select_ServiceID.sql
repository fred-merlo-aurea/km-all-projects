CREATE PROCEDURE [dbo].[e_Application_Select_ServiceID]
@ServiceID int
AS
	select distinct a.*
	from [Application] a with(nolock)
	join ApplicationServiceMap asm with(nolock) on a.ApplicationID = asm.ApplicationID
	where asm.ServiceID = @ServiceID
	order by a.ApplicationName
