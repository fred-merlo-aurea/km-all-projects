create procedure o_Dimension_Select
as
BEGIN

	SET NOCOUNT ON

	select rg.ResponseGroupID,rg.ResponseGroupName,
		p.PubID as 'ProductId',p.PubCode as 'ProductCode',
		cs.CodeSheetID,cs.Responsevalue,
		p.PubCode + ' - ' + rg.ResponseGroupName as 'SubGenResponseGroupName'
	from ResponseGroups rg with(nolock)
		join CodeSheet cs with(nolock) on rg.ResponseGroupID = cs.ResponseGroupID
		join Pubs p with(nolock) on rg.PubID = p.PubID

END
go