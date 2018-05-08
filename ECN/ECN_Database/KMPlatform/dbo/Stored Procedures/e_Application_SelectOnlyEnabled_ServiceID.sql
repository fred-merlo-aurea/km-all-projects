CREATE PROCEDURE [dbo].[e_Application_SelectOnlyEnabled_ServiceID]
@ServiceID int
AS
	select distinct a.*
	from [Application] a with(nolock)
	join ApplicationServiceMap asm with(nolock) on a.ApplicationID = asm.ApplicationID
	where asm.ServiceID = @ServiceID
	and a.IsActive = 'true'
	and asm.IsEnabled = 'true'
	order by a.ApplicationName