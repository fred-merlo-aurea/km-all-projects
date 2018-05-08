create procedure e_ClientGroup_SelectForAMS
as
	select distinct cg.*
	from ClientGroup cg with(nolock)
	join ClientGroupClientMap m with(nolock) on cg.ClientGroupID = m.ClientGroupID
	join Client c with(nolock) on m.ClientID = c.ClientID
	where c.IsAMS = 'true'
go