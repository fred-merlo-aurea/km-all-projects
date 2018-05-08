CREATE PROCEDURE [dbo].[e_Service_Select_ClientGroupID]
@ClientGroupID int
AS
	SELECT s.*
	FROM 
		ClientGroup cg with(nolock)
		JOIN ClientGroupServiceMap cgsf with(nolock) on cg.ClientGroupID = cgsf.ClientGroupID
		JOIN Service s With(NoLock) on s.ServiceID = cgsf.ServiceID
	WHERE cg.ClientGroupID = @ClientGroupID 
		AND cgsf.IsEnabled = 1 
		AND cg.IsActive = 1
