create procedure [dbo].[o_ProductCustomField_Select]
as
BEGIN

	SET NOCOUNT ON

	--product
	select p.PubID as 'ProductId', p.PubCode as 'ProductCode',
		r.ResponseGroupName as 'Name',
		r.DisplayName as 'DisplayName',
		isnull(r.DisplayOrder, 0) as 'DisplayOrder',
		isnull(r.IsMultipleValue, 0) as 'IsMultipleValue',
		isnull(r.IsRequired, 0) as 'IsRequired',
		isnull(r.IsActive, 1) as 'IsActive',
		0 as 'IsAdHoc'
	from ResponseGroups r with(nolock)
		join Pubs p with(nolock) on r.PubID = p.PubID
	order by p.PubCode, r.DisplayOrder

END
GO