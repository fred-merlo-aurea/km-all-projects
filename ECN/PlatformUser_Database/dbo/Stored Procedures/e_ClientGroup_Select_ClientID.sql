CREATE PROCEDURE [dbo].[e_ClientGroup_Select_ClientID]
@ClientID int
AS
	select cg.*
	from ClientGroup cg with(nolock)
	join ClientGroupClientMap m with(nolock) on cg.ClientGroupID = m.ClientGroupID
	join Client c with(nolock) on c.ClientID = m.ClientID
	where m.ClientID = @ClientID
	order by cg.ClientGroupName
GO
