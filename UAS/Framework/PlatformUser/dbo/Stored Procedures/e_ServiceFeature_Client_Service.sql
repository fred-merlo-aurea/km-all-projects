CREATE PROCEDURE [dbo].[e_ServiceFeature_Client_Service]
@ClientID int,
@ServiceID int
AS
	SELECT sf.*
	FROM 
	
		Client c with(nolock)
		JOIN ClientServiceMap csm with(nolock) on c.ClientID = csm.ClientID
		JOIN ClientServiceFeatureMap csfm with(nolock) on csfm.ClientID = c.ClientID 
		JOIN ServiceFeature sf With(NoLock)  on sf.ServiceFeatureID = csfm.ServiceFeatureID
	WHERE c.ClientID = @ClientID 
		AND csm.ServiceID = @ServiceID 
		AND sf.ServiceID = @ServiceID 
		AND csfm.IsEnabled = 1 
		AND c.IsActive = 1 
		AND csm.IsEnabled = 1
