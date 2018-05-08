CREATE PROCEDURE [dbo].[e_ApplicationServiceMap_Select_ServiceID]
@ServiceID int
AS
	select *
	from ApplicationServiceMap with(nolock)
	where ServiceID = @ServiceID