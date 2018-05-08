CREATE PROCEDURE [dbo].[e_Client_Select_ClientGroupID]
@ClientGroupID int
AS
	select distinct c.*
	from Client c with(nolock)
	join ClientGroupClientMap cgcm with(nolock) on c.ClientID = cgcm.ClientID
	where cgcm.ClientGroupID = @ClientGroupID
	order by c.ClientName