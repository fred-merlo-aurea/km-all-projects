CREATE PROCEDURE [dbo].[e_Service_Select_ClientID]
@ClientID int
AS
	SELECT s.*
	FROM 
		Client c with(nolock)
		JOIN ClientServiceMap csm with(nolock) on c.ClientID = csm.ClientID
		JOIN Service s With(NoLock) on s.ServiceID = csm.ServiceID
	WHERE c.ClientID = @ClientID 
		AND csm.IsEnabled = 1 
		AND c.IsActive = 1