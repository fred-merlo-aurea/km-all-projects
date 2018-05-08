CREATE PROCEDURE [dbo].[e_Client_SelectActive_ClientGroupID]
@ClientGroupID int
AS
	select distinct c.*
	from Client c with(nolock)
	join ClientGroupClientMap cgcm with(nolock) on c.ClientID = cgcm.ClientID
	where cgcm.ClientGroupID = @ClientGroupID
	and cgcm.IsActive = 'true'
	order by c.ClientName
