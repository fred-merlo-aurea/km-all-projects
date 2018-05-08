create procedure [dbo].[o_BrandCustomField_Select]
as
BEGIN

	SET NOCOUNT ON

	--Brand
	select Distinct b.BrandID as 'BrandId', b.BrandName as 'BrandName', b.IsDeleted as 'IsBrandDeleted',
		r.Name as 'CustomFieldName',
		r.DisplayName as 'CustomFieldDisplayName',
		0 as 'DisplayOrder',
		isnull(r.IsActive,0)
	from vw_BrandConsensus v with(nolock)
		join Brand b with(nolock) on v.BrandID = b.BrandID
		join MasterGroups r with(nolock) on r.MasterGroupID = v.MasterID
	order by b.BrandName, r.Name
END
GO