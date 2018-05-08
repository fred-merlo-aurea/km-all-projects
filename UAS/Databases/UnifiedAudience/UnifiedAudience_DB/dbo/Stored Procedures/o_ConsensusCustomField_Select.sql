create procedure [dbo].[o_ConsensusCustomField_Select]
as
BEGIN

	SET NOCOUNT ON

	--consensus
	select 0 as 'ProductId', 'Consensus' as 'ProductCode',
		r.Name as 'Name',
		r.DisplayName as 'DisplayName',
		isnull(r.SortOrder,0) as 'DisplayOrder',
		0 as 'IsMultipleValue',
		0 as 'IsRequired',
		isnull(r.IsActive, 1) as 'IsActive',
		0 as 'IsAdHoc'
	from MasterGroups r with(nolock)
	order by r.SortOrder

END
GO