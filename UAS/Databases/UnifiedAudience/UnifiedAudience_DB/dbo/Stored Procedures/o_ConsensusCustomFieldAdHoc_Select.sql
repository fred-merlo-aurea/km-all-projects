create procedure [dbo].[o_ConsensusCustomFieldAdHoc_Select]
as
BEGIN

	SET NOCOUNT ON

	--ConsensusAdHocs
	select 0 as 'ProductId', 'Consensus' as 'ProductCode',
		r.CustomField as 'Name',
		r.CustomField as 'DisplayName',
		0 as 'DisplayOrder',
		0 as 'IsMultipleValue',
		0 as 'IsRequired',
		isnull(r.Active,1) as 'IsActive',
		1 as 'IsAdHoc'
	from SubscriptionsExtensionMapper r with(nolock)
	order by r.CustomField
END
GO