create procedure ccp_Meister_ExcelDemoOptOuts
@ProcessCode varchar(50)
as
BEGIN

	set nocount on

	update sf
	set OtherProductsPermission = 0
	from SubscriberFinal sf
	join UAS..AdHocDimension a on sf.Email = a.MatchValue
	join UAS..AdHocDimensionGroup g on a.AdHocDimensionGroupId = g.AdHocDimensionGroupId 
	where sf.ProcessCode = @ProcessCode
	and g.AdHocDimensionGroupId = (select AdHocDimensionGroupId
								   from UAS..AdHocDimensionGroup with(nolock)
								   where AdHocDimensionGroupName = 'Meister_CventOptOuts')

END
go