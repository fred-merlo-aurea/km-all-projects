create procedure o_CustomFieldValue_Select_Product
as
BEGIN

	SET NOCOUNT ON

	select cs.PubID as 'ProductId',
		p.PubCode as 'ProductCode',
		cs.ResponseGroup as 'Name',
		cs.Responsevalue as 'Value',
		cs.Responsedesc as 'Description',
		isnull(cs.DisplayOrder, 0) as 'DisplayOrder',
		isnull(cs.IsOther, 0) as 'IsOther'
	from CodeSheet cs with(nolock)
		join Pubs p with(nolock) on cs.PubID = p.PubID
	order by p.PubCode,cs.ResponseGroup,cs.DisplayOrder

end
go