create procedure ccp_Meister_RemoveBadPhoneNumber
@SourceFileId int = 0,
@ProcessCode varchar(50) = '',
@ClientId int = 0
as
BEGIN

	set nocount on

	update sf
	set Phone = null
	from SubscriberFinal sf
	join UAS..AdHocDimension a on sf.Phone = a.MatchValue
	join UAS..AdHocDimensionGroup g on a.AdHocDimensionGroupId = g.AdHocDimensionGroupId 
	where sf.ProcessCode = @ProcessCode
	and g.AdHocDimensionGroupId = (select AdHocDimensionGroupId
								   from UAS..AdHocDimensionGroup with(nolock)
								   where AdHocDimensionGroupName = 'Meister_RemoveBadPhoneNumber')

END
go