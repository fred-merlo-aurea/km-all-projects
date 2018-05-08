create procedure o_CustomFieldValue_Select_Consensus
as
BEGIN

	SET NOCOUNT ON

	select g.Name as 'Name',
		cs.MasterValue as 'Value',
		cs.MasterDesc as 'Description',
		isnull(cs.SortOrder, 0) as 'SortOrder'
	from MasterCodeSheet cs with(nolock)
		join MasterGroups g with(nolock) on cs.MasterGroupID = g.MasterGroupID
	order by g.Name,cs.SortOrder
end
go