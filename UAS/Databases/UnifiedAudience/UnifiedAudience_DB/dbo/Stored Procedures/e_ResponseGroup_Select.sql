create procedure e_ResponseGroup_Select
as
BEGIN

	SET NOCOUNT ON

	select rg.*,p.PubCode
	from ResponseGroups rg With(NoLock) 
		join Pubs p with(nolock) on rg.PubID = p.PubID
	order by DisplayName

END
go