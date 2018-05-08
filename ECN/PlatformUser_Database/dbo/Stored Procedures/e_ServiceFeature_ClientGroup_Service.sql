CREATE PROCEDURE [dbo].[e_ServiceFeature_ClientGroup_Service]
@ClientGroupID int,
@ServiceID int
AS
	SELECT sf.*
	FROM 
	
		ClientGroup cg with(nolock)
		JOIN ClientGroupServiceMap cgsm with(nolock) on cg.ClientGroupID = cgsm.ClientGroupID
		JOIN ClientGroupServiceFeatureMap cgsf with(nolock) on cgsm.ClientGroupID = cgsf.ClientGroupID and cgsm.ServiceID = cgsf.ServiceID
		JOIN ServiceFeature sf With(NoLock)  on sf.ServiceFeatureID = cgsf.ServiceFeatureID
	WHERE cg.ClientGroupID = @ClientGroupID 
		AND cgsf.ServiceID = @ServiceID 
		AND cgsf.IsEnabled = 1 
		AND cg.IsActive = 1 
		AND cgsm.IsEnabled = 1