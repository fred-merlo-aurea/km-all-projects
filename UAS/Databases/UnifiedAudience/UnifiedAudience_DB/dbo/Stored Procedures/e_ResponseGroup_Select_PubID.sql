create procedure e_ResponseGroup_Select_PubID
@PubID int
as
BEGIN

	SET NOCOUNT ON

	select rg.*,p.PubCode
	from ResponseGroups rg With(NoLock)
		join Pubs p with(nolock) on rg.PubID = p.PubID
	where rg.PubID = @PubID

END
go