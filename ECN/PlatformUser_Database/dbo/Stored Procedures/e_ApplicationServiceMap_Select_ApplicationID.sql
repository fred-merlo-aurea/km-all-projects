CREATE PROCEDURE [dbo].[e_ApplicationServiceMap_Select_ApplicationID]
@ApplicationID int
AS
	select *
	from ApplicationServiceMap with(nolock)
	where ApplicationID = @ApplicationID
GO
