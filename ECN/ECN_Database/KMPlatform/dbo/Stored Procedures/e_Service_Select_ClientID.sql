CREATE PROCEDURE [dbo].[e_Service_Select_ClientID]
@ClientID int
AS
	SELECT s.*
	FROM 
		Client c with(nolock)
		JOIN ClientServiceMap csm with(nolock) on c.ClientID = csm.ClientID
		JOIN Service s With(NoLock) on s.ServiceID = csm.ServiceID
		JOIN ClientGroupClientMap cgcm with(nolock) on c.ClientID = cgcm.ClientID
		JOIN ClientGroupServiceMap cgsm with(nolock) on cgcm.ClientGroupID = cgsm.ClientGroupID
		JOIN Service s2 with(nolock)on cgsm.ServiceID = s2.ServiceID and s.ServiceID = s2.ServiceID
	WHERE c.ClientID = @ClientID 
		AND csm.IsEnabled = 1 
		AND c.IsActive = 1
		AND cgsm.IsEnabled = 1
		AND cgcm.IsActive = 1