CREATE proc [dbo].[e_RecordViewField_Save](
@MasterGroupID int,
@SubscriptionsExtensionMapperID int
)
as
BEGIN

	SET NOCOUNT ON

    insert into RecordViewField (MasterGroupID, SubscriptionsExtensionMapperID) 
	values (@MasterGroupID, @SubscriptionsExtensionMapperID)

End