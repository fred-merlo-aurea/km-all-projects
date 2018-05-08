create procedure [dbo].[o_ProductCustomFieldAdHoc_Select]
as
BEGIN

	SET NOCOUNT ON

	--ProductAdHocs
	select p.PubID as 'ProductId', p.PubCode as 'ProductCode',
		r.CustomField as 'Name',
		r.CustomField as 'DisplayName',
		0 as 'DisplayOrder',
		0 as 'IsMultipleValue',
		0 as 'IsRequired',
		isnull(r.Active,1) as 'IsActive',
		1 as 'IsAdHoc'
	from PubSubscriptionsExtensionMapper r with(nolock)
		join Pubs p with(nolock) on r.PubID = p.PubID
	order by p.PubCode

END
GO